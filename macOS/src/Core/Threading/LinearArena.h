#pragma once

#include "Common.h"
#include "../Memory/MemoryManager.h"
#include "../Memory/LinearAllocator.h"
#include <cstddef>


namespace Core {
const int LINEAR_SLAB_SIZE = 1024 * 1024;

class LinearArena {
public:
    LinearArena(ThreadId workerId, void* slabPtr, size_t slabSize = LINEAR_SLAB_SIZE);
    ~LinearArena();

    void* Allocate(size_t size, size_t alignment = 32);
    void Reset();

    ThreadId GetId() const { return m_workerId; }
    
private:
    ThreadId m_workerId;
    LinearAllocator* m_linearAllocator;
    void* m_slabPtr;
    size_t m_slabSize;
    
};

}