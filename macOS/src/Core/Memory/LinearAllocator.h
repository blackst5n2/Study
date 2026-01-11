#pragma once

#include "Allocator.h"
#include <cstddef>
#include <cstdint>

namespace Core {
class LinearAllocator : public Allocator {
public:
    // 생성자: 전체 메모리 크기를 받아옴
    // LinearAllocator(size_t totalSize);
    LinearAllocator(void* startPtr, size_t totalSize);
    ~LinearAllocator();

    // 할당: 요청 크기의 정렬 요구사항에 따라 메모리를 할당
    void* allocate(size_t size, size_t alignment = 4) override;
    void deallocate(void* p) override { /* 개별 해제 미지원 */}

    // 전체 해제 (Reset): 현재 포인터를 시작점으로 되돌림
    void reset(); // O(1)

    inline size_t getTotalSize() const { return m_totalSize; }

private:
    void* m_startPtr = nullptr; // 할당된 메모리 블록의 시작 주소
    void* m_currentPtr = nullptr; // 다음 할당이 시작될 주소
    size_t m_totalSize = 0; // 총 메모리 크기

#ifdef ENABLE_ALLOCATOR_STATS
public:
    // 통계
    inline size_t getCurrentUsedSize() const override { return (uintptr_t)m_currentPtr - (uintptr_t)m_startPtr; }
#endif
};

}