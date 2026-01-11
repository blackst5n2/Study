add_library(CoreECS
    Registry.cpp
    ComponentStorage.cpp
    Entity.cpp
    Group.cpp
    SystemManager.cpp
    View.cpp
    CommandBuffer.cpp
)

add_library(Core::ECS ALIAS CoreECS)

target_include_directories(CoreECS PUBLIC
    $<INSTALL_INTERFACE:include>
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}>
)

target_compile_features(CoreECS PUBLIC cxx_std_17)