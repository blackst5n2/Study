#include "Registry.h"

namespace Core {

Registry::Registry(uint32_t maxEntities) : m_entityManager(maxEntities) {
    m_entityMasks = (uint64_t*)MemoryManager::Get().Allocate(sizeof(uint64_t) * maxEntities, MemoryTag::Permanent);
    // 모든 마스크 0으로 초기화
    memset(m_entityMasks, 0, sizeof(uint64_t) * maxEntities);

    // 초기화 시 모드 null로 세팅
    for (int i = 0; i < 64; ++i) {
        m_storages[i] = nullptr;
    }
}

Entity Registry::create() {
    return m_entityManager.create();
}

void Registry::destroy(Entity e) {
    if (!m_entityManager.isValid(e)) return;

    removeAllComponents(e);

    m_entityManager.destroy(e);
} 

void Registry::destroy(uint32_t eId) {
    if (!m_entityManager.isValid(eId)) return;

    removeAllComponents(eId);

    m_entityManager.destroy(eId);
}

template<typename T>
void Registry::addComponent(Entity e, T data) {
    uint32_t typeID = ComponentTypeManager::getID<T>();

    // 1. 해당 타입의 저장소가 없으면 생성
    if (!m_storages[typeID]) {
        void* mem = MemoryManager::Get().Allocate(sizeof(ComponentStorage<T>), MemoryTag::Permanent); 
        m_storages[typeID] = new (mem) ComponentStorage<T>(10000);
    }

    // 2. 저장소에 데이터 추가
    auto* storage = static_cast<ComponentStorage<T>*>(m_storages[typeID]);
    m_entityMasks[e.index()] |= (1ULL << typeID);
    
    // 변화 알림
    notifyGroups(e.index(), m_entityMasks[e.index()]);
}

template<typename T>
T& Registry::getComponent(Entity e) {
    auto* storage = getStorage<T>();
    return storage->get(e);
}

template<typename T>
void Registry::removeComponent(Entity e) {
    uint32_t typeID = ComponentTypeManager::getID<T>();
    uint32_t entityIdx = e.index();

    // 해당 엔티티가 컴포넌트를 가지고 있는지 확인
    if (!(m_entityMasks[entityIdx] & (1ULL << typeID))) return;

    m_storages[typeID]->removeRaw(entityIdx);

    // 비트 마스크 갱신
    m_entityMasks[entityIdx] &= ~(1ULL << typeID);

    notifyGroups(entityIdx, m_entityMasks[entityIdx]);
}

void Registry::removeAllComponents(Entity e) {
    // 해당 엔티티가 가진 모든 컴포넌트 삭제
    uint64_t mask = m_entityMasks[e.index()];
    for (uint32_t i = 0; i < 64; ++i) {
        if (mask & (1ULL << i)) {
            m_storages[i]->removeRaw(e.index());
        }
    }

    m_entityMasks[e.index()] = 0;
    m_entityManager.destroy(e);
    notifyGroups(e.index(), 0);
}

void Registry::removeAllComponents(uint32_t eId) {
    // 해당 엔티티가 가진 모든 컴포넌트 삭제
    uint64_t mask = m_entityMasks[eId];
    for (uint32_t i = 0; i < 64; ++i) {
        if (mask & (1ULL << i)) {
            m_storages[i]->removeRaw(eId);
        }
    }

    m_entityMasks[eId] = 0;
    m_entityManager.destroy(eId);
    notifyGroups(eId, 0);
}


template<typename... Components>
Group* Registry::getGroup() {
    uint64_t mask = 0;
    ((mask |= (1ULL << ComponentTypeManager::getID<Components>())), ...);

    // 이미 있는 그룹인지 확인, 없으면 새로 생성
    for(int i = 0; i < m_groupCount; ++i) {
        if(m_groups[i]->mask == mask) return m_groups[i];
    }

    auto newGroup = Core::New<Group>(MemoryTag::Permanent, sizeof(Group));
    // 신규 그룹 생성 시 기존 엔티티들 전체 스캔해서 초기화 (최초 1회)
    for(uint32_t i = 0; i < 256; ++i) {
        newGroup->onEntityUpdated(i, m_entityMasks[i]);
    }

    return m_groups[m_groupCount++];
}

template<typename... Components>
View<Components...> Registry::view() {
    Group* g = getGroup<Components...>();

    return View<Components...>(g, getStorage<Components>()...);
}

EntityMask Registry::getMask(uint32_t index) const {
    return m_entityMasks[index];
}

uint32_t Registry::getGeneration(uint32_t index) const {
    return m_entityManager.getGeneration(index);
}

template<typename T>
ComponentStorage<T>* Registry::getStorage() {
    uint32_t id = ComponentTypeManager::getID<T>();
    return static_cast<ComponentStorage<T>*>(m_storages[id]);
}

}
