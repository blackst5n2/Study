**"GPIO 제어와 라이브러리 뜯어보기"**

**"LED 켜기"** 작업이, 실제 하드웨어와 C 코드 (라이브러리) 사이에서 어떤 일이 벌어지는지 확인하기.

# 하드웨어의 실체: GPIO 핀 뒤에는 무엇이 있을까?
MCU 칩 밖으로 튀어 나온 핀(Pin)은 단순한 금속 다리가 아님. 그 안에는 복잡한 회로가 숨어 있음.
- Mode Register (모드 레지스터): 이 핀을 입력(Input)으로 쓸지, 출력(Output)으로 쓸지 결정하는 스위치임.
- Output Data Register (ODR): 출력 모드일 때, 핀으로 `High(3.3V)`를 내보낼지 (`Low(0V)`)를  내보낼지 결정하는 메모리 방임.
- Input Data Register (IDR): 입력 모드일 때, 외부 전압이 높은지 낮은지를 저장해두는 방임.
코드로 LED를 켠다는 것은, 결국 **ODR이라는 특정 메모리 주소의 비트 하나를 `1`로 만드는 행위임.**

# 코드의 계층: 라이브러리 vs 레지스터
흔히 사용하는 `HAL_GPIO_WritePin` 같은 함수가 도대체 무슨 짓을 하는지 비교. (예시는 STM32 MCU 기준)

### Level 1. 우리가 작성하는 코드 (라이브러리 사용)
```c
// "GPIOA 포트의 5번 핀을 High(1)로 설정해라"
HAL_GPIO_WritePin(GPIOA, GPIO_PIN_5, GPIO_PIN_SET);
```

### Level 2. 라이브러리 내부 (함수 정의)
```c
void HAL_GPIO_WritePin(GPIO_TypeDef* GPIOx, uint16_t GPIO_Pin, GPIO_PinState PinState)
{
	if (PinState == GPIO_PIN_SET) {
		// GPIOx의 BSRR 레지스터에 핀 위치만큼 값을 쓴다.
		GPIOx->BSRR = GPIO_Pin;
	}
	else {
		// 끄는 동작
		GPIOx->BSRR = (uint32_t)GPIO_Pin << 16;
	}
}
```
- 분석: 라이브러리는 복잡한 주소 계산이나 비트 연산 실수를 막아주는 **"안전 포장지(Wrapper)"** 일 뿐임. 결국 `GPIOx->BSRR`이라는 변수에 값을 대입하고 있음.

### Level 3. 레지스터 직접 제어 (Bare-metal)
라이브러리를 걷어내면, CPU가 수행하는 진짜 명령은 이러함.
```c
//0x400200018 번지에 0x00000020 값을 써라!
*(volatile uint32_t *)(0x40020018) = 0x00000020;
```
이 한 줄이 실행되는 순간, 전자가 회로를 타고 흘러 LED가 켜짐.

# 데이터시트와 주소 계산: "0x40020018은 어디서 나왔나?"
이 주소를 알아내는 것이 **데이터시트(Datasheet)와 레퍼런스 매뉴얼(Reference Manual)** 을 읽는 능력임.
1. Memory Map 확인: 데이터시트에서 `GPIOA` 포트가 할당된 **기준 주소(Base Address)** 를 찾음.
	- `GPIOA Base = 0x4002 0000`
2. Register Offset 확인: `BSRR` (Bit Set/Reset Register) 레지스터가 기준 주소로부터 얼마나 떨어져 있는지 찾기.
	- `Address Offset = 0x18`
3. 최종 주소: `Base` + `Offset` = `0x4002 0018`

	결론: C언어의 구조체 포인터 `GPIOA->BSRR`은 컴파일러가 이 덧셈 연산을 대신 해주는 문법임.

# 디버깅 시뮬레이션: 눈으로 확인하기
### 브레이크포인트 설정
`HAL_GPIO_WritePin` 함수 호출 직전에 멈춤.

### SFR (Special Function Register) 뷰 또는 Memory 뷰 열기
디버거 창에서 주소 `0x40020018`을 직접 입력해서 확인.
- 실행 전: 값은 `0x00000000`임. LED는 꺼져 있음.

### 한 줄 실행 (Step Over)
코드를 한 줄 실행함
### 변화 확인
- 메모리 뷰: `0x40020018` 주소의 값이 빨간색으로 깜빡이며 `0x00000020`으로 바뀜.
- 실물 보드: 동시에 책상 위에 있는 보드의 LED가 번쩍 하고 ㅕ짐.

이 순간이 바로 소프트웨어(코드)가 물리세계(하드웨어)를 제어하는 것을 목격하는 순간.

# 요약
- 모든 라이브러리 함수는 결국 **특정 메모리 주소(레지스터)에 값을 쓰고 읽는 행위** 로 귀결.
- 라이브러리 사용법을 모르면, **함수 내부(Definition)** 을 열어보고 **어떤 레지스터를 건드리는지** 보면 됨. 그리고 데이터시트에서 그 레지스터의 역할을 찾아보면 역으로 사용법을 알 수 있음.

