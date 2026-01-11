#pragma once

#include <cstddef> // sizeof 연산 결과를 위한 size_t
#include <type_traits> // is_polymorhpic, is_standard_layout 등 타입 속성 검사용

// ECS 컴포넌트의 캐시 정렬 및 데이터 순수성을 강제하는 매크로
#define ENFORCE_COMPONENT_RULES(ComponentType) \
    /* 1. 크기 제한: 64바이트(캐시 라인)를 초과할 수 없음 */ \
    static_assert(sizeof(ComponentType) <= 64, \
                    "ERROR: Component '" #ComponentType "' size exceeds 64 bytes. Consider data optimization."); \
    \
    /* 2. 가상 함수 사용 금지: vptr 오버헤드를 막고 순수 데이터임을 강제 */ \
    static_assert(!std::is_polymorphic<ComponentType>::value, \
                    "ERROR: Component '" #ComponentType "' must be pure data (no virtual functions)."); \
    \
    /* 3. 메모리 레이아웃: C 스타일의 Plain Old Data (POD)를 강제함 */ \
    /* (옵션: 컴파일러가 임의로 구조를 변경하지 못하도록 보장) */ \
    static_assert(std::is_standard_layout<ComponentType>::value, \
                    "ERROR: Component '" #ComponentType"' must have a standard memory layout."); 