#pragma once
#include <cstddef>
#include <cstdint>
#include "Entity.h"

namespace Core {

class IStorage {
public:
    virtual ~IStorage() = default;
    virtual void removeRaw(uint32_t entityIdx) = 0;
    virtual uint32_t* getEntityList() = 0;
    virtual uint32_t size() = 0;
};

}