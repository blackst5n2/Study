```cpp
#pragma once
#include <cstdint>
#include <cstddef>
#include <string>

  

namespace Core {

  

class PacketReader {

public:

PacketReader(uint8_t* data, size_t size) : m_data(data), m_size(size), m_pos(0) {};

  

// 기본 타입 읽기

template<typename T>

T Read() {

if (m_pos + sizeof(T) > m_size) {

// 에러 처리 로직

return T();

}

T value;

memcpy(&value, m_data + m_pos, sizeof(T));

m_pos += sizeof(T);

return value;

}

  

// 문자열 읽기 (2바이트 길이 + 문자열 데이터)

std::string ReadString() {

uint16_t len = Read<uint16_t>();

if (len == 0 || m_pos + len > m_size) return "";

  

std::string str(reinterpret_cast<const char*>(m_data + m_pos), len);

m_pos += len;

return str;

}

  

size_t RemainingSize() const { return m_size - m_pos; }

  

private:

uint8_t* m_data;

size_t m_size;

size_t m_pos;

};

  

}
```
