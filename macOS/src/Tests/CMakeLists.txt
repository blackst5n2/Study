# 테스트 소스 파일 목록 정의
set(TEST_SOURCES
    PoolAllocatorTests.cpp
    StackAllocatorTests.cpp
    FreeListAllocatorTests.cpp
    LinearAllocatorTests.cpp
    ProxyAllocatorTests.cpp
    FallbackAllocatorTests.cpp
    MemoryManagerTests.cpp
    ThreadSafetyTests.cpp
    LocalArenaTests.cpp
    MemoryStressTests.cpp
)

# 'MemoryTests' 테스트 실행 파일 정의
add_executable(CoreTests ${TEST_SOURCES})

# 테스트 실행 파일 빌드시 통계 기능 강제 활성화
target_compile_definitions(CoreTests PUBLIC ENABLE_ALLOCATOR_STATS)

# 의존성 링크: Core::Memory 라이브러리와 gtest 링크
target_link_libraries(CoreTests
    PRIVATE
        Core::Memory # 테스트할 대상 라이브러리
        Core::Network
        GTest::gtest_main # gtest 프레임워크와 main 함수를 자동으로 링크
        GTest::gmock
)

# 헤더 경로 추가: Core 라이브러리의 헤더를 찾을 수 있도록 설정
target_include_directories(CoreTests PRIVATE
    ${CMAKE_SOURCE_DIR}/src/Core/Memory
    ${CMAKE_SOURCE_DIR}/src/Core/Network
    ${CMAKE_SOURCE_DIR}/src/Core/Threading
)

# CTest에 Google Test 등록
# gtest의 테스트 코드를 분석하여 CTest에 자동으로 등록
include(GoogleTest)
gtest_discover_tests(CoreTests)