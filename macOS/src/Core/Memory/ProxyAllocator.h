#pragma once

#include "Allocator.h"
#include <cstdint>
#include <cstddef>
#include <cassert>

namespace Core{

// Proxy Allocator의 메타데이터 구조체
// 할당된 블록의 사용자 데이터 바로 앞에 삽입
struct AllocationHeader {
    size_t size;
    void* original_ptr;
};

class ProxyAllocator : public Allocator{
public:
    // Guard Bytes 정의 (메모리 오염 감지용 숫자)
    static constexpr uint32_t FRONT_GUARD = 0xDEADBEEF;
    static constexpr uint32_t BACK_GUARD = 0xBEEFDEAD;

    ProxyAllocator(Allocator& wrappedAllocator);
    ~ProxyAllocator();

    void* allocate(size_t size, size_t alignment = 4) override;

    void deallocate(void* p) override;

    void reset();

private:
    Allocator& m_wrappedAllocator; 

#ifdef ENABLE_ALLOCATOR_STATS
public:
    size_t getCurrentUsedSize() const override;
#endif
};

}