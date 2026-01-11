#include "StackAllocator.h"
#include <cstdlib>
#include "../Utils/MemoryUtils.h"

namespace Core
{
/*
// 생성자: 메모리 획득 (자원 획득)
StackAllocator::StackAllocator(size_t size)
    : m_totalSize(size)
{
    // OS의 힙으로부터 'size'만큼의 메모리를 한 번에 할당 받음.
    // 이 시점부터 Allocator가 이 메모리를 통제함.
    m_startPtr = malloc(size);

    if (m_startPtr == nullptr) {
        assert(m_startPtr != nullptr && "StackAllocator failed to allocate initial memory block!");
    }

    m_currentPtr = m_startPtr; // 시작 주소와 현재 주소를 일치시켜 초기화
}
*/
StackAllocator::StackAllocator(void* startPtr, size_t totalSize) : m_totalSize(totalSize), m_startPtr(startPtr), m_offset(0) {
    assert(m_startPtr != nullptr && "StackAllocator received null memory!");

    m_currentPtr = m_startPtr;
}

// 소멸자: 메모리 반납 (RAII, 자원 해제)
StackAllocator::~StackAllocator() {
    m_startPtr = nullptr;
    m_currentPtr = nullptr;
}

StackAllocator::Marker StackAllocator::getMarker() const {
    // m_currentPtr의 주소값을 uintptr_t로 캐스팅하여 변환
    return (Marker)m_currentPtr; // O(1)
}

void StackAllocator::release(Marker marker) {
    // 1. 포인터 되돌리기 O(1)
    m_currentPtr = (void*)marker;
}

// 메모리 할당 (Allocation)
void* StackAllocator::allocate(size_t size, size_t alignment) {
    // 1. 현재 포인터의 주소값을 정수형으로 변환
    uintptr_t currentAddress = (uintptr_t)m_currentPtr;

    // 2. Alignment 로직 적용: 정렬된 주소 계산
    uintptr_t alignedAddress = Utils::alignForward(currentAddress, alignment);

    // 3. Padding 계산: 정렬 때문에 버려지는 공간 (내부 단편화)
    // (정렬된 주소 - 현재 주소) = 패딩 크기
    size_t padding = alignedAddress - currentAddress;

    // 4. 새로운 포인터 위치 계산
    uintptr_t newCurrentAddress = alignedAddress + size;
    
    // 5. 메모리 초과 검사 (통제 경계 확인)
    // 새로운 포인터 주소가 (시작 주소 + 최대 크기)를 초과하는가?
    uintptr_t endAddress = (uintptr_t)m_startPtr + m_totalSize;

    if (newCurrentAddress > endAddress) {
        return nullptr;
    }

    // 6. 포인터 이동 및 할당
    // 현재 포인터(m_currentPtr)를 정렬된 주소만큼 이동시키고, 요청된 크기만큼 앞으로 전진시킴.
    // 현재 포인터에 Padding을 더할 필요 없이, 정렬된 주소에서 size만큼 이동하면 됨.
    m_currentPtr = (void*)newCurrentAddress;

    // 7. 사용자에게 정렬된 주소 반환 (Padding이 적용된 실제데이터 시작 위치)
    return (void*)alignedAddress;
}

// 해제 (Deallocation): LIFO 규칙 강제
void StackAllocator::deallocate(void* p) {
    // Stack Allocator의 핵심: LIFO가 아닌 포인터 해제는 금지
    // (p가 마지막으로 할당된 블록의 시작 주소여야 함)

    // Stack Allocator는 '해제' 연산을 느슨하게 관리함
    // 보통은 마지막 할당된 블록의 시작 주소로 m_currentPtr을 되돌림.
    (void)p; 
}

}