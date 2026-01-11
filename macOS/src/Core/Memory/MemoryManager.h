#pragma once

#include "ThreadSafeProxy.h"
#include "MemoryTags.h"
#include <memory>
#include <mutex>
#include "../Logger.h"
#include <sys/mman.h>
#include <sys/stat.h>
#include <unistd.h>
#include <fcntl.h>

namespace Core {

class MemoryManager {

// getpagesize()
const size_t PAGE_SIZE = 16384;
// 링버퍼 크기
const size_t RECV_BUFFER_SIZE = PAGE_SIZE * 64 * 256; // 256MB
const size_t SEND_BUFFER_SIZE = 128 * 1024; // 128KB
    
public:
    // 싱글톤 인스턴스
    static MemoryManager& Get();

    // 시스템 초기화: 각 용더별 할당자들의 크기를 설정
    bool Init();
    void Shutdown();

    // 메인 인터페이스
    void* Allocate(size_t size, MemoryTag tag, size_t alignment = 4);
    void Free(void* p, MemoryTag tag);

    void* GetRawSlab(size_t size) {
        // 정렬
        size = (size + 7) & ~7;

        std::lock_guard<std::mutex> lock(m_mutex);

        // 남은 공간 체크
        if (m_usedSize + size > m_totalSize) {
            LOG_ERR("Global Alloc Failed! Current Used: ", m_usedSize, " Requested: ", size, " Max: ", m_totalSize);
            return nullptr;
        }
        // 현재 사용 중인 포인터 위치 저장
        void* ptr = static_cast<uint8_t*>(m_globalHeapPtr) + m_usedSize;

        // 사용량 업데이트 
        m_usedSize += size;

        return ptr;
    }

    void* CreatePlaceholder() {
        // 교유 ID 생성
        static std::atomic<uint64_t> s_bufferCount {0};
        uint64_t bufferId = s_bufferCount.fetch_add(1);

        // 128KB 권한 없는 익명 페이지 예약
        void* placeholder = mmap(NULL, SEND_BUFFER_SIZE * 2, PROT_NONE, MAP_PRIVATE | MAP_ANONYMOUS, -1, 0);
        
        if (placeholder == MAP_FAILED) {
            perror("mmap placeholer failed");
            return nullptr;
        }
        
        // 유니크한 이름 생성
        char shm_name[64];
        snprintf(shm_name, sizeof(shm_name), "/net_buf_%d_%llu", getpid(), bufferId);
        
        // 물리 저장소 확보
        int fd = shm_open(shm_name, O_RDWR | O_CREAT, S_IRUSR | S_IWUSR);
        if (fd < 0) {
            perror("shm_open failed");
            return nullptr;
        }
        
        // 열자마자 언링크 중요
        shm_unlink(shm_name);
        
        // 실제 물리 메모리 크기 할당
        ftruncate(fd, SEND_BUFFER_SIZE);
        
        // 앞부분 매핑
        void* addr1 = mmap(placeholder, SEND_BUFFER_SIZE, PROT_READ | PROT_WRITE, MAP_SHARED | MAP_FIXED, fd, 0);
        // 뒷부분 매핑
        void* addr2 = mmap((uint8_t*)placeholder + SEND_BUFFER_SIZE, SEND_BUFFER_SIZE, PROT_READ | PROT_WRITE, MAP_SHARED | MAP_FIXED, fd, 0);
        
        if (addr1 == MAP_FAILED || addr2 == MAP_FAILED) {
            perror("Mirror mapping failed");
            return nullptr;
        }
        
        // fd는 매핑 후 바로 닫아도 됨
        close(fd);

        return placeholder;
    }
    
    void* GetRecvPlaceholder() { return m_recvPlaceholder; }
    void RemoveRecvPlaceholder() { if (m_recvPlaceholder) munmap(m_recvPlaceholder, RECV_BUFFER_SIZE * 2); }
    size_t GetRecvBufferSize() { return RECV_BUFFER_SIZE; }
    size_t GetSendBufferSize() { return SEND_BUFFER_SIZE; }

private:
    MemoryManager() = default;
    ~MemoryManager() = default;

    bool m_initialized = false;
    
    void* m_recvPlaceholder = nullptr;

    size_t m_usedSize;
    size_t m_totalSize;
    void* m_globalHeapPtr;
    std::mutex m_mutex;
    
    // 실제 할당자 인스턴스들
    ThreadSafeProxy* m_pGeneralHeap = nullptr; // FreeList or Fallback
    ThreadSafeProxy* m_pFrameStack = nullptr; // Stack Allocator
    ThreadSafeProxy* m_pComponentPool = nullptr; // Pool Allocator
    ThreadSafeProxy* m_pNetworkHeap = nullptr; // FreeList
    ThreadSafeProxy* m_pSessionPool = nullptr; // Pool Allocator

    // 각 태그별로 어떤 할당자를 쓸지 매핑
    static constexpr size_t TAG_COUNT = static_cast<size_t>(MemoryTag::Permanent) + 1;
    ThreadSafeProxy* m_allocatorTable[TAG_COUNT] = { nullptr };
};

}
