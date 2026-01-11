#include "SessionManager.h"
#include "../Memory/MemoryHelpers.h"
#include <iostream>

namespace Core {

SessionManager::SessionManager() {
    Init();
}

SessionManager::~SessionManager() {
    // 할당된 배열들 해제 (Custom Free)
    if (m_generations) {
        // 소멸자 호출 필요 없음 (primitive type atomic)
        Core::Free(m_generations);
    }
    if (m_fdTable) {
        Core::Free(m_fdTable);
    }

    for (int i = 0; i < MAX_SESSION_COUNT; ++i) {
        if (m_sessions[i]) {
            Core::Delete(m_sessions[i], MemoryTag::Session);
        }
    }
}

void SessionManager::Init() {
    // 1. 세션 객체 미리 생성 (Object Pool)
    for (int i = 0; i < MAX_SESSION_COUNT; ++i) {
        m_sessions[i] = Core::New<Session>(MemoryTag::Session);
        m_freeIndices.push(i);
    }

    // 2. Generation 배열 할당 (Custom Allocator 사용)
    // std::atomic<uint32_t> 배열 크기만큼 할당
    size_t genArraySize = sizeof(std::atomic<uint32_t>) * MAX_SESSION_COUNT;
    void* genMem = Core::Alloc(genArraySize, MemoryTag::Session, alignof(std::atomic<uint32_t>));
    
    // Placement New로 초기화 (Atomic은 생성자 호출 권장)
    m_generations = static_cast<std::atomic<uint32_t>*>(genMem);
    for(int i=0; i<MAX_SESSION_COUNT; ++i) {
        new (&m_generations[i]) std::atomic<uint32_t>(1); // 초기 세대 1
    }

    // 3. FD Lookup Table 할당 (O(1) 조회를 위함)
    size_t fdTableSize = sizeof(std::atomic<Session*>) * MAX_CONNECTION_FD;
    void* fdMem = Core::Alloc(fdTableSize, MemoryTag::Session, alignof(std::atomic<Session*>));
    
    m_fdTable = static_cast<std::atomic<Session*>*>(fdMem);
    for(int i=0; i<MAX_CONNECTION_FD; ++i) {
        new (&m_fdTable[i]) std::atomic<Session*>(nullptr);
    }

    LOG_INFO("SessionManager Initialized with Custom Memory. Max Sessions:", MAX_SESSION_COUNT);
}

Session* SessionManager::CreateSession(int fd) {
    int index = -1;
    uint64_t newId = 0;

    {   // 락 범위를 최소화
        std::lock_guard<std::mutex> lock(m_lock);
        if (m_freeIndices.empty()) return nullptr;

        index = m_freeIndices.top();
        m_freeIndices.pop();
        
        uint32_t currentGen = m_generations[index].load(std::memory_order_relaxed);
        newId = (static_cast<uint64_t>(currentGen) << 32) | (static_cast<uint32_t>(index));
    }

    // 세션 초기화
    Session* session = m_sessions[index];
    session->Init(newId, fd);

    // FD 테이블에 등록 (O(1) 접근용)
    if (fd >= 0 && fd < MAX_CONNECTION_FD) {
        m_fdTable[fd].store(session, std::memory_order_release);
    }

    return session;
}

void SessionManager::RemoveSession(uint64_t id) {
    uint32_t index = static_cast<uint32_t>(id);
    if (index >= MAX_SESSION_COUNT) return;

    Session* session = m_sessions[index];
    
    // FD 테이블에서 제거
    int fd = session->GetFd();
    if (fd >= 0 && fd < MAX_CONNECTION_FD) {
        m_fdTable[fd].store(nullptr, std::memory_order_release);
    }

    // 세션 리셋
    session->Reset();

    {
        std::lock_guard<std::mutex> lock(m_lock);
        m_generations[index].fetch_add(1, std::memory_order_release);
        m_freeIndices.push(index);
    }
}

}