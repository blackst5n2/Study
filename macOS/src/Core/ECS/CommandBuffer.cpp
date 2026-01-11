#include "CommandBuffer.h"

namespace Core {

CommandBuffer::CommandBuffer(Registry& reg, uint32_t maxCommands)
    : m_registry(reg), m_capacity(maxCommands) {
    m_buffer = (Command*)MemoryManager::Get().Allocate(sizeof(Command) * m_capacity, MemoryTag::Permanent);
}

void CommandBuffer::destroyEntity(uint32_t eId) {
    if (m_head < m_capacity) {
        m_buffer[m_head++] = { CommandType::DestroyEntity, eId, 0, nullptr };
    }
}

template<typename T>
void CommandBuffer::addComponent(uint32_t eId, const T& component) {
    if (m_head < m_capacity) {
        // 데이터는 별도의 임시 Slab 공간에 복사해두어야 함
        void* copy = MemoryManager::Get().Allocate(sizeof(T), MemoryTag::Temporary);
        memcpy(copy, &component, sizeof(T));

        m_buffer[m_head++] = { CommandType::AddComponent, eId, ComponentTypeManager::getID<T>(), copy };
    }
}

void CommandBuffer::execute() {
    for (uint32_t i = 0; i < m_head; ++i) {
        Command& cmd = m_buffer[i];
        switch (cmd.type) {
            case CommandType::DestroyEntity:
                m_registry.destroy(cmd.entityId);
                break;
            case CommandType::AddComponent:
                break;
            default:
                break;
        }
    }
    m_head = 0;
}

}