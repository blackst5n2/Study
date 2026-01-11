#pragma once

#include <cstdint>
#include <cstddef>
#include <atomic>

namespace Core {

struct Metrics {
    std::atomic<size_t> highWatermark{0};
    std::atomic<size_t> overflowCount{0};
    std::atomic<uint32_t> sendCallCount{0};

    void UpdateUsage(size_t usage) {
        size_t currentHW = highWatermark.load(std::memory_order_relaxed);
        while (usage > currentHW && 
                !highWatermark.compare_exchange_weak(currentHW, usage, std::memory_order_relaxed));
    }

    void AddOverflow() { overflowCount.fetch_add(1, std::memory_order_relaxed); }
};

class RecvMirrorRingBuffer {
public:
    RecvMirrorRingBuffer() = default;
    
    void Init(void* buffer, size_t capacity) {
        m_buffer = static_cast<uint8_t*>(buffer);
        m_capacity = capacity;
        m_mask = capacity - 1;
        m_writePos.store(0);
        m_readPos.store(0);
    }

    // 데이터를 쓸 위치 반환
    void* getWritePtr() {
        return m_buffer + (m_writePos.load() & m_mask);
    }

    // 데이터를 쓴 만큼 포인터 전진 (NetworkManager가 호출)
    void advanceWritePtr(size_t len) {
        m_writePos.fetch_add(len, std::memory_order_relaxed);
    } 

    // 데이터를 읽은 만큼 포인터 전진 (WorkerThread가 호출)
    void advanceReadPtr(size_t len) {
        m_readPos.fetch_add(len, std::memory_order_relaxed);
    }

    // 현재 남은 공간 계산 (DDoS/폭주 방지용 Safety Valve)
    size_t getWritableSize() const {
        size_t write = m_writePos.load(std::memory_order_acquire);
        size_t read = m_readPos.load(std::memory_order_acquire);
        size_t used = write - read; // 항상 양수 (오버플로우 시에도 보수 연산으로 유지됨)
        
        if (used >= m_capacity) return 0;
        return m_capacity - used;
    }

    uint8_t* getBasePtr() const { return m_buffer; }
    size_t getCapacity() const { return m_capacity; }

    Metrics& getMetrics() { return m_metrics; }

private:
    uint8_t* m_buffer = nullptr;
    size_t m_capacity = 0;
    size_t m_mask = 0;

    std::atomic<size_t> m_writePos{0};
    std::atomic<size_t> m_readPos{0};

    RecvMirrorRingBuffer(const RecvMirrorRingBuffer&) = delete;
    RecvMirrorRingBuffer& operator=(const RecvMirrorRingBuffer&) = delete;

    Metrics m_metrics;
};

}