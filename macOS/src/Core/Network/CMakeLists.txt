# 1. 소스 파일 목록에 PacketDispatcher.cpp 추가 확인
add_library(CoreNetwork STATIC
    Session.cpp
    SessionManager.cpp
    NetworkManager.cpp
    PacketHandlers.cpp
    PacketDispatcher.cpp
)

add_dependencies(CoreNetwork PacketGen)

add_library(Core::Network ALIAS CoreNetwork)

# 2. 인클루드 경로 최적화
target_include_directories(CoreNetwork PUBLIC
    "/opt/homebrew/include"
    "${CMAKE_CURRENT_SOURCE_DIR}"             # Session.h 등을 찾기 위함
    "${CMAKE_CURRENT_SOURCE_DIR}/Gen"         # Protocol_generated.h를 찾기 위함
)

# 3. 라이브러리 연결
target_link_libraries(CoreNetwork PUBLIC 
    CoreThreading
    flatbuffers::flatbuffers    
)

target_compile_features(CoreNetwork PUBLIC cxx_std_17)