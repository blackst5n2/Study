#pragma once
#include <cstdint>
#include <cstddef>
#include "../Memory/MemoryTags.h"

namespace Core {

using ThreadId = uint8_t;

const ThreadId CENTRAL_ID = 0; // 도매상(Central) ID

struct alignas(32) ThreadBlockHeader {
    ThreadId ownerId; // 할당한 스레드의 고유 ID
    MemoryTag tag;
    uint32_t magic; // 0x DEADBEEF (메모리 오염 및 정합성 체크용)
    ThreadBlockHeader* next = nullptr; // MPSC 큐를 위한 침습형(Intrusive) 포인터
};

static_assert(sizeof(ThreadBlockHeader) == 32, "Header size must be 32 bytes for alignment safety.");

}