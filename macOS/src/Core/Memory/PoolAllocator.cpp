#include "PoolAllocator.h"
#include <cstdlib> 
#include <cassert>
#include "../Utils/MemoryUtils.h"
#include <algorithm>

namespace Core {
// 생성자: 메모리 획득 및 Free Lists 초기화
PoolAllocator::PoolAllocator(void* startPtr, size_t totalSize, size_t slotSize, size_t alignment)
    :   m_totalSize(totalSize),
        m_slotSize(slotSize),
        m_alignment(alignment),
        m_pFreeListHead(nullptr)
{
    // 1. OS로부터 큰 메모리 블럭을 할당 받음
    m_startPtr = startPtr;
    assert(m_startPtr != nullptr && "Pool Allocator failed to allocate memory.");
    assert(slotSize >= sizeof(void*)); // 포인터 크기보다 커야함.

    // 2. Free List를 초기화함.
    FreeListNode* current = nullptr;

    // m_startPtr의 주소값
    uintptr_t currentAddress = (uintptr_t)m_startPtr;

    // 정렬된 시작 주소
    uintptr_t alignedAddress = Utils::alignForward(currentAddress, alignment);

    // 시작 부분에 정렬을 위한 패딩이 생겼을 수 있음
    size_t padding = alignedAddress - currentAddress;

    // 실제 슬롯이 시작될 주소
    uintptr_t currentSlotAddress = alignedAddress;

    // 메모리 경계 확인을 위해 최대 할당 가능 주소 계산
    uintptr_t endAddress = currentAddress + totalSize;

    
    // 슬롯의 실제 크기 (최소한 FreeListNode의 크기보다 크거나 같아야 함)
    // 그리고 alignment를 고려하여 슬롯 크기를 조정해야 함.
    size_t actualSlotSize = std::max(slotSize, sizeof(FreeListNode));

    size_t numSlots = totalSize / slotSize;
    // 3. 전체 메모리를 순회하며 모든 슬롯을 Free List에 연결함.
    for (size_t i = 0; i < numSlots; ++i) {
    // 끝에서부터 거꾸로 연결하면 할당 시 앞(0번)부터 나옵니다.
    uintptr_t slotAddr = alignedAddress + (numSlots - 1 - i) * actualSlotSize;
    current = (FreeListNode*)slotAddr;
    current->next = m_pFreeListHead;
    m_pFreeListHead = current;
}

    // 초기화 과정 완료. 이제 m_pFreeListHead는 모든 빈 슬롯을 가지고 있음.
}

// 소멸자: RAII
PoolAllocator::~PoolAllocator() {
    m_startPtr = nullptr;
    m_pFreeListHead = nullptr;
    // 다른 통계 변수들은 초기화됨.
}

void* PoolAllocator::allocate(size_t size, size_t alignment) {
    // 1. 방어 코드: Pool이 관리하는 슬롯 크기보다 큰 요청이 들어오면 거절
    // (단, 고정 크기 할당자의 특성상 size를 무시하고 무조건 슬롯 하나를 주기도 함)
    if (size > m_slotSize) {
        return nullptr;
    }
    
    // 2. 실제 할당 로직 (기존에 만든 로직 호출)
    return this->allocateInternal();
}

void PoolAllocator::deallocate(void* p) {
    this->deallocateInternal(p);
}

// 할당 (Allocation): O(1)
void* PoolAllocator::allocateInternal() {
    // 1. Free List에 사용 가능한 슬롯이 있는지 확인 (논리적 방어)
    if (m_pFreeListHead == nullptr) {
        // Pool에 남은 슬롯이 없음.
        return nullptr;
    }

    // 2. 헤드가 가리키는 슬롯을 떼어냄. (할당)
    void* allocatedSlot = (void*)m_pFreeListHead;

    // 3. 헤드 포인터를 다음 사용 가능한 슬롯으로 이동시킴.
    // 현재 노드(m_pFreeListHead)가 가지고 있던 next 포인터 값을 새로운 헤드로 지정
    m_pFreeListHead = m_pFreeListHead->next;

#ifdef ENABLE_ALLOCATOR_STATS
    // 4. 통계 업데이트
    ++m_numUsedSlots;
#endif
    // O(1) 할당 완료
    return allocatedSlot; // O(1)
}

// 해제 (Deallocation): O(1)
void PoolAllocator::deallocateInternal(void* p) {
    if (p == nullptr) return;

    // 1. 반납된 주소를 FreeListNode*로 해석함.
    // 이 메모리 공간은 이제 FreeListNode의 데이터 구조를 저장하는 데 사용.
    FreeListNode* node = (FreeListNode*)p;

    // 2. 이 노드의 next 포인터가 현재 Free List의 Head를 가리키도록 연결함.
    // (즉, 반납된 노드를 Free List의 맨 앞에 삽입)
    node->next = m_pFreeListHead;

    // 3. Free List의 Head를 이 새로운 노드로 지정함.
    m_pFreeListHead = node; // O(1) 해제 완료

#ifdef ENABLE_ALLOCATOR_STATS
    // 4. 통계 업데이트
    --m_numUsedSlots;
#endif  
}

#ifdef ENABLE_ALLOCATOR_STATS
size_t PoolAllocator::getNumUsedSlots() const {
    return m_numUsedSlots;
}
#endif

}
