#pragma once
#include <cstdint>
#include "Entity.h"
#include "IStorage.h"
#include "ComponentStorage.h"
#include "ComponentTypeManager.h"
#include "View.h"
#include "Group.h"
#include "../Memory/MemoryHelpers.h"

namespace Core {

using EntityMask = uint64_t;

class Registry {

private:
    // 1. 엔티티 ID 관리
    EntityManager m_entityManager;

    // 2. 각 엔티티의 비트 마스크 보관
    EntityMask* m_entityMasks; // 65536 * 8 bytes

    // 3. 컴포넌트 저장소 관리
    IStorage* m_storages[64];

    // 4. 그룹
    Group* m_groups[256];
    uint32_t m_groupCount = 0;
    uint32_t m_maxEntities;

    void notifyGroups(uint32_t entityIdx, uint64_t newMask) {
        for (uint32_t i = 0; i < 256; ++i) {
            m_groups[i]->onEntityUpdated(entityIdx, newMask);
        }
    }

    template<typename T>
    ComponentStorage<T>* getStorage();

public:
    Registry(uint32_t maxEntities);

    // --- 엔티티 ---
    Entity create();

    void destroy(Entity e);

    void destroy(uint32_t eId);

    // --- 컴포넌트 ---
    template<typename T>
    void addComponent(Entity e, T data);

    template<typename T>
    T& getComponent(Entity e);

    template<typename T>
    void removeComponent(Entity e);

    void removeAllComponents(Entity e);

    void removeAllComponents(uint32_t eId);

    // --- 그룹 ---
    template<typename... Components>
    Group* getGroup();

    // --- 쿼리(View) ---
    template<typename... Components>
    View<Components...> view();

    // --- 헬퍼 ---
    EntityMask getMask(uint32_t index) const;

    uint32_t getGeneration(uint32_t index) const;
};

}
