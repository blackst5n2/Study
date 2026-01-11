#pragma once
#include "../Threading/WorkerContext.h"
#include "Gen/Protocol_generated.h"
#include <memory>
#include "PacketList.h"

namespace Core {
// 핸들러 함수 선언 자동화
#define DECLARE_HANDLER(name) \
    void Handle_##name(const WorkerContext& ctx, const Protocol::GamePacket* packet);

FOR_EACH_PACKET(DECLARE_HANDLER)

#undef DECLARE_HANDLER

}
