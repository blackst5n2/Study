#pragma once

#include "MemoryTags.h"
#include "Memory.h"
#include <new> // Placement new
#include <utility> // std::forward

namespace Core {

// Core::New
// 사용법: Monster* m = Core::New<Monster>(MemoryTag::GameLogic, "Orc", 100);
template<typename T, typename... Args>
T* New(MemoryTag tag, Args&&... args) 
{
    // 1. 메모리 할당 (객체의 크기와 정렬 기준 전달)
    void* rawPtr = Core::Alloc(sizeof(T), tag, alignof(T));
    if (!rawPtr) return nullptr;

    // 2. Placement New: 할당된 주소에서 생성자 호출
    // std::forwaard는 인자들을 완벽하게 전달(Perfect Forwarding)하기 위함.
    return new (rawPtr) T(std::forward<Args>(args)...);
}

// Core::Delete
// 사용법: Core::Delete(myMonster, MemoryTag::GameLogic);
template<typename T>
void Delete(T* ptr, MemoryTag tag)
{
    if (!ptr) return;

    // 1. 소멸자 수동 호출
    ptr->~T();

    // 2. 메모리 해제
    Core::Free(ptr);
}

}
