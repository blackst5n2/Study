#pragma once
#include "Threading/Common.h"

namespace Core {
class AppInstance {
public:
    static AppInstance& Get() {
        static AppInstance instance;
        return instance;
    }

    // 서버의 모든 구성 요소를 순서대로 기상
    bool Init();

    // 메인 루프 실행
    void Run();

    // 안전한 종료 프로세스
    void Shutdown();

private:
    AppInstance() = default;
    ~AppInstance() = default;

    bool m_isInitialized = false;
};

}