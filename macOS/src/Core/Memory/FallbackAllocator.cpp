#include "FallbackAllocator.h"

namespace Core {
void* FallbackAllocator::allocate(size_t size, size_t alignment) {
    // 1. 필요한 총 크기 계산 (헤더 포함)
    size_t totalSize = size + sizeof(FallbackAllocationHeader);

    // 2. 먼저 Primary에게 시도
    void* ptr = m_primaryAllocator.allocate(totalSize, alignment);
    AllocatorType type = AllocatorType::PRIMARY;

    // 3. 실패하면 Fallback에게 시도
    if (ptr == nullptr) {
        ptr = m_fallbackAllocator.allocate(totalSize, alignment);
        type = AllocatorType::FALLBACK;
    }

    if (ptr == nullptr) return nullptr;

    // 4. 주소값에 헤더 정보 기록 (포인터 산술 및 역참조)
    FallbackAllocationHeader* header = (FallbackAllocationHeader*)ptr;

    header->allocator_type = type;
    header->size = size;

    // 5. 실제 데이터 주소(사용자가 쓸 공간) 반환
    return (void*)((uintptr_t)ptr + sizeof(FallbackAllocationHeader));
}

void FallbackAllocator::deallocate(void* p) {
    if (p == nullptr) {
        return;
    }

    // 1. 사용자 주소 p에서 헤더 위치 역추적
    FallbackAllocationHeader* header = (FallbackAllocationHeader*)((uintptr_t)p - sizeof(FallbackAllocationHeader));

    // 2. 헤더에 적힌 '타입'을 보고 원래 주인에게 돌려줌
    if (header->allocator_type == AllocatorType::PRIMARY) {
        m_primaryAllocator.deallocate(header); // 실제 블록의 시작점(header)을 전달
    } else {
        m_fallbackAllocator.deallocate(header);
    }
}

}