#pragma once
#include <cstdint>

namespace Core {

class ComponentTypeManager {
private:
    static inline uint64_t nextID = 0;

public:
    template<typename T>
    static uint64_t getID() {
        // 이 함수를 호출하는 타입 T마다 오직 하나의 static 변수가 생성됨
        static uint64_t id = nextID++;
        return id;
    }
};

}