#include <gtest/gtest.h>
#include "../Core/Memory/PoolAllocator.h"

namespace Core
{
    // 테스트 픽스처(Fixture): 여러 테스트 케이스에서 공통으로 사용되는 설정을 정의
    class PoolAllocatorTest : public ::testing::Test {
    protected:
        // 16바이트 슬롯 10개 (총 160 바이트 + 헤더 공간)를 워한 Allocator
        // 실제 구현 시 PoolAllocator 생성자 매개변수에 맞춰 수정
        const int SLOT_SIZE = 16;

        void SetUp() override {
            // 매 테스트 시작 전 초기화 (생성자에서 초기화가 잘 되었다면 생략 가능)
        }

        void TearDown() override {
            // 매 테스트 종료 후 정리 (PoolAllocator 소멸자가 메모리를 잘 해제하는지 검증 가능)
        }
    };
}

using namespace Core;

// 테스트 1: O(1) 할당 및 해제 로직 검증 (Free List 순환)
TEST_F(PoolAllocatorTest, AllocationAndRecyclingIs01) {
    std::vector<uint8_t> buffer(SLOT_SIZE*10);
    PoolAllocator allocator(buffer.data(), SLOT_SIZE*10, SLOT_SIZE, 8);
    
    void* slots[10];

    // 1. Act (실행): 모든 슬롯 확인 및 사용량 검증
    for (int i = 0; i < 10; ++i) {
        slots[i] = allocator.allocate(10);
        ASSERT_NE(slots[i], nullptr) << "슬롯 #" << i << " 할당 실패";
    }
    // PoolAllocator가 사용하는 통계 함수가 있다면 검증
    ASSERT_EQ(allocator.getNumUsedSlots(), 10) << "사용 슬롯 개수가 일치하지 않음";

    // 2. Act: 중간 슬롯 (5번쨰) 해제 -> 이 주소가 Free List의 새로운 헤드가 되어야 함
    void* slotToRecycle = slots[4];
    allocator.deallocate(slotToRecycle);

    // 3. Assert (단정): 새로운 할당 요청 시, 해제된 슬롯이 O(1)로 재활용되어야 함
    void* recycledSlot = allocator.allocate(10);

    ASSERT_EQ(recycledSlot, slotToRecycle) << "해제된 슬롯이 Free List Head로 재활용되지 않았음.";
}

// 테스트 2: 정렬 (Alignment) 검증
TEST_F(PoolAllocatorTest, AllSlotsAreCorrectlyAligned) {
    std::vector<uint8_t> buffer(SLOT_SIZE*10);
    PoolAllocator allocator(buffer.data(), SLOT_SIZE*10, SLOT_SIZE, 8);

    void* slots[10];
    const size_t ALIGNMENT = 8; // PoolAllocator 생성 시 8바이트 정렬을 가정

    for (int i = 0; i < 10; ++i) {
        slots[i] = allocator.allocate(10);
        ASSERT_NE(slots[i], nullptr);

        // 주소값을 정수로 변환하여 정렬(alignment)을 검증
        // 주소 % alignemnt == 0 이여야 함
        ASSERT_EQ((uintptr_t)slots[i] % ALIGNMENT, 0)
            << "슬롯 #" << i << "의 주소가" << ALIGNMENT << "바이트 정렬에 실패했습니다."; 
    }
}