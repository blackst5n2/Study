#include "MemoryManager.h"
#include <gtest/gtest.h>

namespace Core {

class MemoryManagerTests : public ::testing::Test {
protected:
    void SetUp() override {
        MemoryManager::Get().Init();
    }

    void TearDown() override {
        MemoryManager::Get().Shutdown();
    }
};

}

using namespace Core;

TEST_F(MemoryManagerTests, ComplexAllocationWorkflow) {
    auto& mm = MemoryManager::Get();

    // 1. 다양한 태그로 메모리 할당
    void* pLog = mm.Allocate(1024, MemoryTag::GameLogic); // FreeList 예상
    void* pTemp = mm.Allocate(256, MemoryTag::Temporary); // Stack 예상
    void* pComp = mm.Allocate(64, MemoryTag::Component); // Pool 예상

    // 할당 성공 검증
    ASSERT_NE(pLog, nullptr);
    ASSERT_NE(pTemp, nullptr);
    ASSERT_NE(pComp, nullptr);

    // 2. 데이터 쓰기 (실제 메모리 접근 가능 여부 확인)
    memset(pLog, 0xAA, 1024);
    memset(pTemp, 0xBB, 256);
    memset(pComp, 0xCC, 64);

    // 3. Pool 할당자 사이즈 제약 테스트 (64바이트 슬롯인 경우)
    void* pTooBig = mm.Allocate(128, MemoryTag::Component);
    EXPECT_EQ(pTooBig, nullptr) << "Pool 슬롯보다 큰 요청은 거절되어야 함";

    // 4. 해제 시 태그 명시 (핵심 정책)
    mm.Free(pLog, MemoryTag::GameLogic);
    mm.Free(pTemp, MemoryTag::Temporary);
    mm.Free(pComp, MemoryTag::Component);

    // 주의: 잘못된 태그로 해제 시도 시
    // 할당자 내부 로직에 따라 에러가 나거나 무시됨
}

TEST_F(MemoryManagerTests, StackRestViaTag) {
    auto& mm = MemoryManager::Get();

    // Stack 할당자는 보통 프레임이 끝나면 통째로 리셋됨
    void* p1 = mm.Allocate(100, MemoryTag::Temporary);
    void* p2 = mm.Allocate(100, MemoryTag::Temporary);

    // 현재 구조에서는 개별 해제 의미 없음
    // 나중에 mm.Reset(MemoryTag::Temporary) 같은 기능 추가 가능
    mm.Free(p1, MemoryTag::Temporary);
    mm.Free(p2, MemoryTag::Temporary);
}