#pragma once

#include "Core/ECS/ComponentRules.h"

namespace Components
{
    struct TransformComponent {
        // TODO
    };

    // 매크로 호출 한 번으로 3가지 규칙이 컴파일 타임에 강제됨.
    ENFORCE_COMPONENT_RULES(TransformComponent);
}