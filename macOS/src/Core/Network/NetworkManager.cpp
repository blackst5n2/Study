#include "NetworkManager.h"
#include "PacketDispatcher.h"
#include "../Memory/Memory.h"
#include <sys/socket.h>
#include <netinet/in.h>
#include <fcntl.h>
#include <unistd.h>
#include <iostream>
#include <chrono>
#include "../Threading/ThreadPool.h"

namespace Core {

NetworkManager::NetworkManager() : m_recvQueue(RECV_QUEUE_SIZE), m_sendQueue(SEND_QUEUE_SIZE) {

}

NetworkManager::~NetworkManager() {
    if (m_kq != -1) close(m_kq);
    if (m_serverSocket != -1) close(m_serverSocket);
}

bool NetworkManager::Init(int port) {
    if (m_isRunning) return true;

    // 스레드 설정
    unsigned int threadCount = std::thread::hardware_concurrency();
    if (threadCount == 0) threadCount = 4; // 가본값 방어
    
    // ThreadPool 생성
    void* tmem = MemoryManager::Get().Allocate(sizeof(ThreadPool), MemoryTag::Permanent);
    m_threadPool = new (tmem) ThreadPool(threadCount);

    void* mem = MemoryManager::Get().Allocate(sizeof(SessionManager), MemoryTag::GameLogic);
    m_sessionManager = new (mem) SessionManager();
    
    // 수신용 미러 링버퍼 초기화
    void* recvBufferAddr = MemoryManager::Get().GetRecvPlaceholder();
    if (!recvBufferAddr) {
        LOG_ERR("Failed to get Recv Placeholder from MemoryManager");
        return false;
    }
    m_recvBuffer.Init(recvBufferAddr, MemoryManager::Get().GetRecvBufferSize());

    // 1. 서버 소켓 생성 (IPv4, TCP)
    m_serverSocket = socket(AF_INET, SOCK_STREAM, 0);
    if (m_serverSocket == -1) return false;
    
    // 2. 주소 재사용 옵션 (서버 재시작 시 포트 바인딩 에러 방지)
    int opt = 1;
    setsockopt(m_serverSocket, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(opt));
    
    // 3. 논블로킹 모드 설정 (핵심!)
    SetNonBlocking(m_serverSocket);
    
    // 4. 바인딩 및 리슨
    sockaddr_in addr{};
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = INADDR_ANY;
    addr.sin_port = htons(port);
    
    if (bind(m_serverSocket, (struct sockaddr*)&addr, sizeof(addr)) == -1) return false;
    if (listen(m_serverSocket, SOMAXCONN) == -1) return false;
    
    // 5. kqueue 핸들 생성
    m_kq = kqueue();
    if (m_kq == -1) return false;
    
    // 6. 서버 소켓의 READ 이벤트를 kqueue에 등록
    ModifyEvent(m_serverSocket, EVFILT_READ, EV_ADD | EV_ENABLE, nullptr);
    
    // 스레드 풀 가동
    m_threadPool->Start();
    
    m_isRunning = true;
    std::cout << "[Network] ThreadPool Initialized with " << threadCount << " threads." << std::endl;
    std::cout << "[Network] Kqueue Server Initialized on Port " << port << std::endl;
    
    return m_isRunning;
}

void NetworkManager::Shutdown() {
    
}

void NetworkManager::Update() {
    if (!m_isRunning) return;

    // 1. 이벤트 수집 (비차단 모드: timeout 0)
    m_loopTimeout.tv_sec = 0;
    m_loopTimeout.tv_nsec = 0;

    // 워커 스레드들이 처리한 양만큼 Read 커서를 통합해서 밀어줌 (안전함)
    size_t consumed = m_pendingReadBytes.exchange(0, std::memory_order_relaxed);
    if (consumed > 0) {
        m_recvBuffer.advanceReadPtr(consumed);
    }

    int eventCount = kevent(m_kq, nullptr, 0, m_eventList, MAX_EVENTS, &m_loopTimeout);
    
    for (int i = 0; i < eventCount; ++i) {
        struct kevent& ev = m_eventList[i];
        int fd = static_cast<int>(ev.ident);
        
        if (fd == m_serverSocket) {
            HandleNewConnection();
            continue;
        }

        Session* session = static_cast<Session*>(ev.udata);
        // 이미 세션매니저에서 Reset된 세션이 kqueue 목록에 남아있을 수 있음
        if (!session || !session->IsActive() || session->GetFd() != fd) continue;

        // [3] READ 처리
        if (ev.filter == EVFILT_READ) {
            bool queueFull = false;
            while (true) {
                void* writePtr = m_recvBuffer.getWritePtr();
                size_t writable = m_recvBuffer.getWritableSize();
                if (writable == 0) break;

                ssize_t bytesRead = ::recv(fd, writePtr, writable, 0);
                
                if (bytesRead > 0) {                    
                    PacketDescriptor desc{ reinterpret_cast<uintptr_t>(writePtr), (uint32_t)bytesRead, session->GetID() };
                    if (!m_recvQueue.push(desc)) break;
                    m_recvBuffer.advanceWritePtr(bytesRead); 
                } else if (bytesRead == -1) {
                    if (errno == EAGAIN || errno == EWOULDBLOCK) break; 
                    if (errno == EINTR) continue; // 인터럽트면 재시도
                    HandleDisconnect(fd, session); break;
                } else { // bytesRead == 0 (정상 종료)
                    HandleDisconnect(fd, session); break;
                }
            }
        }

        // [4] WRITE 처리
        if (ev.filter == EVFILT_WRITE) {
            Session* session = static_cast<Session*>(ev.udata);
            if (session && session->IsActive() && session->GetFd() == fd) {
                session->FlushSend();
            }
        }
    }
    
    SendDescriptor desc;
    while (m_sendQueue.pop(desc)) {
        Session* session = m_sessionManager->GetSession(desc.sessionId);
        if (session) {
            session->FlushSend(); 
        }
    }

    // [통계 출력 로직]
    static auto lastReportTime = std::chrono::steady_clock::now();
    auto currentTime = std::chrono::steady_clock::now();
    
    if (currentTime - lastReportTime >= std::chrono::milliseconds(2000)) {
        PrintGlobalReport();
        lastReportTime = currentTime;
    }
}

void NetworkManager::HandleNewConnection() {
    while (true) { // 더 이상 연결이 없을 때까지 계속 accept
        struct sockaddr_in clientAddr;
        socklen_t clientLen = sizeof(clientAddr);
        int clientSocket = accept(m_serverSocket, (struct sockaddr*)&clientAddr, &clientLen);
        
        if (clientSocket == -1) {
            if (errno == EAGAIN || errno == EWOULDBLOCK) break;
            break;
        }

        SetNonBlocking(clientSocket);
        Session* newSession = m_sessionManager->CreateSession(clientSocket);
        
        if (newSession) {
            ModifyEvent(clientSocket, EVFILT_READ, EV_ADD | EV_ENABLE, newSession);
        } else {
            ::close(clientSocket);
        }
    }
}

void NetworkManager::HandleDisconnect(int fd, Session* session) {
    // 1. kqueue에서 제거 (자원 정리 시작)
    ModifyEvent(fd, EVFILT_READ, EV_DELETE, NULL);
    
    if (session) {
        uint64_t id = session->GetID();
        LOG_INFO("Connection Closing: ID=", id, " FD=", fd);
        
        // 3. 고유 ID를 전달하여 안전하게 제거
        m_sessionManager->RemoveSession(id); 
    } else {
        // 이미 지워졌거나 없는 경우 소켓만 닫음
        ::close(fd);
    }
}

void NetworkManager::SetNonBlocking(int fd) {
    int flags = fcntl(fd, F_GETFL, 0);
    fcntl(fd, F_SETFL, flags | O_NONBLOCK);
}

void NetworkManager::ModifyEvent(int fd, int16_t filter, uint16_t flags, void* udata) {
    struct kevent ev;
    EV_SET(&ev, fd, filter, flags, 0, 0, udata);
    kevent(m_kq, &ev, 1, NULL, 0, NULL);
}

void NetworkManager::RegisterWriteEvent(int fd, Session* session) {
    // EV_ADD | EV_ENABLE을 통해 WRITE 이벤트를 감시 목록에 넣음.
    ModifyEvent(fd, EVFILT_WRITE, EV_ADD | EV_ENABLE, session);
}

const int NetworkManager::GetKqueueFd() const {
    return m_kq;
}

void NetworkManager::Close(int fd) {
    ::close(fd);
}

void NetworkManager::PrintGlobalReport() {
    printf("\n========== [Network Global Report] ==========\n");
    
    // 1. 글로벌 수신 버퍼 통계
    auto& rm = m_recvBuffer.getMetrics();
    printf("[Recv Buffer] Max Usage: %zu / %zu bytes (%.2f%%)\n",
           rm.highWatermark.load(), 
           MemoryManager::Get().GetRecvBufferSize(),
           (double)rm.highWatermark.load() / MemoryManager::Get().GetRecvBufferSize() * 100.0);

    // 2. MPMC 큐 상태 (수신 큐, 송신 큐)
    printf("[Recv MPMC] High Watermark Depth: %zu\n", m_recvQueue.GetMetrics().highWatermark.load());
    printf("[Send MPMC] High Watermark Depth: %zu\n", m_sendQueue.GetMetrics().highWatermark.load());

    printf("==============================================\n");
}

}


