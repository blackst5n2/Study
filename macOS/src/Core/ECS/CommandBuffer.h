#pragma once
#include <cstdint>
#include "Registry.h"

namespace Core {

enum class CommandType : uint8_t {
    CreateEntity,
    DestroyEntity,
    AddComponent,
    RemoveComponent,
};

struct Command {
    CommandType type;
    uint32_t entityId;
    uint32_t componentTypeId;
    void* data; // 추가될 컴포넌트 데이터 주소
};

class CommandBuffer {
private:
    Command* m_buffer;
    uint32_t m_head = 0;
    uint32_t m_capacity;
    Registry& m_registry;

public:
    CommandBuffer(Registry& reg, uint32_t maxCommands);

    // 엔티티 삭제 예약
    void destroyEntity(uint32_t eId);

    // 컴포넌트 추가 예약
    template<typename T>
    void addComponent(uint32_t eId, const T& component);

    // 쌓인 명령들을 한꺼번에 실행
    void execute();
};

}