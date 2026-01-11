#pragma once
#include <cstddef>
#include "MemoryManager.h"
#include "../Threading/LocalArena.h"
#include "../Threading/Common.h"
#include "../Threading/ThreadManager.h"

namespace Core {
    inline void* Alloc(size_t size, MemoryTag tag = MemoryTag::Default, size_t alignment = 4) {
        auto* local = GetLocalArena();
        if (local) {
            return local->Allocate(size, tag, alignment);
        }

        // 메인 스레드 등이라면 전역 힙(ThreadSafeProxy + FreeList) 사용
        void* rawPtr = MemoryManager::Get().Allocate(size, tag, alignment);
        if (!rawPtr) return nullptr;

        ThreadBlockHeader* header = static_cast<ThreadBlockHeader*>(rawPtr);
        header->ownerId = CENTRAL_ID; // 전역 힙 소유
        header->tag = tag;
        header->magic = 0xDEADBEEF;

        return static_cast<uint8_t*>(rawPtr) + sizeof(ThreadBlockHeader);
    }

    inline void Free(void* ptr) {
        if (!ptr) return;

        ThreadBlockHeader* header = reinterpret_cast<ThreadBlockHeader*>(
            static_cast<uint8_t*>(ptr) - sizeof(ThreadBlockHeader)
        );

        if (header->magic != 0xDEADBEEF) {
            LOG_ERR("Memory Corruption detected or invalid free call!");
            return;
        }
        
        Core::ThreadId myId = Core::GetCurrentThreadId();

        // 전역 풀 or 내가 주인
        if (header->ownerId == CENTRAL_ID || header->ownerId == myId) {
            if (header->ownerId == CENTRAL_ID)
                MemoryManager::Get().Free(header, header->tag);
            else
                GetLocalArena()->PushRemote(header);
        }
        // 원격 반납
        else {
            // 주인을 찾을 수 없는 경우에만 전역 힙으로
            auto* target = ThreadManager::GetInstance().GetLocalArena(header->ownerId);
            if (target) target->PushRemote(header);
            else MemoryManager::Get().Free(header, header->tag);
        }
    }
}
