#include "../Core/Threading/LocalArena.h"
#include "../Core/Threading/ThreadManager.h"
#include "../Core/Memory/Memory.h"
#include <gtest/gtest.h>

using namespace Core;

class LocalArenaTests : public ::testing::Test {
protected:
    
};

TEST_F(LocalArenaTests, LocalArenaAlignmentTest) {
    MemoryManager::Get().Init();
    ThreadManager::GetInstance().InitThread();
    
    void* ptr = Alloc(17, MemoryTag::Default, 8);
    
    EXPECT_NE(ptr, nullptr);

    uint8_t* raw = static_cast<uint8_t*>(ptr) - 16; // THeader 위치

    auto* tHeader = reinterpret_cast<Core::ThreadBlockHeader*>(raw);
    
    EXPECT_EQ(tHeader->magic, 0xDEADBEEF);

    Free(ptr);
}