#pragma once

#include <cstdint>
#include <cstddef>
#include <atomic>

namespace Core {

class SendMirrorRingBuffer {
public:
    SendMirrorRingBuffer() = default;
    ~SendMirrorRingBuffer() = default;
    
    void Init(void* buffer, size_t capacity) {
        m_buffer = static_cast<uint8_t*>(buffer);
        m_capacity = capacity;
        m_mask = capacity - 1;
    }

    // 포인터를 읽기 전용으로 변환
    uint8_t* getBasePtr() const { return m_buffer; }
    size_t getCapacity() const { return m_capacity; }

    // -- send ---
    // 절대 위치 받아서 실제 메모리 주소로 변환
    void WriteAt(uint64_t pos, const uint8_t* data, uint32_t len) {
        memcpy(m_buffer + (pos & m_mask), data, len);
    }

    // 읽기 위치 주소 변환
    uint8_t* GetReadPtr(uint64_t pos) const {
        return m_buffer + (pos & m_mask);
    }

    // 받은 위치 기록
    void MoveReadPos(uint64_t newReadPos) {
        m_readPos.store(newReadPos, std::memory_order_release);
    }

    uint64_t GetReadPos() const { return m_readPos.load(std::memory_order_acquire); }

private:
    uint8_t* m_buffer = nullptr;
    size_t m_capacity = 0;
    std::atomic<uint64_t> m_readPos = {0}; // 전송 완료된 절대 위치
    size_t m_mask = 0;

    // 복사 방지
    SendMirrorRingBuffer(const SendMirrorRingBuffer&) = delete;
    SendMirrorRingBuffer& operator=(const SendMirrorRingBuffer&) = delete;
};

}
