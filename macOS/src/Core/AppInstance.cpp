#include "AppInstance.h"
#include "Memory/MemoryManager.h"
#include "Threading/ThreadManager.h"
#include "Network/NetworkManager.h"
#include "Network/PacketDispatcher.h"
#include "Network/Gen/Protocol_generated.h"
#include <iostream>
#include <csignal>

#define PORT 8080

namespace Core {

bool AppInstance::Init() {
    if (m_isInitialized) return true;
    PacketDispatcher::GetInstance().Init();

    // 1. 메모리 매니저 초기화 (최우선 순위)
    // 모든 LocalArena의 Slab 할당이 가능해지는 지점
    if (!MemoryManager::Get().Init()) {
        std::cerr << "[Critical] MemoryManager Init Failed!" << std::endl;
        return false;
    }
    
    // 2. 메인 스레드 등록
    ThreadManager::GetInstance().InitThread();

    // 3. 네트워크 매니저 초기화
    NetworkManager::GetInstance().Init(PORT);
    std::signal(SIGPIPE, SIG_IGN);

    std::cout << "[App] All systems initialized successfully." << std::endl;
    m_isInitialized = true;
    return true;
}

void AppInstance::Run() {
    // 서버가 죽지 않도록 메인 스레드 유지
    while(true) {
        Core::NetworkManager::GetInstance().Update();
    }
}

void AppInstance::Shutdown() {
    // 종료는 역순으로 진행
    NetworkManager::GetInstance().Shutdown();
    ThreadManager::GetInstance().Shutdown();
    MemoryManager::Get().Shutdown();
}

}
