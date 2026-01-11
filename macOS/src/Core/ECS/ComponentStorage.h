#pragma once
#include <cstdint>
#include <cstddef>
#include "IStorage.h"
#include "../Memory/MemoryHelpers.h"

namespace Core {

template<typename T>
class ComponentStorage : public IStorage {
private:
    T* m_denseData; // 실제 POD 데이터
    uint32_t* m_denseToEntity; // 밀집 데이터가 어떤 엔티티의 것인지 역참조
    int32_t* m_sparse; // 엔티티 인덱스로 밀집 데이터 위치 찾기

    uint32_t m_capacity;
    uint32_t m_count;

    static constexpr int32_t EMPTY = -1;

public:
    ComponentStorage(uint32_t maxEntities);

    void assign(Entity entity, T component);

    T& get(Entity entity);

    // 핵심: 삭제 시 O(1) 최적화 (Swap and Pop)
    void removeRaw(uint32_t entityIdx) override;

    void remove(Entity entity);
    void remove(uint32_t entityIdx);

    uint32_t size() override;
};

}