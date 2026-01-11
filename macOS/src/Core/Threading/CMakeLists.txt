add_library(CoreThreading
    LocalArena.cpp
    ThreadManager.cpp
    ThreadPool.cpp
    LinearArena.cpp
)

add_library(Core::Threading ALIAS CoreThreading)

#target_link_libraries(CoreThreading PRIVATE)

target_include_directories(CoreThreading PUBLIC
    $<INSTALL_INTERFACE:include>
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}>
)

target_compile_features(CoreThreading PUBLIC cxx_std_17)