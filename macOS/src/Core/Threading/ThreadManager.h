#pragma once
#include <atomic>
#include <vector>
#include "Common.h"
#include "../Network/Gen/Protocol_generated.h"

namespace Core {

class LocalArena;
class LinearArena;

LocalArena* GetLocalArena();
LinearArena* GetLinearArena();
flatbuffers::FlatBufferBuilder* GetFBB();
ThreadId GetCurrentThreadId();

class ThreadManager {
public:
    static ThreadManager& GetInstance() {
        static ThreadManager instance;
        return instance;
    }

    // 스레드가 시작될 때 호출하여 ID와 로컬 아레나를 할당받음
    void InitThread();
    void Shutdown();

    // 스레드가 종료될 때 자원 정리
    void DestroyThread();

    // ID로 아레나 찾기 (Lock-Free 조회)
    LocalArena* GetLocalArena(ThreadId id) const {
        if (id >= m_threadCount) return nullptr;
        return m_arenaMap[id].load(std::memory_order_relaxed);
    }

    LinearArena* GetLinearArena(ThreadId id) const {
        if (id >= m_threadCount) return nullptr;
        return m_linearMap[id].load(std::memory_order_relaxed);
    }

    void RegisterLocalArena(ThreadId id, LocalArena* arena) {
        if (id < m_threadCount) {
            m_arenaMap[id].store(arena, std::memory_order_release);
        }
    }

    void ResgisterLinearArena(ThreadId id, LinearArena* arena) {
        if (id <m_threadCount) {
            m_linearMap[id].store(arena, std::memory_order_release);
        }
    }

    // 스레드 특정 코어에 그룹에 고정
    void ApplyThreadAffinity();
    
private:
    ThreadManager() : m_threadCount(0) {}
    
    std::atomic<ThreadId> m_threadCount;

    // 스레드 ID를 인덱스로 사용하여 즉시 접근
    std::atomic<LocalArena*> m_arenaMap[11];
    std::atomic<LinearArena*> m_linearMap[11];
};

// TLS 선언
extern thread_local ThreadId t_threadId;
extern thread_local LocalArena* t_localArena;
extern thread_local LinearArena* t_linearArena;

}