Registry는 하드웨어 친화적 구조를 하나로 묶어주는 컨테이너.

# 레지스트리의 핵심 책임 (Core Responsibilities)
#### Entity ID 관리 (Entity Manager)
- 책임: 새로운 Entity ID를 발급하고, 사용이 끝난 ID를 회수하여 재활용함 (Entity Pooling).
- 하드웨어 배경: ID 발급/재활용 과정에서 Generation ID를 관리하여, 오래된 포인터(ID)가 새 Entity를 잘못 가리키는 문제를 방지해야 함 (유효성 검사 $O(1)$).
- 내부 구조: 엔티티 ID 풀링을 위한 큐나 스택 구조를 가짐.

#### Component 데이터 저장 및 매핑 (Component Manager)
- 책임: 모든 컴포넌트 타입별 Sparse Set 인스턴스를 관리하고, Entity ID와 컴포넌트 데이터를 연결함.
- 하드웨어 배경: 캐시 효율을 위해 모든 컴포넌트 데이터가 **타입별로 연속된 메모리** 에 저장되록 보장함.
- 구현 시: `std::unordered_map<ComponentTypeID, ComponentPool*>`과 같은 형태로, 컴포넌트 ID(비트마스크의 인덱스)를 키로 사용하여 해당 타입의 Sparse Set 컨테이너에 접근하는 구조를 가짐.

#### System 실행 순서 및 스케줄링 (System Manager)
- 책임: 등록된 시스템들을 관리하고, 적절한 순서로 루프를 돌며 실행하거나, 병렬 실행을 위해 워커 스레드 풀에 작업을 분배함.
- 하드웨어 배경: 시스템 간의 **데이터 경쟁(Race Condition)** 을 막기 위해, 시스템이 Read/Write 하는 컴포넌트 시그니처를 분석하여 안전한 병렬 실행 스케줄을 짜야 함. (이것이 복잡해지면 Unity DOTS의 Job System처럼 발전함.)
- 구현 시: 시스템 실행 순서를 정의하는 `std::vector<SystemBase*>`와 같은 컨테이너를 가짐.

#### 쿼리 및 필터링 캐싱 (Query Cache)
- 책임: 시스템이 요청하는 특정 컴포넌트 조합(`Position`과 `Velocity`)을 가진 엔티티들의 목록을 미리 계산하여 캐싱함.
- 하드웨어 배경: 시스템이 매 프레임마다 수많은 엔티티의 비트마스크를 다시 계산하는 것은 비효율적임. 엔티티의 컴포넌트 구성이 변경되지 않는 한, **쿼리 결과** 는 캐싱되어야 함.
- 핵심: 어떤 엔티티에 컴포넌트가 추가/제거될 때만 (Entity Mutation), 관련 있는 모든 쿼리 캐시를 **무효화(Invalidate)** 하고 다시 계산함.

# 레지스트리의 구현 설계
Registry는 수많은 기능을 하나로 통합하는 단일 클래스로 구현되지만, TDD를 위해 그 기능을 논리적으로 모듈화하여 관리하는 것이 좋음.

| 모듈 이름                     | 핵심 기능 (TDD 테스트 대상)                                                          | 사용 데이터 구조                          |
| ------------------------- | --------------------------------------------------------------------------- | ---------------------------------- |
| `Registry::Entity` API    | `CreateEntity()`,<br>`DestroyEntity()`<br>`IsAlive(id)`                     | ID Pool (Queue)m, Generation Array |
| `Registry::Component` API | `AddComponent<T>(id`,<br>`RemoveComponent<T>(id)`,<br>`GetComponent<T>(id)` | `std::unordered_map` -> Sparse Set |
| `Registry::System` API    | `AddSystem<T>(id)`,<br>`UpdateSystems(dt)`                                  | `std::vector<System>`, 쿼리 캐시 맵     |

#### Registry가 핵심인 이유
Registry의 존재는 OOP와의 근본적인 차이를 만듦.
- OOP: 객체 자신이 `Player.Update()`를 호출하며 자신의 데이터를 스스로 처리함.
- ECS (Registry 중심):
	1. Registry가 모든 데이터를 **모두 한 곳에** 연속적으로 모아두고 (Component Manager).
	2. Registry가 모든 로직(System)을 **순서대로 또는 병렬로** 실행하며 (System Manager).
	3. System은 Registry에게 "이런 데이터를 가진 애들 목록 줘!"라고 요청함 (Query Cache).

즉, 게임 월드의 상태 변화와 흐름이 **Registry라는 중앙 집중식 관리자** 를 통해 이루어지며, 이를 통해 데이터 지향 설계의 장점(연속된 메모리, 병렬 처리 용이성)이 극대화 됨.
