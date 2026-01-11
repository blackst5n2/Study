#include "../Core/Memory/LinearAllocator.h"
#include <gtest/gtest.h>

namespace Core
{
class LinearAllocatorTests : public ::testing::Test {
protected:
    const size_t TEST_SIZE = 200;

    void SetUp() override {

    }
    
    void TearDown() override {

    }
};

}

using namespace Core;

TEST_F(LinearAllocatorTests, BasicAllocationAndReset) {
    std::vector<uint8_t> buffer(TEST_SIZE);

    LinearAllocator allocator(buffer.data(), TEST_SIZE);
    // 1. 순차 할당 검증
    size_t a = 10;
    size_t b = 20;
    size_t c = 30;
    size_t d = 40;

    void* pA = allocator.allocate(a);
    void* pB = allocator.allocate(b);
    void* pC = allocator.allocate(c);
    void* pD = allocator.allocate(d);

    ASSERT_NE(pA, nullptr);
    ASSERT_NE(pB, nullptr);
    ASSERT_NE(pC, nullptr);
    ASSERT_NE(pD, nullptr);

    // 시작 포인터 추출
    uintptr_t startPtr = (uintptr_t)pA;

    // 2. 경계 조건 검증
    size_t e = 150;
    
    void* pE = allocator.allocate(e);
    ASSERT_EQ(pE, nullptr) << "불가능한 크기 할당";

    // 3. Reset 기능 검증 (포인터가 시작점으로 돌아감)
    size_t usedBeforeReset = allocator.getCurrentUsedSize();
    ASSERT_NE(usedBeforeReset, 0) << "Reset 전에는 사용량이 0이 아니어야 함";

    allocator.reset();
    
    ASSERT_EQ(allocator.getCurrentUsedSize(), 0) << "Reset 후 사용량이 0이 되어야 함";

    void* pNew = allocator.allocate(e);
    ASSERT_NE(pNew, nullptr) << "Reset 후 재할당에 성공해야 함";

    // 시작 주소 검증
    ASSERT_EQ((uintptr_t)pNew, startPtr) << "Reset후 재할당에는 시작 주소가 같아야 함";
}