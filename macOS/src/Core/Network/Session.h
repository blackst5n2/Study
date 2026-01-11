#pragma once
#include <cstdint>
#include <sys/socket.h>
#include "PacketHeader.h"
#include <memory>
#include "SendMirrorRingBuffer.h"
#include "../Logger.h"

namespace Core {

struct SessionMetrics {
    std::atomic<size_t> highWatermark{0};
    std::atomic<size_t> overflowCount{0};
    std::atomic<size_t> spinCountTotal{0}; // 스레드 경합 측정용
    std::atomic<uint32_t> sendCallCount{0};

    void UpdateUsage(size_t usage) {
        size_t currentHW = highWatermark.load(std::memory_order_relaxed);
        while (usage > currentHW && 
                !highWatermark.compare_exchange_weak(currentHW, usage, std::memory_order_relaxed));
    }

    void AddOverflow() { overflowCount.fetch_add(1, std::memory_order_relaxed); }
    void AddSpin(uint32_t spins) { spinCountTotal.fetch_add(spins, std::memory_order_relaxed); }
};

class alignas(64) Session : public std::enable_shared_from_this<Session> {
public:
    Session();
    ~Session();
    
    void Init(uint64_t sessionId, int fd);
    bool IsActive() const { return m_active.load(std::memory_order_acquire); }

    void Reset();

    uint64_t GetID() const { return m_sessionId.load(std::memory_order_relaxed); }
    void Invalidate() { m_active.store(false, std::memory_order_release); }

    bool IsValid(uint64_t id) const { return m_active.load(std::memory_order_acquire) && m_sessionId.load(std::memory_order_relaxed) == id; } 
    uint64_t GetSessionId() const { return m_sessionId.load(std::memory_order_relaxed); }
    int GetFd() const { return m_fd; }

    // 워커가 호출
    void Send(const uint8_t* data, uint32_t len);

    // 실제 전송
    void FlushSend();

    // backlog 관리
    void SaveBacklog(uint8_t* data, uint32_t size);
    uint8_t* GetBacklogPtr() { return m_backLogBuffer; }
    uint32_t GetBacklogSize() { return m_backLogSize; }
    void ClearBacklog() { m_backLogSize = 0; } // 메모리는 유지하고 인덱스만 초기화
    bool HasBacklog() const { return m_backLogSize > 0; }

    // batching
    void SetNeedsFlush(bool need) { m_needsFlush.store(need, std::memory_order_release); }
    bool IsNeedsFlush() const { return m_needsFlush.load(std::memory_order_acquire); }
    std::atomic<bool>& GetIsFlushScheduled() { return m_isFlushScheduled; }

    // kqueue 상태 관리
    void UpdateLastActiveTime();
    uint64_t GetLastActiveTime() const { return m_lastActiveTime; }

    void DisableWriteEvent();

    void PrintReport();

private:
    void ForceClose(); // 세션 종료 및 자원 정리

    int m_fd = -1;
    std::atomic<uint64_t> m_sessionId{ 0 };
    std::atomic<bool> m_active{false};
    uint64_t m_lastActiveTime;
    std::atomic<bool> m_isWriteEventRegistered{false};

    SendMirrorRingBuffer m_sendBuffer;

    alignas(64) std::atomic<uint64_t> m_reservePos = {0}; // 예약 커서
    alignas(64) std::atomic<uint64_t> m_commitPos = {0}; // 실제 사용 가능 커서

    std::atomic<bool> m_isFlushScheduled{false}; // 중복 큐잉 방지 플래그
    std::atomic<bool> m_needsFlush{false};

    uint8_t* m_backLogBuffer = nullptr;
    uint32_t m_backLogSize = 0;
    uint32_t m_backLogCapacity = 0;

    SessionMetrics m_metrics;
};

}
