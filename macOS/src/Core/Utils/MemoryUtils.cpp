#include "MemoryUtils.h"
#include <cassert>

namespace Core
{
    namespace Utils
    {
        uintptr_t alignForward(uintptr_t currentAddress, size_t alignment)
        {
            // Alignment는 2의 거듭제곱이어야 함.
            assert((alignment & (alignment - 1)) == 0);

            uintptr_t mask = (uintptr_t)(alignment - 1);

            // 핵심 비트 연산 로직
            return (currentAddress + mask) & (~mask);
        }
    }
}

