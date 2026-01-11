#pragma once
#include <cstddef>
#include <cstdint>
#include <atomic>
#include "../Memory/MemoryManager.h"

namespace Core {

struct QueueMetrics {
    std::atomic<size_t> highWatermark{0};
    std::atomic<size_t> pushCount{0};

    void UpdateDepth(size_t currentDepth) {
        size_t currentHW = highWatermark.load(std::memory_order_relaxed);
        while (currentDepth > currentHW && 
               !highWatermark.compare_exchange_weak(currentHW, currentDepth, std::memory_order_relaxed));
        pushCount.fetch_add(1, std::memory_order_relaxed);
    }
};

struct PacketDescriptor {
    uintptr_t data_ptr;
    uint32_t length;
    uint64_t session_id;
};

struct SendDescriptor {
    uint64_t sessionId;
};

template<typename T>
class MPMCQueue {

private:
    struct Slot {
        T data;
        std::atomic<size_t> sequence;
    };
    
    static constexpr size_t CACHE_LINE_SIZE = 64;
    
    // flase sharing 방지용 alignment
    alignas(CACHE_LINE_SIZE) std::atomic<size_t> enqueue_pos;
    alignas(CACHE_LINE_SIZE) std::atomic<size_t> dequeue_pos;
    
    Slot* buffer;
    size_t buffer_mask;

    QueueMetrics m_metrics;
    
public:
    explicit MPMCQueue(size_t size) : buffer_mask(size - 1) {
        size_t allocSize = sizeof(Slot) * size;
        void* mem = MemoryManager::Get().Allocate(allocSize, MemoryTag::Permanent);
        buffer = static_cast<Slot*>(mem);

        for (size_t i = 0; i < size; ++i) {
        new (&buffer[i]) Slot(); // Placement new (기본 생성자 호출)
        buffer[i].sequence.store(i, std::memory_order_relaxed);
        }

        enqueue_pos.store(0, std::memory_order_relaxed);
        dequeue_pos.store(0, std::memory_order_relaxed);
    }
    
    bool push(const T& data) {
        Slot* slot;
        size_t pos = enqueue_pos.load(std::memory_order_relaxed);

        for (;;) {
            slot = &buffer[pos & buffer_mask];
            size_t seq = slot->sequence.load(std::memory_order_acquire);
            intptr_t diff = (intptr_t)seq - (intptr_t)pos;

            if (diff == 0) { // 빈 슬롯
                if (enqueue_pos.compare_exchange_weak(pos, pos + 1, std::memory_order_relaxed))
                    break;
            } else if (diff < 0 ) { // 큐 full
                return false;
            } else {
                pos = enqueue_pos.load(std::memory_order_relaxed);
            }
        }
        slot->data = data;
        slot->sequence.store(pos + 1, std::memory_order_release);

        size_t d_pos = dequeue_pos.load(std::memory_order_relaxed);
        if (pos >= d_pos) {
            size_t currentDepth = pos - d_pos;
            m_metrics.UpdateDepth(currentDepth);
        }

        return true;
    }
    
    bool pop(T& data) {
        Slot* slot;
        size_t pos = dequeue_pos.load(std::memory_order_relaxed);
        for (;;) {
            slot = &buffer[pos & buffer_mask];
            size_t seq = slot->sequence.load(std::memory_order_acquire);
            intptr_t diff = (intptr_t)seq - (intptr_t)(pos + 1);
            
            if (diff == 0) { // 데이터 존재함
                if (dequeue_pos.compare_exchange_weak(pos, pos + 1, std::memory_order_relaxed))
                break;
            } else if (diff < 0) { // 큐 empty
                return false;
            } else {
                pos = dequeue_pos.load(std::memory_order_relaxed);
            }
        }
        data = slot->data;
        slot->sequence.store(pos + buffer_mask + 1, std::memory_order_release);
        return true;
    }

    bool empty() const {
        // 넣은 위치와 뺀 위치가 같으면 비어있는 상태
        return enqueue_pos.load(std::memory_order_relaxed) == dequeue_pos.load(std::memory_order_relaxed);
    }

    QueueMetrics& GetMetrics() { return m_metrics; }
};

}
