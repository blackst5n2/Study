#pragma once

#include <cstdint>
#include <cstddef>

namespace Core
{
    namespace Utils
    {
        /**
         * 현재 주소를 주어진 경계(alignment)에 맞게 정렬함.
         * @param currentAddress - 현재 포인터의 주소값 (uintptr_t)
         * @param alignment - 정렬할 바이트 경계 (2의 거듭제곱이어야 함)
         * @return 정렬된 다음 주소 (uintptr_t)
         */
        uintptr_t alignForward(uintptr_t currentAddress, size_t alignment);
    }
}
