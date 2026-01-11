#include "Session.h"
#include "PacketDispatcher.h"
#include "NetworkManager.h"
#include <iostream>
#include <chrono>

namespace Core {

Session::Session() {
    m_backLogBuffer = nullptr;
}

Session::~Session() {
    if (m_backLogBuffer) {
        
    }
}

void Session::Init(uint64_t sessionId, int fd) {
    // 1. 상태 변수 초기화
    m_sessionId.store(sessionId, std::memory_order_relaxed);
    printf("[Debug] Session Init: ID=%llu, FD=%d\n", sessionId, fd);
    m_fd = fd;
    
    // 커서 초기화
    m_reservePos.store(0, std::memory_order_relaxed);
    m_commitPos.store(0, std::memory_order_relaxed);
    m_isFlushScheduled.store(false, std::memory_order_relaxed);
    m_needsFlush.store(false, std::memory_order_relaxed);
    m_isWriteEventRegistered.store(false, std::memory_order_relaxed);
    
    // 백로그 초기화
    m_backLogSize = 0;

    // 2. 링 버퍼 메모리 매핑
    void* mirrorPtr = MemoryManager::Get().CreatePlaceholder();
    // m_sendBuffer 내부에서도 Read/Write Pos를 0으로 리셋하는지 확인 필요
    m_sendBuffer.Init(mirrorPtr, MemoryManager::Get().GetSendBufferSize());

    // 3. 마지막으로 활성화
    m_active.store(true, std::memory_order_release);

    UpdateLastActiveTime();
}

void Session::Reset() {
    m_active.store(false, std::memory_order_release);

    PrintReport();

    if (m_backLogBuffer) {
        Core::Free(m_backLogBuffer);
        m_backLogBuffer = nullptr;
        m_backLogCapacity = 0;
        m_backLogSize = 0;
    }
    
    if (m_fd != -1) {
        close(m_fd);
        m_fd = -1;
    }

    if (m_sendBuffer.getBasePtr()) {
        munmap(m_sendBuffer.getBasePtr(), MemoryManager::Get().GetSendBufferSize() * 2);
    }

    m_metrics.highWatermark.store(0);
    m_metrics.overflowCount.store(0);
    m_reservePos.store(0);
    m_commitPos.store(0);

    // 3. 내부 카운터 리셋
    m_reservePos.store(0);
    m_commitPos.store(0);
}

void Session::Send(const uint8_t* data, uint32_t len) {
    if (!IsActive()) return;

    // 1. 공간 예약 (Atomic Fetch-Add)
    uint64_t myStart = m_reservePos.fetch_add(len, std::memory_order_relaxed);
    
    // 2. [최적화] Smart Spin-wait
    uint32_t spinCount = 0;
    while (m_commitPos.load(std::memory_order_acquire) != myStart) {
        // 하드웨어에 spin-loop 중임을 알려 전력 및 리소스 최적화 (PAUSE 명령)
#if defined(__x86_64__) || defined(_M_X64)
        __builtin_ia32_pause(); 
#elif defined(__arm64__) || defined(__aarch64__)
        asm volatile("yield" ::: "memory");
#endif

        if (++spinCount > 1000) { // 일정 횟수 이상 경합 시
            std::this_thread::yield(); // 다른 스레드에게 양보
            spinCount = 0;
            m_metrics.AddSpin(1000); // 지표 기록
        }
    }

    // 3. 실제 복사 (Mirror Ring Buffer라 경계 처리 불필요)
    m_sendBuffer.WriteAt(myStart, data, len);

    // 4. 커밋 (내가 쓴 만큼 다음 사람에게 차례를 넘김)
    m_commitPos.store(myStart + len, std::memory_order_release);

    bool expected = false;
    if (m_isFlushScheduled.compare_exchange_strong(expected, true)) {
        NetworkManager::GetInstance().PushSendRequest({ m_sessionId.load() });
    }
}

void Session::FlushSend() {
    m_isFlushScheduled.store(false, std::memory_order_release);

    while (true) {
        uint64_t committed = m_commitPos.load(std::memory_order_acquire);
        uint64_t sent = m_sendBuffer.GetReadPos();

        if (committed <= sent) {
            if (m_isWriteEventRegistered.load()) DisableWriteEvent();
            return;
        }

        uint32_t sizeToSend = static_cast<uint32_t>(committed - sent);
        ssize_t sentBytes = ::send(m_fd, m_sendBuffer.GetReadPtr(sent), sizeToSend, 0);

        if (sentBytes > 0) {
            m_sendBuffer.MoveReadPos(sent + sentBytes);
            if (m_sendBuffer.GetReadPos() < committed) continue;
            if (m_isWriteEventRegistered.load()) DisableWriteEvent();
            break;
        } else if (sentBytes == -1) {
            int err = errno; // 에러 번호 보존
            if (err == EAGAIN || err == EWOULDBLOCK || err == EINTR) {
                // 커널 버퍼 꽉 참: 쓰기 이벤트 등록 후 종료
                if (!m_isWriteEventRegistered.exchange(true)) {
                    NetworkManager::GetInstance().RegisterWriteEvent(m_fd, this);
                }
            } else {
                // 실제 심각한 에러 (EPIPE, ECONNRESET 등)
                LOG_ERR("FlushSend Critical Error: FD=", m_fd, " errno=", err);
                ForceClose();
            }
            break;
        }
    }
}

void Session::UpdateLastActiveTime() {
    auto now = std::chrono::system_clock::now();
    m_lastActiveTime = std::chrono::duration_cast<std::chrono::milliseconds>(
        now.time_since_epoch()).count();
}

void Session::DisableWriteEvent() {
    struct kevent ev;
    EV_SET(&ev, m_fd, EVFILT_WRITE, EV_DELETE, 0, 0, nullptr);
    if(kevent(NetworkManager::GetInstance().GetKqueueFd(), &ev, 1, nullptr, 0, nullptr) == -1) {
        // 이미 지워졌을 경우 에러가 날 수 있으니 무시
    }
    m_isWriteEventRegistered.store(false);
}

// --- Backlog 관리 (동적 할당 구현) ---
void Session::SaveBacklog(uint8_t* data, uint32_t size) {
    if (size == 0) return;

    // 1. 공간이 부족한 경우 확장
    if (m_backLogSize + size > m_backLogCapacity) {
        // 현재 크기의 2배 또는 필요한 만큼 계산
        uint32_t newCapacity = (m_backLogCapacity == 0) ? 4096 : m_backLogCapacity * 2;
        while (newCapacity < m_backLogSize + size) {
            newCapacity *= 2;
        }

        // Network 태그를 사용하여 할당
        uint8_t* newBuffer = static_cast<uint8_t*>(Core::Alloc(newCapacity, MemoryTag::Network));
        
        if (newBuffer) {
            // 기존 데이터 복사
            if (m_backLogBuffer && m_backLogSize > 0) {
                memcpy(newBuffer, m_backLogBuffer, m_backLogSize);
                // 기존 버퍼 해제
                Core::Free(m_backLogBuffer);
            }
            m_backLogBuffer = newBuffer;
            m_backLogCapacity = newCapacity;
        } else {
            LOG_ERR("Backlog allocation failed!");
            ForceClose();
            return;
        }
    }

    // 2. 데이터 복사
    memcpy(m_backLogBuffer + m_backLogSize, data, size);
    m_backLogSize += size;
    m_metrics.UpdateUsage(m_backLogSize);
}

void Session::ForceClose() {
    // 소켓을 강제로 닫아 kqueue가 EOF를 감지하게 함
    if (m_fd != -1) {
        shutdown(m_fd, SHUT_RDWR);
    }
}

void Session::PrintReport() {
    size_t hw = m_metrics.highWatermark.load();
    size_t ov = m_metrics.overflowCount.load();
    uint32_t totalSpins = m_metrics.spinCountTotal.load();
    uint32_t calls = m_metrics.sendCallCount.load();
    size_t cap = MemoryManager::Get().GetSendBufferSize();

    printf("\n--- Session %llu Report ---\n", m_sessionId.load());
    printf("Buffer Capacity: %zu bytes\n", cap);
    printf("High Watermark:  %zu bytes (%.2f%%)\n", hw, (double)hw / cap * 100.0);
    printf("Overflow Count:  %zu\n", ov);
    printf("Avg Spin/Send:   %.2f\n", calls > 0 ? (double)totalSpins / calls : 0);
    printf("Current Backlog: %u bytes\n", m_backLogSize);
    printf("---------------------------\n");
}

}
