#pragma once

#include "Core/ECS/ComponentRules.h"

namespace Components
{
    struct TestComponent
    {
        // 컴포넌트 데이터 멤버 선언
    };
    
    ENFORCE_COMPONENT_RULES(TestComponent);
}