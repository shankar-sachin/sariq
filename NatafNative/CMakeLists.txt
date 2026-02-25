cmake_minimum_required(VERSION 3.15)
project(NatafNative LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Build as shared library for P/Invoke from C#
add_library(NatafNative SHARED
    NatafNative.cpp
    NatafNative.h
)

# Platform-specific settings
if(WIN32)
    target_compile_definitions(NatafNative PRIVATE _WIN32)
    target_link_libraries(NatafNative PRIVATE kernel32)
endif()

# Set output directory
set_target_properties(NatafNative PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin"
    LIBRARY_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin"
)

# Install
install(TARGETS NatafNative
    RUNTIME DESTINATION bin
    LIBRARY DESTINATION lib
)
