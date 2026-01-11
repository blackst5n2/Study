# 실제 라이브러리 타겟 이름은 CoreUtils
add_library(CoreUtils
    MemoryUtils.cpp
)

# Core::Utils라는 별칭(ALIAS)을 정의하여 네임스페이스처럼 사용
add_library(Core::Utils ALIAS CoreUtils)

# 다른 라이브러리가 CoreUtils를 사용할 때 헤더를 찾을 수 있도록 경로 지정
target_include_directories(CoreUtils PUBLIC
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}>
)
target_compile_features(CoreUtils PUBLIC cxx_std_17)