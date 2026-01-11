# 프로젝트 이름 정의
cmake_minimum_required(VERSION 3.10.0)
project(pf VERSION 0.1.0 LANGUAGES C CXX)

# C++ 표준 설정
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# FlatBuffers 패키지 찾기
find_package(FlatBuffers CONFIG REQUIRED)
if(NOT FlatBuffers_FOUND)
    # 패키지를 못 찾을 경우를 대비한 하드코딩 경로 (Mac 전용)
    set(FlatBuffers_INCLUDE_DIR "/opt/homebrew/include")
endif()

include_directories(SYSTEM "/opt/homebrew/include")
include_directories("${CMAKE_CURRENT_SOURCE_DIR}/src/Core/Network/Gen")

# .fbs 파일 경로와 출력 경로 설정
set(FBS_SCHEMA "${CMAKE_CURRENT_SOURCE_DIR}/Protocols/Protocol.fbs")
set(FBS_OUT_DIR "${CMAKE_CURRENT_SOURCE_DIR}/src/Core/Network/Gen")

# 출력 디렉토리 생성
file(MAKE_DIRECTORY ${FBS_OUT_DIR})

# flatc를 이용한 자동 생성 명령 정의
add_custom_command(
    OUTPUT "${FBS_OUT_DIR}/Protocol_generated.h"
    COMMAND flatbuffers::flatc --cpp --gen-object-api -o "${FBS_OUT_DIR}" "${FBS_SCHEMA}"
    DEPENDS "${FBS_SCHEMA}"
    COMMENT "Generating FlatBuffers header from ${FBS_SCHEMA}"
)

add_custom_target(PacketGen DEPENDS "${FBS_OUT_DIR}/Protocol_generated.h")

# python script 자동 생성
add_custom_command(
    OUTPUT "${CMAKE_SOURCE_DIR}/src/Core/Network/PacketList.h"
    COMMAND python3 ${CMAKE_SOURCE_DIR}/Scripts/gen_packets.py
    DEPENDS "${CMAKE_SOURCE_DIR}/Protocols/Protocol.fbs"
    COMMENT "Generating PacketList.h from Porotocol.fbs"
)

add_custom_target(GenPacket DEPENDS "${CMAKE_SOURCE_DIR}/src/Core/Network/PacketList.h")

set(PY_OUT_DIR "${CMAKE_SOURCE_DIR}/../Client")

# 출력 디렉토리 생성
file(MAKE_DIRECTORY ${PY_OUT_DIR})

# flatc를 이용한 Python 코드 자동 생성 명령 추가
add_custom_command(
    OUTPUT "${PY_OUT_DIR}/Protocol/GamePacket.py" # 대표적인 생성 파일 하나를 지정
    COMMAND flatbuffers::flatc --python -o "${PY_OUT_DIR}" "${FBS_SCHEMA}"
    DEPENDS "${FBS_SCHEMA}"
    COMMENT "Generating Python FlatBuffers code for Client"
)

# 기존 타겟에 Python 생성 파일도 의존성으로 추가
add_custom_target(ClientPacketGen DEPENDS "${PY_OUT_DIR}/Protocol/GamePacket.py")

# CTest 활성화
enable_testing()

# Google Test
include(FetchContent)

# gtest를 GitHub에서 가져와 프로젝트에 통합
FetchContent_Declare(
    googletest
    GIT_REPOSITORY https://github.com/google/googletest.git
    GIT_TAG        v1.14.0# 안정적인 태그 사용
)
FetchContent_MakeAvailable(googletest)
# gtest는 이제 'GTest::gtest_main' 타겟으로 사용 가능

# sub-dir 포함
add_subdirectory(src/Core) # 코어 라이브러리
add_subdirectory(src/Core/Utils) # 유틸 라이브러리
add_subdirectory(src/Core/Memory) # 메모리 라이브러리 정의
add_subdirectory(src/Core/Network) # 네트워크 라이브러리 정의
add_subdirectory(src/Core/Threading) # 스레드 라이브러리 정의
add_subdirectory(src/Core/ECS)
add_subdirectory(src/Tests) # 테스트 실행 파일 정의

# 기본적으로 통계 비활성화 (링리즈 모드)
if (NOT CMAKE_BUILD_TYPE STREQUAL "Release")
    # Debug 또는 RelWithDebInfo 모드에서는 통계 활성화
    target_compile_definitions(CoreMemory PRIVATE ENABLE_ALLOCATOR_STATS)
endif()

if (NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE RelWithDebInfo)
endif()

add_executable(pf main.cpp)

if(APPLE)
    add_custom_command(TARGET pf POST_BUILD
        COMMAND codesign -s - --entitlements "${CMAKE_CURRENT_SOURCE_DIR}/debug.entitlements" --force $<TARGET_FILE:pf>
        COMMENT "Signing pf with get-task-allow for Instruments..."
    )
endif()

if (APPLE)
    # 컴파일 시 디버그 정보 포함
    add_compile_options(-g)

    # 2. Xcode Generator용
    set(CMAKE_XCODE_ATTRIBUTE_DEBUG_INFORMATION_FORMAT "dwarf-with-dsym")

    # 3. Ninja/Make 환경에서 .dSYM 생성
    add_custom_command(TARGET pf POST_BUILD
        COMMAND dsymutil $<TARGET_FILE:pf>
        COMMENT "Extracting debug symbols (dSYM) for pf..."
    )
endif()

add_dependencies(pf PacketGen)
add_dependencies(pf GenPacket)
add_dependencies(pf ClientPacketGen)

target_include_directories(pf PRIVATE
    ${CMAKE_CURRENT_SOURCE_DIR}
    src/Core
    src/Core/Network
    src/Core/Memory
    src/Core/Utils
    src/Core/Threading
    src/Core/ECS
    ${FlatBuffers_INCLUDE_DIR}
    ${FLATBUFFERS_INCLUDE_DIR}
)

target_link_libraries(pf PRIVATE
    Core
    CoreNetwork
    CoreThreading
    CoreMemory
    CoreUtils
    CoreECS
    flatbuffers::flatbuffers
)

add_executable(dummy_client dummy_client.cpp)

# 서버에서 사용하는 라이브러리와 FlatBuffers 연결
target_link_libraries(dummy_client PRIVATE
    CoreNetwork
    CoreMemory
    flatbuffers::flatbuffers
)

target_include_directories(dummy_client PRIVATE
    ${CMAKE_CURRENT_SOURCE_DIR}/src/Core/Network/Gen
    ${FlatBuffers_INCLUDE_DIR}
)

# 빌드 대상에 포함
target_sources(pf PRIVATE "${FBS_OUT_DIR}/Protocol_generated.h")
