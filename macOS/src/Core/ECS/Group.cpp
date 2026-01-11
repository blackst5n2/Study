#include "Group.h"

namespace Core {

Group::Group(uint64_t m, uint32_t maxEntities) : mask(m), count(0), capacity(maxEntities) {
    entities = (uint32_t*)MemoryManager::Get().Allocate(sizeof(uint32_t) * maxEntities, MemoryTag::Permanent);
    entityToIndex = (int32_t*)MemoryManager::Get().Allocate(sizeof(int32_t) * maxEntities, MemoryTag::Permanent);
    // 초기화: -1은 그룹에 포함되지 않음을 의미
    for (uint32_t i = 0; i < capacity; ++i) {
        entityToIndex[i] = -1;
    }
}

void Group::onEntityUpdated(uint32_t entityIdx, uint64_t newMask) {
    bool match = (newMask & mask) == mask;
    bool exists = entityToIndex[entityIdx] != -1;

    if (match && !exists) {
        // 추가: 배열 끝에 넣고 인덱스 기록
        entityToIndex[entityIdx] = count;
        entities[count++] = entityIdx;
    } else if (!match && exists) {
        // 삭제: Swap and Pop
        uint32_t targetIdx = entityToIndex[entityIdx];
        uint32_t lastEntity = entities[--count];

        entities[targetIdx] = lastEntity;
        entityToIndex[lastEntity] = targetIdx;

        entityToIndex[entityIdx] = -1;
    }
}

}