#include "ThreadManager.h"
#include "LocalArena.h"
#include "LinearArena.h"
#include <iostream>
#include <thread>
#include "../Logger.h"

#if defined(__APPLE__)
    #include <mach/thread_policy.h>
    #include <mach/thread_act.h>
    #include <pthread.h>
#elif defined(__linux__)
    #include <pthread.h>
#endif

namespace Core {

// TLS 변수 정의 (0은 보통 메인 스레드나 미할당 상태로 사용)
thread_local ThreadId t_threadId = 0;
thread_local LocalArena* t_localArena = nullptr;
thread_local LinearArena* t_linearArena = nullptr;
thread_local flatbuffers::FlatBufferBuilder* t_fbb = nullptr;

void ThreadManager::InitThread() {
    // 1부터 시작하는 고유 ID 부여 (Atomic)
    t_threadId = ++m_threadCount;

    // --- LocalArena ---
    // 1. 전역 힙에서 메모리 확보
    size_t totalNeeded = sizeof(LocalArena) + LOCAL_SLAB_SIZE;
    void* rawMemory = MemoryManager::Get().GetRawSlab(totalNeeded);
    if (!rawMemory) {
        LOG_ERR("Critical: Global Heap Exhausted while creating LocalArena for Thread ", (int)t_threadId);
        return;
    }
    
    // 해당 스레드 전용 로컬 아레나 생성 (소매상 오픈)
    void* slabPtr = static_cast<uint8_t*>(rawMemory) + sizeof(LocalArena);
    
    t_localArena = new (rawMemory) LocalArena(t_threadId, slabPtr, LOCAL_SLAB_SIZE);

    // --- Linear Arena ---
    size_t total = sizeof(LinearArena) + LINEAR_SLAB_SIZE;
    void* rawMem = MemoryManager::Get().GetRawSlab(total);
    if (!rawMem) {
        LOG_ERR("Critical: Global Heap Exhausted while creating LinearArena for Thread", (int)t_threadId);
        return;
    }

    void* sPtr = static_cast<uint8_t*>(rawMem) + sizeof(LinearArena);
    
    t_linearArena = new (rawMem) LinearArena(t_threadId, sPtr, LINEAR_SLAB_SIZE);
    
    // --- fbb ---
    size_t fbbSize = 4096;
    void* fMem = MemoryManager::Get().GetRawSlab(sizeof(flatbuffers::FlatBufferBuilder));
    if (!fMem) {
        LOG_ERR("Critical: Global Heap Exhausted while creating fbb for Thread", (int)t_threadId);
        return;
    }
    
    t_fbb = new (fMem) flatbuffers::FlatBufferBuilder(fbbSize);

    // Qos, 피닝 적용
    ApplyThreadAffinity();

    // 전역 테이블에 내 아레나 등록
    GetInstance().RegisterLocalArena(t_threadId, t_localArena);
    GetInstance().ResgisterLinearArena(t_threadId, t_linearArena);

    LOG_INFO("LocalArena [Thread-", (int)t_threadId, "] created at ", rawMemory, " with Placement New");
    LOG_INFO("LinearArena [Thread-", (int)t_threadId, "] created at ", rawMem, " with Placement New");
}

void ThreadManager::DestroyThread() {
    // 등록 해제
    t_localArena = nullptr;
    t_linearArena = nullptr;
    if (t_fbb) {
        delete t_fbb;
        t_fbb = nullptr;
    }

    t_threadId = 0;
}

LocalArena* GetLocalArena() {
    return t_localArena;
}

LinearArena* GetLinearArena() {
    return t_linearArena;
}

flatbuffers::FlatBufferBuilder* GetFBB() {
    return t_fbb;
}

ThreadId GetCurrentThreadId() {
    return t_threadId;
}

void ThreadManager::ApplyThreadAffinity() {
#if defined(__APPLE__)
    // 1. QoS 설정
    pthread_set_qos_class_self_np(QOS_CLASS_USER_INTERACTIVE, 0);

    // 2. Affinity 설정
    thread_affinity_policy_data_t policy = { 1 }; // 1번 그룹 설정
    thread_port_t mach_thread = pthread_mach_thread_np(pthread_self());

    kern_return_t kr = thread_policy_set(mach_thread, THREAD_AFFINITY_POLICY,
                                        (thread_policy_t)&policy,
                                        THREAD_AFFINITY_POLICY_COUNT);
    if (kr != KERN_SUCCESS) {
        LOG_INFO("Affinity hint ignored: ", kr);
    }

    

#elif defined(__linux__)
    // Linux: 특정 CPU 코어에 명시적으로 고정
    cpu_set_t cpuset;
    CPU_ZERO(&cpuset);;
    CPU_SET(coreId, &cpuset);

    pthread_t current_thread = pthread_self();
    int rc = pthread_setaffinity_np(current_thread, sizeof(cpu_set_t), &cpuset);
    if (rc != 0) {
        std::cerr << "Error calling pthread_setaffinity_np: " << rc << std::endl;
    }
#endif
}

void ThreadManager::Shutdown() {
    for (int i = 0; i < m_threadCount; ++i) {
        auto* arena = m_arenaMap[i].load();
        auto* linear = m_linearMap[i].load();
        if (arena) {
            arena->~LocalArena();
        }
        if (linear) {
            linear->~LinearArena();
        }
        m_arenaMap[i].store(nullptr, std::memory_order_relaxed);
        m_linearMap[i].store(nullptr, std::memory_order_relaxed);
    }
}

}
