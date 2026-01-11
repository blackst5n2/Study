#pragma once
#include "Registry.h"

namespace Core {

class ISystem {
public:
    virtual ~ISystem() = default;
    virtual void update(float dt, Registry& registry) = 0;
};

}