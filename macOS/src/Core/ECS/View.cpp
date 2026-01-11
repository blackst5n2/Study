#include "View.h"

namespace Core {

template<typename... Components>
template<typename Func>
void View<Components...>::each(Func&& func) {
    if (!m_group) return;

    uint32_t* entities = m_group->entities;
    uint32_t count = m_group->count;

    for (uint32_t i = 0; i < count; ++i) {
        uint32_t eId = entities[i];

        // 람다 함수에 엔티티 ID와 각 Storage에서 꺼낸 컴포넌트 참조를 직접 주입
        func(eId, std::get<ComponentStorage<Components>*>(storages)->get(eId)...);
    }
}

}