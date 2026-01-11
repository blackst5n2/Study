#pragma once
#include "Session.h"
#include "../Memory/MemoryHelpers.h"
#include <mutex>
#include <atomic>
#include <stack>

namespace Core {

const int MAX_SESSION_COUNT = 10000;
const int MAX_CONNECTION_FD = 65536; // 리눅스/Unix 계열 기본 ulimit 고려

class SessionManager {
public:
    SessionManager();
    ~SessionManager();

    // 초기화 (메모리 할당)
    void Init();

    // 새로운 세션 생성 (O(1))
    Session* CreateSession(int fd);

    // 세션 제거 (O(1))
    void RemoveSession(uint64_t id);

    // [최적화] Lock-Free 조회 (O(1)) - 가장 빈번하게 호출됨
    inline Session* GetSession(uint64_t id) {
        // 1. 인덱스 추출 및 범위 검사
        uint32_t index = static_cast<uint32_t>(id & 0xFFFFFFFF);
        if (index >= MAX_SESSION_COUNT) return nullptr;

        // 2. 세대(Generation) 검증 (Atomic Load)
        // 락 없이 메모리 가시성만 보장받아 즉시 비교
        uint64_t targetGen = id >> 32;
        if (m_generations[index].load(std::memory_order_relaxed) != targetGen) {
            return nullptr;
        }

        // 3. 세션 포인터 반환 (세션이 Active 상태인지 재확인)
        Session* session = m_sessions[index];
        if (!session || !session->IsActive()) return nullptr;

        return session;
    }

    // [최적화] 배열 인덱스 접근 (O(1))
    inline Session* GetSessionByFD(int fd) {
        if (fd < 0 || fd >= MAX_CONNECTION_FD) return nullptr;
        // Atomic Load로 안전하게 읽기
        return m_fdTable[fd].load(std::memory_order_relaxed);
    }
    
private:
    // 세션 객체 풀 (포인터 배열)
    Session* m_sessions[MAX_SESSION_COUNT] = {nullptr};

    // [변경] 세대 관리를 위한 Atomic 배열 (Lock-Free 핵심)
    // 힙에 할당하여 스택 오버플로우 방지 및 정렬 보장
    std::atomic<uint32_t>* m_generations; 

    // [추가] FD로 세션을 바로 찾기 위한 룩업 테이블 (O(1))
    std::atomic<Session*>* m_fdTable;

    std::stack<int> m_freeIndices;
    std::mutex m_lock; // Create/Remove 동기화용 (조회에는 안 씀!)
};

}