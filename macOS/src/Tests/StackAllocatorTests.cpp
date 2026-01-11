#include <gtest/gtest.h>
#include "../Core/Memory/StackAllocator.h"

namespace Core
{
    //테스트 픽스쳐 설정
    class StackAllocatorTest : public::testing::Test {
    protected:
        // 200바이트 크기의 스택 영역을 생성 (8바이트 기본 정렬 가정)
        const size_t TEST_SIZE = 200;
        
        void SetUp() override {

        }

        void TearDown() override {

        }
    };
}

using namespace Core;

// 테스트 1: 기본 할당, 정렬, 해제 검증
TEST_F(StackAllocatorTest, AllocationRespectsAlignment) {
    std::vector<uint8_t> buffer(TEST_SIZE);
    StackAllocator allocator(buffer.data(), TEST_SIZE);

    const size_t ALIGNMENT = 8;
    const size_t NUM_ALLOCATIONS = 15;

    // 할당 전 마커 저장 
    uintptr_t initalAddress = allocator.getMarker();

    // 다양한 크기를 8바이트 정렬로 할당
    for (size_t i = 1; i <= NUM_ALLOCATIONS; ++i) {
        size_t size = i; // 1바이트 부터 15바이트까지
        void* p = allocator.allocate(size, ALIGNMENT);

        // 1. 할당 성공 단정
        ASSERT_NE(p, nullptr) << "Allocation of size " << size << " failed.";

        // 2. 정렬 단정: 주소 % 8 == 0 이어야 함
        ASSERT_EQ((uintptr_t)p % ALIGNMENT, 0) << "Allocation for size " << size << " failed alignment.";
    }
}

// 테스트2: Mark and Release (Swap) 핵심 기능 검증
TEST_F(StackAllocatorTest, MarkAndReleaseFunctionality) {
    std::vector<uint8_t> buffer(TEST_SIZE);
    StackAllocator allocator(buffer.data(), TEST_SIZE);

    // 1. 초기 할당: 10바이트 할당 (마커 이전)
    void* p1 = allocator.allocate(10);

    // 2. 마커 저장: 이 지점 이전으로 되돌릴 것
    StackAllocator::Marker marker = allocator.getMarker();
    uintptr_t markerAddress = (uintptr_t)marker; // 주소값으로 저장하여 비교

    // 3. 임시 할당: 20바이트와 30바이트 할당 (릴리즈 대상)
    void* p2 = allocator.allocate(20);
    void* p3 = allocator.allocate(30);

    // 4. 단정 (Before Release): 사용된 메모리 크기 검증
    // (10바이트 + 20바이트 + 30바이트)와 정렬 패딩을 고려하여 계산
    size_t totalUsedBeforeRelease = allocator.getCurrentUsedSize();
    ASSERT_GT(totalUsedBeforeRelease, 60);

    // 5. Act (Release): 마커 지점으로 되돌림. (O(1) 해제)
    allocator.release(marker);

    // 6. 단정 (After Release): 포인터와 사용량이 정확히 복구되었는지 확인
    ASSERT_EQ((uintptr_t)allocator.getMarker(), markerAddress)
        << "Stack Pointer did not return precisely to the marker address.";

    // 7. 재사용성 단정: 해제된 공간이 재사용 가능한지 확인 (큰 블록 할당 시도)
    void* p4 = allocator.allocate(100);
    ASSERT_NE(p4, nullptr) << "Release space is not available for reuse.";

    // p4의 시작 주소가 이전에 p2가 시작했던 주소와 같아야 함.
    ASSERT_EQ(p4, p2) << "Reused slot should start where the released block begin.";
}

// 테스트 3: 스택오버플로우 및 경계 조건 확인
TEST_F(StackAllocatorTest, HandlesStackOverflow) {
    std::vector<uint8_t> buffer(TEST_SIZE);
    StackAllocator allocator(buffer.data(), TEST_SIZE);

    // 1. 현재 사용 가능한 잔여 공간을 계산하여 거의 다 채움
    size_t currentUsed = allocator.getCurrentUsedSize();
    size_t remainingSize = TEST_SIZE - currentUsed;

    // 2. 남은 공간보다 1바이트 더 크게 할당을 시도
    void* pFail = allocator.allocate(remainingSize + 1); 
    
    // 3. Assert: nullptr이 반환되어야 함
    ASSERT_EQ(pFail, nullptr) << "Stack Allocator must return nullptr on overflow.";
    
    // 4. Assert: 포인터가 실패했음에도 움직이지 않았는지 함
    ASSERT_EQ(allocator.getCurrentUsedSize(), currentUsed) 
        << "Allocator state changed despite failed allocation.";
    
    // 5. Assert: 남은 공간을 정확히 할당하는 것은 성공해야 함
    void* pSuccess = allocator.allocate(remainingSize); 
    ASSERT_NE(pSuccess, nullptr) << "Exactly remaining size allocation should succeed.";
}

// 테스트 4: 무효한 해제 검증
TEST_F(StackAllocatorTest, InvalidMarkerCausesAssert) {
    std::vector<uint8_t> buffer(TEST_SIZE);
    StackAllocator allocator(buffer.data(), TEST_SIZE);
    
    // 1. Act 정상 할당
    allocator.allocate(32);

    // 2. Act: 유효하지 않은 (시작 주소보다 작은) 마커 생성
    StackAllocator::Marker invalidMarker = (uintptr_t)allocator.getStartPtr() - 1;
}
