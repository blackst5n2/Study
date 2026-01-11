add_library(Core
    AppInstance.cpp
)

target_include_directories(Core PUBLIC
    $<INSTALL_INTERFACE:include>
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}>
)

target_compile_features(Core PUBLIC cxx_std_17)