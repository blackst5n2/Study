#include "LocalArena.h"
#include <cassert>
#include "../Logger.h"

namespace Core {

LocalArena::LocalArena(ThreadId id, void* slabPtr, size_t slabSize) : m_ownerId(id), m_slabSize(slabSize) {
    // 1. 도매상으로부터 slab 확보
    m_slabPtr = slabPtr;
    assert(m_slabPtr != nullptr && "[MemoryManager] failed to provide a local slab");

    size_t allocatorObjSize = sizeof(FreeListAllocator);
    void* allocatorAddr = slabPtr;

    void* actualDataPtr = static_cast<uint8_t*>(slabPtr) + allocatorObjSize;
    size_t actualDataSize = slabSize - allocatorObjSize;

    // 2. 확보한 Slab 주입하여 내부 할당자 생성
    m_internalAllocator = new (allocatorAddr) FreeListAllocator(actualDataPtr, actualDataSize, 8);

    LOG_INFO("LocalArena [ID:", (int)m_ownerId, "] Internal Allocator Initialized.");
}

LocalArena::~LocalArena() {
    FlushRemote();

    if (m_internalAllocator) {
        m_internalAllocator->~FreeListAllocator();
        m_internalAllocator = nullptr;
    }
}

void* LocalArena::Allocate(size_t size, MemoryTag tag, size_t alignment) {
    FlushRemote(); // 청소

    // 헤더 공간을 포함하여 할당
    size_t totalSize = size + sizeof(ThreadBlockHeader);
    void* rawPtr = m_internalAllocator->allocate(totalSize, alignment);

    if (!rawPtr) return nullptr;

    // 주인 정보 각인
    ThreadBlockHeader* header = static_cast<ThreadBlockHeader*>(rawPtr);
    header->tag = tag;
    header->ownerId = m_ownerId;
    header->magic = 0xDEADBEEF;
    header->next = nullptr;

    // 실제 유저 데이터 영역 반환
    return static_cast<uint8_t*>(rawPtr) + sizeof(ThreadBlockHeader);
}

}