#include <gtest/gtest.h>
#include <thread>
#include <vector>
#include "../Core/Memory/MemoryManager.h"

using namespace Core;

TEST(ThreadSafetyTests, StressTest) {
    MemoryManager::Get().Init();

    const int THREAD_COUNT = 8;
    const int ALLOC_COUNT = 10000;
    std::vector<std::thread> threads;

    for (int i = 0; i < THREAD_COUNT; ++i) {
        threads.emplace_back([]() {
            std::vector<void*> allocatedPointers;
            allocatedPointers.reserve(ALLOC_COUNT);

            for (int j = 0; j < ALLOC_COUNT; ++j) {
                // 여러 쓰레드에서 동시에 GameLogic(GeneralHeap)에 접근
                void* p = MemoryManager::Get().Allocate(16, MemoryTag::GameLogic);
                if (p) allocatedPointers.push_back(p);
            }

            for (void* p : allocatedPointers) {
                MemoryManager::Get().Free(p, MemoryTag::GameLogic);
            }
        });
    }

    for (auto& t : threads) {
        t.join();
    }

    // 결과: ThreadSafeProxy가 없다면 여기서 레이스 컨디션으로 인해 크래시가 나거나
    // 할당자 내부 포인터가 깨져서 값이 엉망이 되었을 것.
    std::cout << "All threads finished safely!" << std::endl;
    MemoryManager::Get().Shutdown();
}