#pragma once

#include "Allocator.h"
#include <cstdint>
#include <cstddef>

namespace Core
{
class PoolAllocator : public Allocator {
public:
    //PoolAllocator(size_t totalSize, size_t slotSize, size_t alignment = 4);
    PoolAllocator(void* startPtr, size_t totalSize, size_t slotSize, size_t alignment = 4);
    
    ~PoolAllocator();

    void* allocate(size_t size, size_t alignment = 4) override;

    void deallocate(void* p) override;

private:
    void* m_startPtr; // 메모리 풀 시작 주소
    size_t m_slotSize; // 고정된 슬롯 크기 (예: 16바이트, 24바이트)

    // A. Free Lists의 노드 역할을 하는 구조체
    // 중요: 이 구조체 자체는 할당된 메모리 슬롯의 '내부'에 위치함.
    struct FreeListNode {
        FreeListNode* next; // 다음 사용 가능한 슬롯을 가리키는 포인터
    };

    size_t m_totalSize;
    size_t m_alignment;

    FreeListNode* m_pFreeListHead; // Free List의 가장 첫 번째 블록 포인터

    // 기존의 인자 없는 할당 로직을 내부 함수로 분리
    void* allocateInternal();
    void deallocateInternal(void* p);

#ifdef ENABLE_ALLOCATOR_STATS // 통계가 필요한 경우에만 컴파일
public:
    // 테스트를 위한 헬퍼 함수
    size_t getNumUsedSlots() const;
    size_t getCurrentUsedSize() const override { return m_slotSize; }
private:
    size_t m_numUsedSlots = 0; // 사용중인 슬롯의 개수
#endif
};

}