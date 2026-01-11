#pragma once
#include <cstdint>

namespace Core {

#pragma pack(push, 1) // 패딩 없이 1바이트 단위로 정렬 (네트워크 전송용)
struct PacketHeader {
    uint16_t magic;
    uint16_t type;
    uint32_t size;
};
#pragma pack(pop)

}

