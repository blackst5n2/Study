#pragma once
#include "ComponentTypeManager.h"
#include "ComponentStorage.h"
#include "Entity.h"
#include "Group.h"
#include <tuple>

namespace Core {

template<typename... Components>
class View {
private:
    Group* m_group;
    std::tuple<ComponentStorage<Components>*...> storages;

public:
    View(Group* g, ComponentStorage<Components>*... s) : m_group(g), storages(s...) {}

    // 가변 인자 람다를 받는 each
    template<typename Func>
    void each(Func&& func);
};

}
