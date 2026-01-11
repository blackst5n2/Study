#include "Entity.h"

namespace Core {

EntityManager::EntityManager(uint32_t capacity)
    : maxEntities(capacity), freeListTop(0), nextIndex(0) {
    
    freeList = (uint32_t*)MemoryManager::Get().Allocate(sizeof(uint32_t) * maxEntities, MemoryTag::Permanent);
    generations = (uint32_t*)MemoryManager::Get().Allocate(sizeof(uint32_t) * maxEntities, MemoryTag::Permanent);

    // 2. 세대 값 초기화
    for (uint32_t i = 0; i < maxEntities; ++i) {
        generations[i] = 0;
    }
}

Entity EntityManager::create() {
    uint32_t id;
    
    // 재사용 가능 인덱스가 있다면 먼저 사용
    if (freeListTop > 0) {
        id = freeList[--freeListTop];
    } else {
        // 재사용 리스트가 비었다면 새로운 인덱스 할당
        if (nextIndex < maxEntities) {
            id = nextIndex++;
        } else {
            // 할당 가능 범위 초과
            LOG_ERR("엔티티 할당 범위 초과 ", freeListTop, "개의 엔티티 존재");
        }
    }

    // 현재 세대 정보를 비트에 합쳐서 반환
    return { uint32_t(generations[id]) << Entity::GEN_SHIFT | id };
}

void EntityManager::destroy(Entity entity) {
    // 유효성 확인 후 세대 값을 올리고 Free List에 반납
    if (isValid(entity)) {
        generations[entity.index()]++; // 세대를 올려서 이전 Entity 복사본을 무효화
        freeList[freeListTop++] = entity.index();
    }
}

void EntityManager::destroy(uint32_t idx) {
    if (isValid(idx)) {
        generations[idx]++;
        freeList[freeListTop++] = idx;
    }
}

bool EntityManager::isValid(Entity entity) const {
    // ID의 세대와 현재 관리 중인 세대가 일치해야만 유효함
    return entity.index() < nextIndex && generations[entity.id] == entity.generation();
}

bool EntityManager::isValid(uint32_t idx) const {
    return idx < nextIndex && generations[idx] == getGeneration(idx);
}

uint32_t EntityManager::getGeneration(uint32_t id) const {
    return generations[id];
}

}