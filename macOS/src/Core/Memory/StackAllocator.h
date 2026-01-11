#pragma once

#include "Allocator.h"
#include <cstdint>
#include <cstddef>
#include <cassert>

namespace Core
{
class StackAllocator : public Allocator {
public:
    // 마커는 현재 스택 포인터의 주소값을 저장하는 타입.
    using Marker = uintptr_t;

    StackAllocator(size_t size);
    StackAllocator(void* startPtr, size_t totalSize);

    virtual ~StackAllocator();

    // 1. 현재 포인터 위치를 마커로 변환
    Marker getMarker() const;

    // 2. 마커 위치로 포인터를 되돌림
    void release(Marker marker);

    void* allocate(size_t size, size_t alignment = 4) override;
    
    void deallocate(void* p) override;

#ifdef ENABLE_ALLOCATOR_STATS
    size_t getCurrentUsedSize() const override { return (uintptr_t)m_currentPtr - (uintptr_t)m_startPtr; }
#endif
    size_t getCapacity() const {return m_totalSize; }

    void* getStartPtr() const { return m_startPtr; }
    
private:
    void* m_startPtr;
    void* m_currentPtr;
    size_t m_totalSize;
    size_t m_offset;
};

}