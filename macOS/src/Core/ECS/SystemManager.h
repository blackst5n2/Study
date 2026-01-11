#pragma once
#include "ISystem.h"
#include "../Memory/MemoryHelpers.h"

namespace Core {

class SystemManager {
private:
    struct SystemEntry {
        ISystem* ptr;
        int priority;
    };
    // 우선순위별로 시스템을 정렬하여 보관
    SystemEntry m_systems[64];
    uint32_t m_systemCount = 0;
    Registry& m_registry;

public:
    SystemManager(Registry& reg) : m_registry(reg) {}

    template<typename T, typename... Args>
    void addSystem(int priority, Args&&... args);

    void Update(float dt);

    void Shutdown();
};

}
