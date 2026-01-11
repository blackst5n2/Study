#include "../Core/Memory/FreeListAllocator.h"
#include <gtest/gtest.h>
#include <vector>

using namespace Core;

class FreeListAllocatorTests : public ::testing::Test {
protected:
    // 전체 메모리 크기 (예: 1MB)
    const size_t TEST_SIZE = 1024 * 1024;
};

// 가장 중요한 테스트: 인접한 빈 블록들이 해제 시 병합되는지 확인
TEST_F(FreeListAllocatorTests, BlockCoalescingWorksCorrectly) {
    std::vector<uint8_t> buffer(TEST_SIZE);

    FreeListAllocator allocator(buffer.data(), TEST_SIZE);

    // 1. 블록 A, B, C을 순차적으로 할당함
    size_t sizeA = 100;
    size_t sizeB = 200;
    size_t sizeC = 300;

    void* pA = allocator.allocate(sizeA);
    void* pB = allocator.allocate(sizeB);
    void* pC = allocator.allocate(sizeC);

    // 할당 성공 확인
    ASSERT_NE(pA, nullptr);
    ASSERT_NE(pB, nullptr);
    ASSERT_NE(pC, nullptr);

    // 2. B 블록을 먼저 해제. (Free List에 B가 들어감)
    allocator.deallocate(pB);

    // 3. A 블록을 해제함
    // A 블록은 바로 뒤에 있는 빈 블록 B와 병합되어야 함
    allocator.deallocate(pA);

    // 4. (선택적) C 블록을 해제함.
    // 이제 A+B 블록은 C와 병함되어야 하며, 전체 메모리가 하나의 큰 블록으로 돌아가야 함
    allocator.deallocate(pC);

    // 5. 검증: 전체 메모리가 하나의 큰 빈 블록으로 병합되었는지 확인
    // 이는 Allocator가 초기화 직후의 상태 (총 사용량 0, 빈 블록 1개)로 돌아갔음을 의미

    // 이론적으로, 모든 블록이 해제되고 병합되면,
    // Allocator의 총 순수 할당량은 0이 되어야 함.
    // (메타데이터는 초기화 시점에 이미 사용된 공간으로 간주될 수 있으므로,
    // 여기서는 모든 블록이 해제된 후 '가장 큰 빈 블록'이 전체 크기와 동일한지 확인하는 것이 가장 정확.)
    ASSERT_EQ(allocator.getCurrentUsedSize(), 0)
        << "모든 블록 해제 후에도 순수 사용량이 0이 아님. 병합 실패 가능성";
    
    // 핵심 검증: 가장 큰 빈 블록의 크기가 거의 전체 크기와 동일해야 함
    // 이 검증을 위해 FreeListAllocator에 getLargestFreeBlockSize() 함수가 구현되어야 함
    // ASSERT_GE(allocator.getLargestFreeBlockSize(), TEST_SIZE - (메타데이터 오버헤드))
}

// 테스트 2: Best Fit 테스트
TEST_F(FreeListAllocatorTests, BestFitCorrectly) {
    std::vector<uint8_t> buffer(TEST_SIZE);

    FreeListAllocator allocator(buffer.data(), TEST_SIZE);
    
    // 블록 사이즈 정의
    size_t a = 500;
    size_t b = 400;
    size_t c = 300;
    size_t d = 200;
    size_t e = 100;

    void* pA = allocator.allocate(a);
    void* pB = allocator.allocate(b);
    void* pC = allocator.allocate(c);
    void* pD = allocator.allocate(d);
    void* pE = allocator.allocate(e);

    // 할당 성공 확인
    ASSERT_NE(pA, nullptr);
    ASSERT_NE(pB, nullptr);
    ASSERT_NE(pC, nullptr);
    ASSERT_NE(pD, nullptr);
    ASSERT_NE(pE, nullptr);

    BlockHeader** headerAPtrLocation = (BlockHeader**)((uintptr_t)pA - sizeof(BlockHeader*));

    BlockHeader* headerA = *headerAPtrLocation;

    BlockHeader** headerDPtrLOcation = (BlockHeader**)((uintptr_t)pD - sizeof(BlockHeader*));

    BlockHeader* headerD = *headerDPtrLOcation;

    // a[500] b[400] c[300] d[200] e[100]
    // a, d 해제
    // Free[500] b[400] c[300] Free[200] e[100]
    allocator.deallocate(pA);

    allocator.deallocate(pD);
    
    // 100 사이즈 재할당
    void* pNew = allocator.allocate(e);
    
    // 헤더 계산
    BlockHeader** headerNewPtrLocation = (BlockHeader**)((uintptr_t)pNew - sizeof(BlockHeader*));
    BlockHeader* headerNew = *headerNewPtrLocation;

    // Free[500]이 아닌 Free[200] 위치에 할당이 되어야 함
    // 1. pA 블록은 해제 상태인가?
    ASSERT_EQ(headerA->is_free, true) << "원래 pA 블록이 해제되어야 함.";
    // 2. 새 할당 블록은 pD 위치의 Header를 사용했는가? (Best Fit) 검증
    ASSERT_EQ(headerNew, headerD) << "Best Fit은 pD 블록을 사용해야 함.";

    ASSERT_EQ(headerD->is_free, false) << "pD 위치에 할당된 블록이 사용 중이 아님.";
}