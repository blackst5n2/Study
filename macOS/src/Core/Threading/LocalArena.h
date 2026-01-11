#pragma once
#include <cstddef>
#include "Common.h"
#include "../Memory/MemoryManager.h"
#include "../Memory/FreeListAllocator.h"
#include "RemoteFreeQueue.h"

namespace Core {
const int LOCAL_SLAB_SIZE = 16 * 1024 * 1024;

class LocalArena {
public:
    LocalArena(ThreadId id, void* slabPtr, size_t slabSize = LOCAL_SLAB_SIZE);
    ~LocalArena();

    // 1. 소매상에게 할당 요청
    void* Allocate(size_t size, MemoryTag tag = MemoryTag::Default, size_t alignment = 8);

    ThreadId GetId() const { return m_ownerId; }

    // 남들이 내 메모리르 반납할 때 호출
    void PushRemote(ThreadBlockHeader* header) {
        m_remoteQueue.Push(header);
    }

    void deallocate(void* p) {
        if (!p) return;

        m_internalAllocator->deallocate(p);
    }

    // 내가 필요할 때(예: 할당 직전, 혹은 주기적으로) 호출
    void FlushRemote() {
        ThreadBlockHeader* current = m_remoteQueue.PopAll();

        while (current) {
            ThreadBlockHeader* next = current->next;
            m_internalAllocator->deallocate(current);
            current = next;
        }
    }

private:
    ThreadId m_ownerId;
    size_t m_slabSize;
    void* m_slabPtr;
    FreeListAllocator* m_internalAllocator;
    RemoteFreeQueue m_remoteQueue;
};

}
