#include "ProxyAllocator.h"
#include "LinearAllocator.h"

namespace Core {

ProxyAllocator::ProxyAllocator(Allocator& wrappedAllocator)
    : m_wrappedAllocator(wrappedAllocator)
{
    // 생성자는 할 일 없음
}

ProxyAllocator::~ProxyAllocator() {
    // 소멸자에서는 Wrapped Allocator를 해제하지 않음 (소유권 없음)
}

void* ProxyAllocator::allocate(size_t size, size_t alignment) {
    if (size == 0) {
        return nullptr;
    }

    // 1. 오버헤드 계산 (Header + Guard Bytes)
    // [Front Guard] [Allocation Header] | [User Data (p)] | [Back Guard]
    const size_t headerSize = sizeof(AllocationHeader);
    const size_t guardSize = sizeof(uint32_t); // 4 bytes

    // 총 필요한 크기: Header + Guard Bytes 2개 + 사용자 요청 크기
    size_t totalRequiredSize = headerSize + (2 * guardSize) + size;

    // 2. Wrapped Allocator에게 메모리 요청
    // 정렬은 Header+Guard 영역이 아닌, 최종적으로 반환할 사용자 포인터 p가 조정
    void* raw_ptr = m_wrappedAllocator.allocate(totalRequiredSize, alignment);

    if (raw_ptr == nullptr) {
        return nullptr;
    }

    uintptr_t currentAddress = (uintptr_t)raw_ptr;

    // 3. 메타데이터 (Header) 배치
    // Header 위치 = raw_ptr의 시작
    AllocationHeader* header = (AllocationHeader*)currentAddress;
    header->size = size;

    // 4. Front Guard 배치
    // Front Gaurd 위치 = Header 끝
    uint32_t* frontGuard = (uint32_t*)(currentAddress + headerSize);
    *frontGuard = FRONT_GUARD;

    // 5. 사용자 데이터 영역 포인터 계산 (p)
    // p 위치 = FrontGuard 끝
    void* userPtr = (void*)(currentAddress + headerSize + guardSize);

    // 6. Back Guard 배치 (가장 중요한 부분)
    // Back Guard 위치 = User Data 끝
    uint32_t* backGuard = (uint32_t*)((uintptr_t)userPtr + size);
    *backGuard = BACK_GUARD;

    return userPtr; // 사용자에게 데이터 영역 시작 주소 반환
}

void ProxyAllocator::deallocate(void* p) {
    // LinearAllcator는 deallocate를 지원하지 않으므로, 이 함수는 Guard 검증만 하고 종료
    if (p == nullptr) {
        return;
    }

    const size_t headerSize = sizeof(AllocationHeader);
    const size_t guardSize = sizeof(uint32_t);

    // 1. 사용자 포인터 p로부터 Front Guard와 Header 역추적
    // Front Guard 위치
    uint32_t* frontGuard = (uint32_t*)((uintptr_t)p - guardSize);
    // 헤더 위치
    AllocationHeader* header = (AllocationHeader*)((uintptr_t)frontGuard - headerSize);

    // 2. Back Guard 위치 계산
    uint32_t* backGuard = (uint32_t*)((uintptr_t)p + header->size);

    // 3. Guard Bytes 검증 (Buffer Overflow / Underflow 감지)

    // Front Guard 검증 (Underflow 감지)
    assert(*frontGuard == FRONT_GUARD && "Proxy Allocator Error: Buffer Underflow Detected!");
    
    // Back Guard 검증 (Overflow 감지)
    assert(*backGuard == BACK_GUARD && "Proxy Allocator Error: Buffer Overflow Detected!");
}

void ProxyAllocator::reset() {

}

#ifdef ENABLE_ALLOCATOR_STATS
size_t ProxyAllocator::getCurrentUsedSize() const {
    // Proxy는 오버헤드가 추가된 상태로 사용하므로, 실제 사용량은 Wrapped Allocator의 사용량을 그대로 반환
    return m_wrappedAllocator.getCurrentUsedSize();
}
#endif

}