```cpp
#pragma once

#include <cstdint>

#include <cstddef>

#include <memory>

#include <string>

#include "SendBuffer.h"

#include "PacketHeader.h"

#include "Session.h"

  

namespace Core {

  

class PacketWriter {

public:

PacketWriter(uint16_t packetId) : m_id(packetId) {

// 최대 패킷 사이즈로 할당

m_sendBuffer = Core::SendBuffer::Create(Core::Session::MAX_PACKET_SIZE);

m_pos = sizeof(PacketHeader); // 헤더 공간은 비워두고 시작

}

  

template<typename T>

void Write(T value) {

if (m_pos + sizeof(T) > Core::Session::MAX_PACKET_SIZE) return;

memcpy(m_sendBuffer->GetBuffer() + m_pos, &value, sizeof(T));

m_pos += sizeof(T);

}

  

void WriteString(const std::string& str) {

uint16_t len = static_cast<uint16_t>(str.size());

Write<uint16_t>(len);

  

if (m_pos + len > m_sendBuffer->GetCapacity()) return;

memcpy(m_sendBuffer->GetBuffer() + m_pos, str.c_str(), len);

m_pos += len;

}

  

// 패킷 완성

std::shared_ptr<SendBuffer> Finalize() {

PacketHeader* header = reinterpret_cast<PacketHeader*>(m_sendBuffer->GetBuffer());

header->size = static_cast<uint16_t>(m_pos);

header->id = m_id;

header->magic = 0x12345678;

  

m_sendBuffer->Close(m_pos);

return m_sendBuffer;

}

  

private:

uint16_t m_id;

std::shared_ptr<SendBuffer> m_sendBuffer;

size_t m_pos;

};

}
```