#include "PacketDispatcher.h"
#include "PacketHandlers.h"
#include "../Logger.h"
#include "PacketList.h"

namespace Core {

void PacketDispatcher::Init() {
    // 등록 자동화 매크로
    #define REGISTER_HANDLER(name) \
        m_handlers[Protocol::PacketPayload_##name] = Handle_##name;
    
    FOR_EACH_PACKET(REGISTER_HANDLER)

    #undef REGISTER_HANDLER
}

}
