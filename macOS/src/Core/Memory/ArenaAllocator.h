#pragma once

#include <cstddef>
#include <cstdint>
#include <cstdlib>
#include <vector>

namespace Core {

// 캐시 라인 간섭 방지 (64바이트 정렬)
class alignas(64) ArenaAllocator {
public:
    ArenaAllocator(size_t arenaSize);
    ArenaAllocator(void* slabPtr, size_t slabSize); // 외부에서 할당된 Slab을 주입받는 생성자
    ~ArenaAllocator();

    // 할당: 단순히 포인터를 밀어서 할당 (O(1))
    void* Allocate(size_t size, size_t aligment = 16);

    // 전체 해제: 실제 메모리를 OS에 반환하지 않고 포인터만 초기화
    void Reset();

    // 아예 OS에 메모리 반환
    void Release();

private:
    uint8_t* m_bufferStart = nullptr;
    uint8_t* m_offsetPtr = nullptr;
    size_t m_totalSize = 0;

    bool m_isExternallyAllocated = false; // 소멸자에서 free 방지
};

}