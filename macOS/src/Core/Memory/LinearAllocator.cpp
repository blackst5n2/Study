#include "LinearAllocator.h"
#include <cstdlib>
#include <cassert>
#include "../Utils/MemoryUtils.h"

namespace Core
{
// 생성자
LinearAllocator::LinearAllocator(void* startPtr, size_t size)
    : m_startPtr(startPtr), m_totalSize(size)
{
    assert(m_startPtr != nullptr && "LinearAllocator failed to allocate initial memory block!");

    m_currentPtr = m_startPtr;
}

// 소멸자
LinearAllocator::~LinearAllocator() {
    m_startPtr = nullptr;
    m_currentPtr = nullptr;
    m_totalSize = 0;
}

// 할당
void* LinearAllocator::allocate(size_t size, size_t alignment) {
    if (size == 0) {
        return nullptr;
    }

    uintptr_t currentAddress = (uintptr_t)m_currentPtr;

    // 1. Alignment 로직 적용: 정렬된 주소 계산
    uintptr_t alignedAddress = Utils::alignForward(currentAddress, alignment);

    // 2. 새로운 포인터 위치 계산
    uintptr_t newCurrentAddress = alignedAddress + size;

    // 3. 메모리 초과 검사
    uintptr_t endAddress = (uintptr_t)m_startPtr + m_totalSize;

    if (newCurrentAddress > endAddress) {
        return nullptr;
    }

    // 4. 포인터 이동 및 할당
    m_currentPtr = (void*)newCurrentAddress;

    // 5. 사용자에게 정렬된 주소 변경
    return (void*)alignedAddress;
}

// 전체 해제 (Reset) - O(1)
void LinearAllocator::reset() {
    // 현재 포인터를 시작 주소로 되돌림
    m_currentPtr = m_startPtr;
}

}