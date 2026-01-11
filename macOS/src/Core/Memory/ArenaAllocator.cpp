#include "ArenaAllocator.h"
#include "../Utils/MemoryUtils.h"
#include <cassert>
#include <iostream>

namespace Core {

ArenaAllocator::ArenaAllocator(size_t arenaSize) : m_totalSize(arenaSize) {
    // 1. OS 페이지 크기 (4KB) 경계로 정렬하여 거대 블록 할당
    const size_t pageSize = 4096;

    // Mac/POSIX: posix_memalign 사용
    int result = posix_memalign((void**)&m_bufferStart, pageSize, m_totalSize);

    if (result != 0) {
        assert(false && "Failed to allocate Page-Aligned Arena Memory");
    }

    m_offsetPtr = m_bufferStart;
}

ArenaAllocator::ArenaAllocator(void* slabPtr, size_t slabSize) : m_bufferStart((uint8_t*)slabPtr), m_totalSize(slabSize), m_isExternallyAllocated(true) {
    m_offsetPtr = m_bufferStart;
}

ArenaAllocator::~ArenaAllocator() {
    if (!m_isExternallyAllocated) {
        Release();
    }
    m_bufferStart = nullptr;
    m_offsetPtr = nullptr;
}

void* ArenaAllocator::Allocate(size_t size, size_t alignment) {
    // 현재 포인터에서 alignment만큼 앞으로 밀어서 정렬된 주소 계산
    uintptr_t currentAddr = (uintptr_t)m_offsetPtr;
    uintptr_t alignedAddr = Utils::alignForward(currentAddr, alignment);

    size_t neededSize = (alignedAddr - currentAddr) + size;

    // Arena 용량 체크
    if (m_offsetPtr + neededSize > m_bufferStart + m_totalSize) {
        std::cerr << "Arena Allocator out of memory!" << std::endl;
        return nullptr;
    }

    m_offsetPtr = (uint8_t*)(alignedAddr + size);
    return (void*)alignedAddr;
}

void ArenaAllocator::Reset() {
    // 포인터만 초기화하여 재사용
    m_offsetPtr = m_bufferStart;
}

void ArenaAllocator::Release() {
    if (m_bufferStart) {
        free(m_bufferStart); // posix_memalign으로 할당한 것은 free로 해제
    }
}

}