#include <gtest/gtest.h>
#include <thread>
#include <vector>
#include <atomic>
#include "../Core/Threading/LocalArena.h"
#include "../Core/Memory/Memory.h"
#include "../Core/Memory/MemoryManager.h"
#include "../Core/Threading/ThreadManager.h"


using namespace Core;

class MemoryStressTests : public ::testing::Test {
protected: 
    void SetUp() override {
        MemoryManager::Get().Init();
    }

    void TearDown() override {
        ThreadManager::GetInstance().Shutdown();
        MemoryManager::Get().Shutdown();
    }
};

TEST_F(MemoryStressTests, RemoteFreeAndReclaimTest) {
    std::atomic<void*> sharedPtr{nullptr};
    std::atomic<bool> producerDone{false};

    // [Thread 1] 할당자
    std::thread producer([&]() {
        ThreadManager::GetInstance().InitThread();

        // 1. 메모리 할당
        void* ptr = Alloc(128);
        ASSERT_NE(ptr, nullptr);

        // 헤더 매직 확인
        ThreadBlockHeader* header = reinterpret_cast<ThreadBlockHeader*>((uint8_t*)ptr - sizeof(ThreadBlockHeader));
        EXPECT_EQ(header->ownerId, GetCurrentThreadId());
        EXPECT_EQ(header->magic, 0xDEADBEEF);

        // 2. 소비자 스레드에게 전달
        sharedPtr.store(ptr);

        // 3. 소비자가 해제할 때까지 대기
        while (sharedPtr.load() != nullptr) {std::this_thread::yield();}

        // 4. 다시 할당 시도 (Flush Remote 작동)
        // 이전 메모리가 리모트 큐로 들어와서 재사용되는지 확인
        void* newPtr = Alloc(128);
        EXPECT_NE(newPtr, nullptr);

        Free(newPtr);
        producerDone = true;
    });

    // [Thread 2] 해제자
    std::thread consumer([&]() {
        ThreadManager::GetInstance().InitThread();

        // 1. 생산자가 물건을 줄 때까지 대기
        void* ptr = nullptr;
        while ((ptr = sharedPtr.load()) == nullptr) {std::this_thread::yield();}

        // 2. 남의 메모리를 해제 (Push Remote)
        // Thread 1의 Arena->m_remoteQueue에 쌓임
        Free(ptr);

        // 3. 처리 신호 완료
        sharedPtr.store(nullptr);
    });

    producer.join();
    consumer.join();
    EXPECT_TRUE(producerDone);
}

TEST_F(MemoryStressTests, MultiThreadExchangeTest) {
    const int THREAD_COUNT = 4;
    const int ITERATIONS = 1000;
    std::vector<std::thread> workers;
    std::atomic<int> activeThreads{0};

    // 공유 자원: 각 스레드가 할당한 메모리를 다른 스레드가 가져가서 해제함
    std::vector<std::atomic<void*>> exchangeBox(THREAD_COUNT);
    for (auto& b : exchangeBox) b.store(nullptr);

    for (int i = 0; i < THREAD_COUNT; ++i) {
        workers.emplace_back([&, i]() {
            ThreadManager::GetInstance().InitThread();
            ++activeThreads;

            for (int step = 0; step < ITERATIONS; ++step) {
                // 1. 할당하여 상자에 넣기
                void* myAlloc = Alloc(64);
                exchangeBox[i].store(myAlloc);

                // 2. 옆 스레드의 상자 확인하여 있으면 해제하기
                int targetIdx = (i + 1) % THREAD_COUNT;
                void* othersAlloc = exchangeBox[targetIdx].exchange(nullptr);
                if (othersAlloc) {
                    Free(othersAlloc);
                }

                std::this_thread::yield();
            }
        });
    }

    for (auto& t : workers) t.join();

    // 잔여 메모리 정리 (Flush) 확인을 위해 각 아레나 강제 정리
    ThreadManager::GetInstance().Shutdown();
}