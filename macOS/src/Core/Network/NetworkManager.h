#pragma once
#include <sys/event.h>
#include "../Memory/ArenaAllocator.h"
#include "Session.h"
#include "SessionManager.h"
#include "RecvMirrorRingBuffer.h"
#include "../Threading/ThreadPool.h"
#include "MPMCQueue.h"
#include "../Logger.h"

namespace Core {

class NetworkManager {
const int RECV_QUEUE_SIZE = 65536;
const int SEND_QUEUE_SIZE = 32678;

public:
    bool Init(int port);
    void Shutdown();
    void Update(); // 엔진 메인 루프에서 호출될 이벤트 처리기
    void Close(int fd);

    // 전역 접근을 위한 싱글톤 (필요시)
    static NetworkManager& GetInstance() {
        static NetworkManager instance;
        return instance;
    }

    const int GetKqueueFd() const;

    void RegisterWriteEvent(int fd, Session* session);

    ThreadPool& GetThreadPool() { return *m_threadPool; }
    SessionManager& GetSessionManager() { return *m_sessionManager; }
    MPMCQueue<PacketDescriptor>* GetRecvQueue() { return &m_recvQueue; }
    RecvMirrorRingBuffer& GetRecvBuffer() { return m_recvBuffer; }
    void PushSendRequest(const SendDescriptor& desc) {
        m_sendQueue.push(desc);
    }

    void AddConsumedBytes(size_t len) {
        m_pendingReadBytes.fetch_add(len, std::memory_order_relaxed);
    }
    
    void PrintGlobalReport();

private:
    NetworkManager();
    ~NetworkManager();

    void HandleNewConnection();
    void HandleDisconnect(int fd, Session* session);
    
    void SetNonBlocking(int fd);
    void ModifyEvent(int fd, int16_t filter, uint16_t flags, void* udata);

private:
    int m_serverSocket = -1;
    int m_kq = -1; // kqueue 핸들
    bool m_isRunning = false;

    // kqueue 이벤트 등록 및 수신용 버퍼
    static constexpr int MAX_EVENTS = 1024;
    struct kevent m_eventList[MAX_EVENTS];

    // 미러링 버퍼
    RecvMirrorRingBuffer m_recvBuffer;

    // 추가 되었지만 커서에 반영 되지 않은 바이트 수
    std::atomic<size_t> m_pendingReadBytes{0};

    // MPMC 큐
    MPMCQueue<PacketDescriptor> m_recvQueue;
    MPMCQueue<SendDescriptor> m_sendQueue;

    // 시스템 구성 요소
    ArenaAllocator* m_networkAllocator = nullptr;
    SessionManager* m_sessionManager = nullptr;
    
    // 스레드풀
    ThreadPool* m_threadPool;

    // 타임아웃
    struct timespec m_loopTimeout;
};

}
