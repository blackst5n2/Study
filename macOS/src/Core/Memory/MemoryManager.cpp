#include "MemoryManager.h"
#include "FreeListAllocator.h"
#include "StackAllocator.h"
#include "PoolAllocator.h"
#include "ThreadSafeProxy.h"
#include "../Threading/Common.h"
#include "../Logger.h"

namespace Core {

MemoryManager& MemoryManager::Get() {
    static MemoryManager instance;
    return instance;
}

bool MemoryManager::Init() {
    if (m_initialized) return true;
    
    // Ring Buffer용 가상 메모리 페이지 미러링
    // 128KB 권한 없는 익명 페이지 예약
    m_recvPlaceholder = mmap(NULL, RECV_BUFFER_SIZE * 2, PROT_NONE, MAP_PRIVATE | MAP_ANONYMOUS, -1, 0);
    
    if (m_recvPlaceholder == MAP_FAILED) {
        perror("mmap placeholer failed");
        return false;
    }
    
    // 유니크한 이름 생성
    std::string shm_name = "/recv_ring_buffer_" + std::to_string(getpid());
    
    // 물리 저장소 확보
    int fd = shm_open(shm_name.c_str(), O_RDWR | O_CREAT, S_IRUSR | S_IWUSR);
    if (fd < 0) {
        perror("shm_open failed");
        return false;
    }
    
    // 열자마자 언링크 중요
    shm_unlink(shm_name.c_str());
    
    // 실제 물리 메모리 크기 할당
    ftruncate(fd, RECV_BUFFER_SIZE);
    
    // 앞부분 매핑
    void* addr1 = mmap(m_recvPlaceholder, RECV_BUFFER_SIZE, PROT_READ | PROT_WRITE, MAP_SHARED | MAP_FIXED, fd, 0);
    // 뒷부분 매핑
    void* addr2 = mmap((uint8_t*)m_recvPlaceholder + RECV_BUFFER_SIZE, RECV_BUFFER_SIZE, PROT_READ | PROT_WRITE, MAP_SHARED | MAP_FIXED, fd, 0);
    
    if (addr1 == MAP_FAILED || addr2 == MAP_FAILED) {
        close(fd);
        munmap(m_recvPlaceholder, RECV_BUFFER_SIZE * 2);
        perror("Mirror mapping failed");
        return false;
    }
    
    // fd는 매핑 후 바로 닫아도 됨
    close(fd);

    // 1. 시스템 전체 메모리 확보 (4GB)
    m_totalSize = 4096LL * 1024 * 1024;
    m_globalHeapPtr = ::malloc(m_totalSize);
    if (!m_globalHeapPtr) return false;
    m_usedSize = 0;

    // 2. 구획 분할 및 할당자 주입 (순서 중요)
    // Heap
    size_t heapSize = 512 * 1024 * 1024;
    void* heapPtr = GetRawSlab(heapSize);
    void* heapAllocPtr = GetRawSlab(sizeof(FreeListAllocator));
    void* heapProxyPtr = GetRawSlab(sizeof(ThreadSafeProxy));
    auto* rawHeap = new (heapAllocPtr) FreeListAllocator(heapPtr, heapSize);
    m_pGeneralHeap = new (heapProxyPtr) ThreadSafeProxy(rawHeap);

    // Stack
    size_t stackSize = 10 * 1024 * 1024;
    void* stackPtr = GetRawSlab(stackSize);
    void* stackAllocPtr = GetRawSlab(sizeof(StackAllocator));
    void* stackProxyPtr = GetRawSlab(sizeof(ThreadSafeProxy));
    auto* rawStack = new (stackAllocPtr) StackAllocator(stackPtr, stackSize);
    m_pFrameStack = new (stackProxyPtr) ThreadSafeProxy(rawStack); 

    // Component Pool
    size_t poolSize = 10 * 1024 * 1024;
    size_t slotSize = 64;
    void* poolData = GetRawSlab(poolSize);
    void* poolAllocPtr = GetRawSlab(sizeof(PoolAllocator));
    void* poolProxyPtr = GetRawSlab(sizeof(ThreadSafeProxy));
    auto* rawPool = new (poolAllocPtr) PoolAllocator(poolData, poolSize, slotSize);
    m_pComponentPool = new (poolProxyPtr) ThreadSafeProxy(rawPool);

    // Network 전용
    size_t networkSize = 1024 * 1024 * 1024; // 200MB 확보
    void* networkPtr = GetRawSlab(networkSize);
    void* networkAllocPtr = GetRawSlab(sizeof(FreeListAllocator));
    void* networkProxyPtr = GetRawSlab(sizeof(ThreadSafeProxy));
    auto* rawNetwork = new (networkAllocPtr) FreeListAllocator(networkPtr, networkSize);
    m_pNetworkHeap = new (networkProxyPtr) ThreadSafeProxy(rawNetwork);
    
    size_t sessionSize = 10 * 1024 * 1024;
    size_t sessionSlotSize = 192;
    void* sessionPtr = GetRawSlab(sessionSize);
    void* sessionAllocPtr = GetRawSlab(sizeof(PoolAllocator));
    void* sessionProxyPtr = GetRawSlab(sizeof(ThreadSafeProxy));
    auto* rawSession = new (sessionAllocPtr) PoolAllocator(sessionPtr, sessionSize, sessionSlotSize);
    m_pSessionPool = new (sessionProxyPtr) ThreadSafeProxy(rawSession);

    // 3. 테이블 매핑
    m_allocatorTable[(int)MemoryTag::Default]   = m_pGeneralHeap;
    m_allocatorTable[(int)MemoryTag::GameLogic] = m_pGeneralHeap;
    m_allocatorTable[(int)MemoryTag::Temporary] = m_pFrameStack;
    m_allocatorTable[(int)MemoryTag::Component] = m_pComponentPool;
    m_allocatorTable[(int)MemoryTag::Permanent] = m_pGeneralHeap;
    m_allocatorTable[(int)MemoryTag::Network] = m_pNetworkHeap;
    m_allocatorTable[(int)MemoryTag::Session] = m_pSessionPool;

    m_initialized = true;
    LOG_INFO("MemoryManager: Systems Placed in Global Heap Successfully.");
    return true;
}

void MemoryManager::Shutdown() {
    if (!m_initialized) return;

    if (m_pGeneralHeap) m_pGeneralHeap->~ThreadSafeProxy();
    if (m_pFrameStack) m_pFrameStack->~ThreadSafeProxy();
    if (m_pComponentPool) m_pComponentPool->~ThreadSafeProxy();
    
    if (m_globalHeapPtr) {
        ::free(m_globalHeapPtr);
        m_globalHeapPtr = nullptr;
    }

    m_initialized = false;
    LOG_INFO("MemoryManager: Global Heap Released.");
}

void* MemoryManager::Allocate(size_t size, MemoryTag tag, size_t alignment) {
    if (!m_initialized) return nullptr;

    Allocator* allocator = m_allocatorTable[static_cast<int>(tag)];
    if (allocator) {
        return allocator->allocate(size, alignment);
    }
    return nullptr;
}

void MemoryManager::Free(void* p, MemoryTag tag) {
    if (!p) return;
    
    ThreadBlockHeader* header = (ThreadBlockHeader*)((uint8_t*)p - sizeof(ThreadBlockHeader));
    if (header->magic != 0xDEADBEEF) {
        return; // 오염된 건 절대 다시 해제하면 안 됨
    }
    header->magic = 0;
    
    Allocator* allocator = m_allocatorTable[static_cast<int>(tag)];
    if (allocator && p) {
        allocator->deallocate(p);
    }
}

}
