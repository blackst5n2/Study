#include "ComponentStorage.h"
#include "../Memory/Memory.h"

namespace Core {

template<typename T>
ComponentStorage<T>::ComponentStorage(uint32_t maxEntities) {
    m_capacity = maxEntities;
    m_count = 0;

    m_denseData = (T*)MemoryManager::Get().Allocate(sizeof(T) * m_capacity, MemoryTag::Permanent);
    m_denseToEntity = (uint32_t*)MemoryManager::Get().Allocate(sizeof(uint32_t) * m_capacity, MemoryTag::Permanent);
    m_sparse = (int32_t*)MemoryManager::Get().Allocate(sizeof(int32_t) * m_capacity, MemoryTag::Permanent);

    // sparse 배열 초기화
    for(uint32_t i = 0; i < m_capacity; ++i) m_sparse[i] = -1;
}

template<typename T>
void ComponentStorage<T>::assign(Entity entity, T component) {
    uint32_t idx = entity.index();

    // 2. Dense 배열 끝에 데이터 추가
    m_sparse[idx] = static_cast<int32_t>(m_capacity);
    m_denseData[m_count] = component;
    m_denseToEntity[m_count] = idx; // 삭제 시 필요

    ++m_count;
}

template<typename T>
T& ComponentStorage<T>::get(Entity entity) {
    return m_denseData[m_sparse[entity.index()]];
}

template<typename T>
void ComponentStorage<T>::removeRaw(uint32_t entityIdx) {
    remove(entityIdx);
}

template<typename T>
void ComponentStorage<T>::remove(Entity entity) {
    uint32_t idx = entity.index();
    int32_t denseIdx = m_sparse[idx];

    // 마지막 데이터와 위치를 바꿈 (순서는 깨지지만 메모리는 밀집됨)
    T lastData = m_denseData[m_count];
    uint32_t lastEntityIdx = m_denseToEntity[m_count];

    m_denseData[denseIdx] = lastData;
    m_denseToEntity[denseIdx] = lastEntityIdx;
    m_sparse[lastEntityIdx] = denseIdx;

    // 마지막 요소 제거
    m_denseData[m_count] = nullptr;
    m_denseToEntity[m_count] = nullptr;
    m_sparse[idx] = EMPTY;

    --m_count;
}

template<typename T>
void ComponentStorage<T>::remove(uint32_t entityIdx) {
    if (m_sparse[entityIdx] == -1) return;

    int32_t denseIdx = m_sparse[entityIdx];

    // 마지막 데이터와 위치를 바꿈 (순서는 깨지지만 메모리는 밀집됨)
    T lastData = m_denseData[m_count];
    uint32_t lastEntityIdx = m_denseToEntity[m_count];

    m_denseData[denseIdx] = lastData;
    m_denseToEntity[denseIdx] = lastEntityIdx;
    m_sparse[lastEntityIdx] = denseIdx;

    // 마지막 요소 제거
    m_denseData[m_count] = nullptr;
    m_denseToEntity[m_count] = nullptr;
    m_sparse[entityIdx] = EMPTY;

    --m_count;
}

template<typename T>
uint32_t ComponentStorage<T>::size() {
    return m_count;
}

}