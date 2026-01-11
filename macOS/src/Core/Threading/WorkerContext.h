#pragma once
#include "../Network/Session.h"
#include "../Network/PacketHeader.h"
#include <cstdint>
#include "LinearArena.h"
#include "../Network/Gen/Protocol_generated.h"

namespace Core {

struct WorkerContext {
    Session* session;
    uint8_t* payload;
    size_t payloadSize;
    uint16_t type;
    LinearArena* arena;
    flatbuffers::FlatBufferBuilder* fbb;
};

}