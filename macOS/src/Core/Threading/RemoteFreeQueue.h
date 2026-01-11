#pragma once

#include "Common.h"
#include <atomic>

namespace Core {

class RemoteFreeQueue {
public:
    RemoteFreeQueue() {
        m_head.store(nullptr, std::memory_order_relaxed);
    }

    // [Producer] 여러 스레드가 동시에 호츌 (Lock-Free)
    void Push(ThreadBlockHeader* node) {
        // 1. 현재 헤드 읽기
        ThreadBlockHeader* oldHead = m_head.load(std::memory_order_relaxed);

        // 2. CAS Loop (Compare-And-Swap)
        // "내가 넣으려는 순간 헤드가 바뀌자 않았따면 교체, 바뀌었다면 다시 시도"
        do {
            node->next = oldHead;
        } while (!m_head.compare_exchange_weak(oldHead, node, std::memory_order_release, std::memory_order_relaxed));
        
    }

    // [Consumer] 주인 스레드만 호출 (Wait-Free)
    // 현재 쌓인 리스트 전체를 원자적으로 떼어옴 -> O(1)
    ThreadBlockHeader* PopAll() {
        return m_head.exchange(nullptr, std::memory_order_acquire);
    }

private:
    std::atomic<ThreadBlockHeader*> m_head;
};

}