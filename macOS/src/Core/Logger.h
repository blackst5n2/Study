#pragma once
#include <iostream>
#include <mutex>
#include <string>
#include <sstream>

namespace Core {

class Logger {
public:
    // 재귀 템플릿을 이용한 로그 합성
    template<typename T>
    static void BuildMessage(std::ostringstream& oss, T value) {
        oss << value;
    }

    template<typename T, typename... Args>
    static void BuildMessage(std::ostringstream& oss, T first, Args... args) {
        oss << first;
        BuildMessage(oss, args...);
    }

    template<typename... Args>
    static void Log(const std::string& level, Args... args) {
        std::ostringstream oss;
        oss << "[" << level << "] ";
        BuildMessage(oss, args...);

        static std::mutex s_mutex;
        std::lock_guard<std::mutex> lock(s_mutex);
        std::cout << oss.str() << std::endl;
    }
};

}

// 매크로 등록: LOG_INFO("Port: ", 8080);
#define LOG_INFO(...) Core::Logger::Log("Info", __VA_ARGS__)
#define LOG_ERR(...) Core::Logger::Log("Error", __VA_ARGS__)