#include "PacketHandlers.h"
#include "../Logger.h"

namespace Core {

void Handle_C_Chat(const WorkerContext& ctx, const Protocol::GamePacket* packet) {
    auto data = packet->payload_as_C_Chat();
    if (!data) return;
}

void Handle_C_Login(const WorkerContext& ctx, const Protocol::GamePacket* packet) {
    auto data = packet->payload_as_C_Login();
    if (!data) return;

    auto& fbb = *ctx.fbb;

    // 1. S_Login 내용물 생성
    bool success = true;
    uint32_t playerId = 12345; // DB에서 가져온 값이라 가정
    int64_t clientTimestamp = data->timestamp();
    auto loginResponse = Protocol::CreateS_Login(fbb, success, playerId, clientTimestamp);

    auto responsePacket = Protocol::CreateGamePacket(
        fbb, 
        Protocol::PacketPayload_S_Login, 
        loginResponse.Union()
    );
    
    fbb.Finish(responsePacket);
    
    // 헤더 조립
    uint32_t bodySize = fbb.GetSize();
    uint32_t totalSize = sizeof(PacketHeader) + bodySize;

    PacketHeader header;
    header.magic = 0xAAFF;
    header.type = static_cast<uint16_t>(Protocol::PacketPayload_S_Login);
    header.size = totalSize;

    uint8_t* sendBuffer = static_cast<uint8_t*>(ctx.arena->Allocate(totalSize));
    memcpy(sendBuffer, &header, sizeof(PacketHeader));
    memcpy(sendBuffer + sizeof(PacketHeader), fbb.GetBufferPointer(), bodySize);

    ctx.session->Send(sendBuffer, totalSize);
}

void Handle_C_Move(const WorkerContext& ctx, const Protocol::GamePacket* packet) {
    auto data = packet->payload_as_C_Move();
    LOG_INFO("Move Request: x=", data->x(), " y=", data->y());
}

void Handle_C_Test(const WorkerContext& ctx, const Protocol::GamePacket* packet) {
    auto data = packet->payload_as_C_Test();
    if (!data) return;
    
}

}
