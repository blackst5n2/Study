#include "../Core/Memory/FallbackAllocator.h"
#include "../Core/Memory/LinearAllocator.h"
#include <gtest/gtest.h>
#include <vector>

using namespace Core;

class FallbackAllocatorTests : public ::testing::Test {
protected:
    //테스트용 메모리 사이즈 정의
    const size_t PRIMARY_SIZE = 128;
    const size_t FALLBACK_SIZE = 1024;
};

TEST_F(FallbackAllocatorTests, FullWorkflowTest) {
    // 1. Primary(100바이트), Fallback(1000바이트) 준비
    std::vector<uint8_t> pBuffer(PRIMARY_SIZE);
    std::vector<uint8_t> fBuffer(FALLBACK_SIZE);

    LinearAllocator primary(pBuffer.data(), PRIMARY_SIZE);
    LinearAllocator fallback(fBuffer.data(), FALLBACK_SIZE);

    // 2. FallbackAllocator 구성
    FallbackAllocator combined(primary, fallback);

    // --- 시나리오 1 ---
    size_t request1 = 32;
    void* p1 = combined.allocate(request1);

    ASSERT_NE(p1, nullptr);

    // 헤더 역추적
    FallbackAllocationHeader* h1 =(FallbackAllocationHeader*)((uintptr_t)p1 - sizeof(FallbackAllocationHeader));
    EXPECT_EQ(h1->allocator_type, AllocatorType::PRIMARY);
    std::cout << "[Test] p1 allocated from PRIMARY at: " << p1 << std::endl;

    // --- 시나리오 2 ---
    size_t request2 = 80;
    void* p2 = combined.allocate(request2);

    ASSERT_NE(p2, nullptr);

    // 헤더 역추적
    FallbackAllocationHeader* h2 =(FallbackAllocationHeader*)((uintptr_t)p2 - sizeof(FallbackAllocationHeader));
    EXPECT_EQ(h2->allocator_type, AllocatorType::FALLBACK);
    std::cout << "[Test] p1 allocated from PRIMARY at: " << p2 << std::endl;

    // --- 시나리오 3 ---
    void* pTooBig = combined.allocate(2000); // 전체 합 보다 큼.
    EXPECT_EQ(pTooBig, nullptr);

    // --- 시나리오 4 ---
    combined.deallocate(p1);
    combined.deallocate(p2);

    std::cout << "[Test] All deallocations completed without crash." << std::endl;
}

TEST_F (FallbackAllocatorTests, AlignmentPreservation) {
    std::vector<uint8_t> pBuffer(PRIMARY_SIZE);
    std::vector<uint8_t> fBuffer(FALLBACK_SIZE);

    LinearAllocator primary(pBuffer.data(), PRIMARY_SIZE);
    LinearAllocator fallback(fBuffer.data(), FALLBACK_SIZE);
    FallbackAllocator combined(primary, fallback);

    // 16바이트 정렬 요청
    void* p = combined.allocate(16, 16);

    // 주소가 16으로 나누어 떨어지는 지 확인
    EXPECT_EQ((uintptr_t)p % 16, 0);
}