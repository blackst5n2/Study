#pragma once

#include "Allocator.h"
#include <cstdint>


namespace Core {
// Allocator 출처 정의
enum class AllocatorType : uint8_t {
    PRIMARY = 0,
    FALLBACK = 1
    // 나중에 다른 Fallback Allocator를 추가할 때 확장 가능
};

// Fallback Allocator의 메타데이터
struct FallbackAllocationHeader {
    // 이 블록을 할당한 Allocator의 Type을 기록 (1바이트 사용)
    AllocatorType allocator_type;
    // 사용자가 요청한 순수 크기 (deallocate 시 필요)
    size_t size;
    // 현재 size_t(8) + uint8_t(1) = 9바이트. Alignment를 위해 7바이트 더미 필드를 추가하는 것도 좋음.
};

class FallbackAllocator : public Allocator {
public:
    // 생성자: Primary와 Fallback Allocator를 받음
    FallbackAllocator(Allocator& primary, Allocator& fallback)
        : m_primaryAllocator(primary), m_fallbackAllocator(fallback) {}

    void* allocate(size_t size, size_t alignment = 4) override;
    void deallocate(void* p) override;

#ifdef ENABLE_ALLOCATOR_STATS
    size_t getCurrentUsedSize() const override {
        return m_primaryAllocator.getCurrentUsedSize() + m_fallbackAllocator.getCurrentUsedSize();
    }
#endif
private:
    Allocator& m_primaryAllocator;
    Allocator& m_fallbackAllocator;
};

}

