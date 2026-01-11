#pragma once

#include <cstddef>

namespace Core {
class Allocator {
public:
    virtual ~Allocator() = default;

    // 모든 할당자는 이 두 기능을 구현해야 함
    virtual void* allocate(size_t size, size_t alignment = 4) = 0;
    virtual void deallocate(void* p) = 0;

#ifdef ENABLE_ALLOCATOR_STATS
    // 통계용 기능
    virtual size_t getCurrentUsedSize() const = 0;
#endif
};

}