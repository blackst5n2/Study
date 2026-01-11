#include "LinearArena.h"
#include <cassert>
#include "../Logger.h"

namespace Core {

LinearArena::LinearArena(ThreadId workerId, void* slabPtr, size_t slabSize)
    : m_workerId(workerId), m_slabSize(slabSize) {
    m_slabPtr = slabPtr;
    assert(m_slabPtr != nullptr && "[MemoryManager] failed to provide a linear slab");

    size_t linearObjSize = sizeof(LinearAllocator);
    void* linearAddr = slabPtr;

    void* actualPtr = static_cast<uint8_t*>(slabPtr) + linearObjSize;
    size_t actualSize = slabSize - linearObjSize;

    m_linearAllocator = new (linearAddr) LinearAllocator(actualPtr, actualSize);

    LOG_INFO("LinearArena [ID:", (int)m_workerId, "] Internal Allocator Initialized.");
}

LinearArena::~LinearArena() {
    if (m_linearAllocator) {
        m_linearAllocator->~LinearAllocator();
        m_linearAllocator = nullptr;
    }
    
}

void* LinearArena::Allocate(size_t size, size_t alignment) {
    return m_linearAllocator->allocate(size, alignment);
}

void LinearArena::Reset() {
    m_linearAllocator->reset();
}

}