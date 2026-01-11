#pragma once
#include <cstdint>
#include "../Memory/MemoryHelpers.h"
#include "../Logger.h"

namespace Core {

struct Entity {
    uint32_t id;

    // 비트 마스크 설정 (Index 24비트, Generation 8비트)
    static constexpr uint32_t INDEX_MASK = 0x00FFFFFF;
    static constexpr uint32_t GEN_MASK = 0xFF000000;
    static constexpr uint32_t GEN_SHIFT = 24;
    
    uint32_t index() const { return id & INDEX_MASK; }
    uint32_t generation() const { return (id & GEN_MASK) >> GEN_SHIFT; }
};

class EntityManager {
private: 
    uint32_t maxEntities;
    uint32_t* freeList; // 재사용 가능한 인덱스들을 모아둔 스택
    uint32_t freeListTop; // Free List의 현재 위치
    uint32_t nextIndex; // 한 번도 사용되지 않은 인덱스를 가리키는 포인터
    uint32_t* generations; // 각 인덱스의 현재 세대 저장

public:
    EntityManager(uint32_t capacity);

    Entity create();

    void destroy(Entity entity);
    void destroy(uint32_t idx);

    bool isValid(Entity entity) const;
    bool isValid(uint32_t idx) const;

    uint32_t getGeneration(uint32_t id) const;
};

}

