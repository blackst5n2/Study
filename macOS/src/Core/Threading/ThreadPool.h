#pragma once
#include <thread>
#include <condition_variable>
#include "../Network/MPMCQueue.h"
#include "../Memory/LinearAllocator.h"
#include "../Network/Session.h"

namespace Core {

class ThreadPool {
public:
    ThreadPool(size_t threadCount);
    ~ThreadPool();

    void Start();
    
private:
    void WorkerThread(); // 각 스레드가 실행할 루프
    void ProcessPacket(Session* session, const PacketDescriptor desc);
    void HandleBusinessLogic(Session* session, uint8_t* raw_data, uint32_t size);

    std::thread* m_workers;
    size_t m_threadCount;
};

}
