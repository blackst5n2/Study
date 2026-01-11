#include "SystemManager.h"

namespace Core {

template<typename T, typename... Args>
void SystemManager::addSystem(int priority, Args&&... args) {
    if (m_systemCount >= 64) return;

    T* system = Core::New<T>(MemoryTag::Permanent);

    // 우선순위에 맞게 삽입 (삽입 정렬)
    uint32_t insertIdx = m_systemCount;
    for (uint32_t i = 0; i < m_systemCount; ++i) {
        if (priority < m_systems[i].priority) {
            insertIdx = i;
            break;
        }
    }

    // 기존 요소들 뒤로 밀기
    for (uint32_t i = m_systemCount; i > insertIdx; --i) {
        m_systems[i] = m_systems[i - 1];
    }

    m_systems[insertIdx] = { system, priority };
    m_systemCount++;
}

void SystemManager::Update(float dt)  {
    for (uint32_t i = 0; i < m_systemCount; ++i) {
        m_systems[i].ptr->update(dt, m_registry);
    }
}

void SystemManager::Shutdown()  {
    for (int i = 0; i < m_systemCount; ++i) {
        m_systems[i].ptr->~ISystem();
    }
}

}
