# CoreMemory라는 이름의 라이브러리 정의
add_library(CoreMemory
    # 여기에 소스 추가
    StackAllocator.cpp
    PoolAllocator.cpp
    FreeListAllocator.cpp
    LinearAllocator.cpp
    MemoryManager.cpp
    ProxyAllocator.cpp
    FallbackAllocator.cpp
    ArenaAllocator.cpp
)

# 별칭 설정
add_library(Core::Memory ALIAS CoreMemory)

# Utils 연결
target_link_libraries(CoreMemory PRIVATE Core::Utils)

# 헤더 파일의 위치를 저장 (다른 타겟이 이 라이브러리를 사용할 때 필요)
target_include_directories(CoreMemory PUBLIC
    $<INSTALL_INTERFACE:include>
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}> # 빌드 시 자신의 위치를 포함    
)

# C++17 표준 재확인
target_compile_features(CoreMemory PUBLIC cxx_std_17)