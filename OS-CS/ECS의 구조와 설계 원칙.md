# ECS 아키텍처 청사진
ECS는 게임 월드를 **관계형 데이터베이스(RDBMS)** 처럼 다루는 방식이라고 생각하면 가장 이해가 빠름.
- Entity = Primary Key (ID)
- Component = Table의 Column (Data)
- System = SQL Query & Update (Logic)

## 엔티티 (Entity): "존재의 증명"
OOP에서 객체는 '데이터+메서드'의 덩어리였지만, ECS에서 엔티티는 아무것도 가지지 않음, 오직 식별자일 뿐.

#### 설계 원칙 & 구조
1. 정의: 엔티티는 단지 **정수형 ID (Integer ID)** 임. (`uint32_t` 또는 `uint64_t`)
2. 역할: 컴포넌트 데이터들에 접근하기 위한 Key 역할만 수행.
3. 구현 요구사항:
	- 유니크함: 월드 내에서 중복되지 않아야 함.
	- 재사용성 (Pooling): 엔티티가 삭제되면 그 ID는 재사용되어야함. (ID 고갈 방지)
		- 버전 관리 (Generation): 중요한 문제. ID `10`번 엔티티가 삭제되고, 새로운 엔티티가 ID `10`을 물려받았다고 가정. 이전에 `10`번을 가리키던 총알이 엉뚱한 새 엔티티를 추적하면 안 됨.
			- 따라서 엔티티 ID는 보통 ` [ Index (24bit) | Generation (8bit) ]` 형태로 비트를 쪼개서 구성.
		Entity는 단순한 ID 타입이어야 하며, 생성/삭제/유효성 검증(Alive check)이 가능해야 함.

## 컴포넌트 (Component): "순수한 데이터"
#### 설계 원칙 & 구조
1. 정의: 로직이 전혀 없는 POD(Plain Old Data) 구조체(Struct)임.
2. 역할: 데이터의 속성(Attribute)을 정의함. (예: `Position`, `Velocity`, `Health`)
3. 저장 방식 (핵심):
	- 각 컴포넌트 타입별로 **별도의 배열(Array)** 에 저장됨.
	- AoS (Array of Structures) 금지.
	- 무든 `Position` 컴포넌트는 `PositionArray`에, 모든 `Health`는 `HealthArray`에 밀집되어야 함.
#### 구현의 난관: "희소성 (Sparsity)"
모든 엔티티가 모든 컴포넌트를 갖지는 않음.
- Entity 1: `Position`, `Velocity`
- Entity 2: `Position`, `Name`
- Entity 100: `Position`
`positionArray`의 인덱스 100번 위치에 Entity 100의 데이터를 넣는다면(Direct Mapping), Entity 3~99가 비어있어 메모리 낭비가 심함.

이를 해결하기 위해 두 가지 주요 접근법이 있음. 구현 난이도와 특성이 다름.
1. Sparse Set (희소 집합) 방식: 구현이 쉽고 유연함.
2. Archetype (아키타입) 방식: (Unity DOTS 방식) 메모리가 더 조밀하지만 구현이 복잡하고 런타임에 컴포넌트 추가/삭제가 느림.

	컴포넌트는 로직 없이 데이터만 담아야 함. 컴포넌트 매니저는 엔티티 ID를 통해 해당 엔티티의 컴포넌트 데이터에 $O(1)$로 접근할 수 있어야 함.

## 시스템 (System): "데이터 변환기"
#### 설계 원칙 & 구조
1. 정의: 실제 게임 로직을 수행하는 함수들의 집합. 상태(State)를 가지지 않는 것이이상적임.
2. 작동 방식:
	- "나는 `Position`과 `Velocity`를 둘 다 가진 엔티티만 필요해"라고 **질의(Query)** 함.
	- 조건에 맞는 엔티티들의 데이터 배열(Pointer)를 받아옴.
	- 루프를 돌며 데이터를 처리함.
3. 데이터 접근:
	- `Entity` 객체에게 `Update()`를 요청하는 것이 아님.
	- `System`이 `Component Array`를 직접 순회함.
```Plaintext
// 논리적 흐름
For each (entity in World having [Position, Velocity]) {
	Velocity = VlocityCOmponent[entity];
	Position = PositionComponent[entity];
	
	Position += Velocity * dt; // 단순 연산
}
```
.
	 시스템은 특정 컴포넌트 조합을 가진 엔티티들을 필터링할 수 있어야 함. 시스템은 필터링된 데이터 배열을 순회하며 로직을 수행해야 함.

## 월드 (World) / 레지스트리 (Registry)
이 모든 것을 관리하는 관리자 클래스임. 보통 `Registry` 또는 `World`라고 부름.

역할
1. Entity 생성/파괴 관리: ID 발급소.
2. Component 저장소 관리: 각 컴포넌트 배열(Pool)들을 관리.
3. Query 처리: 시스템이 "A랑 B 가진 애들 다 줘" 하면 찾아주는 역할.

# 요약
구현해야 할 대상들의 역할
1. Entity: 그냥 ID. (Generation ID 기법 필요)
2. Component: 데이터 덩어리. (연속된 메모리 할당 필요)
3. Registry: ID와 데이터 배열을 연결해주는 관리자. (Sparse Set 등의 매핑 기술 필요)
4. System: 데이터를 쿼리해서 루프 도는 로직.
