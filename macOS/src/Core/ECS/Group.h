#pragma once
#include <cstdint>
#include "Entity.h"

namespace Core {

class Group {
public:
    uint64_t mask;
    uint32_t* entities; // Dense Array: 조건 충족 엔티티 리스트
    int32_t* entityToIndex; // Sparse Array: Entity ID -> entities 배열의 인덱스
    uint32_t count;
    uint32_t capacity;

    Group(uint64_t m, uint32_t maxEntities);

    void onEntityUpdated(uint32_t entityIdx, uint64_t newMask);
};

}