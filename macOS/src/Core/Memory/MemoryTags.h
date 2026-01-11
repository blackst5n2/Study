#pragma once

namespace Core {

enum class MemoryTag {
    Default, // 기본 (보통 FreeList/OS 힙)
    GameLogic, // 일반적인 게임 로직
    Rendering, // 렌더링 관련 (잦은 할당/해제)
    Physics, // 물리 엔진 (고정 크기 객체 많음 -> Pool)
    Temporary, // 아주 잠깐 쓰고 버릴 것 (-> Stack)
    Component, // ECS 컴포넌트 (->Pool)
    Network,
    Session,
    Send,
    Job,
    Permanent // 프로그램 종료 시까지 유지될 것
};

}
