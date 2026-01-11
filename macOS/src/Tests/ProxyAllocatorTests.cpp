#include "../Core/Memory/ProxyAllocator.h"
#include "../Core/Memory/LinearAllocator.h"
#include <gtest/gtest.h>
#include <cstring> // std::memset 사용

namespace Core {

class ProxyAllocatorTests : public ::testing::Test {
protected:
    const size_t TEST_SIZE = 1024;

    void SetUp() override {
    }

    void TearDown() override {

    }
};

}

using namespace Core;

TEST_F(ProxyAllocatorTests, BasicAllocationAndDeallocation) {
    std::vector<uint8_t> buffer(TEST_SIZE);
    LinearAllocator linearAllocator(buffer.data(), TEST_SIZE);
    ProxyAllocator proxyAllocator = ProxyAllocator(linearAllocator);

    // 1. 정상적인 할당 및 해제 시 GuardByte 변경 업슴
    size_t size = 100;
    void* p = proxyAllocator.allocate(size);
    ASSERT_NE(p, nullptr);

    // 데이터 쓰기 (Guard Bytes를 건드리지 않음)
    std::memset(p, 0xAA, size);

    // 해제 시 Guard 검증 (ASSERT 발생 없어야 함)
    // ASSERT 검증은 CTest 환경에서 테스트 프레임워크 자체의 ASSERT_DEATH를 사용해야 함
    // 간단히 deallocate를 호출하여 논리적으로 통과함을 확인
    proxyAllocator.deallocate(p);

    proxyAllocator.reset();
}

TEST_F(ProxyAllocatorTests, BufferOverflowDetection) {
    std::vector<uint8_t> buffer(TEST_SIZE);
    LinearAllocator linearAllocator(buffer.data(), TEST_SIZE);
    ProxyAllocator proxyAllocator = ProxyAllocator(linearAllocator);

    size_t size = 16;
    void* p = proxyAllocator.allocate(size);
    ASSERT_NE(p, nullptr);

    // 1. Buffer Overflow 발생: 요청 크기(16)보다 1바이트 더 쓰기
    // 이 1바이트가 Back Guard Bytes의 첫 바이트를 덮어씀.
    uint8_t* data = (uint8_t*)p;

    // 요청 크기까지 데이터 채우기
    std::memset(data, 0xCC, size);

    // 오버플로우 발생: Back Guard Bytes를 덮어씀
    *(data + size) = 0xFF;

    proxyAllocator.deallocate(p);

    FAIL() << "deallocate()는 Guard Bytes 오염으로 인해 ASSERT를 발생시켜야 했음";
}