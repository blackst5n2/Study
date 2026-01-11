#### flatbuffers의 파싱의 3단계
```cpp
#include "game_packet_generated.h" // fbs로 생성된 헤더

void ProcessFlatbuffers(uint8_t* buffer_ptr, size_t size) {
	// 1. Root Table 가져오기
	// buffer_ptr을 헤더(8바이트)를 제외한 Faltbuffers 데이터의 시작점.
	auto game_packet = GetGamePacket(buffer_ptr);
	
	// 2. Union 타입 확인
	// Union 필드 이름이 'data'라면, 'data_type()' 메서드가 자동 생성됨
	auto packet_type = game_packet->data_type();
	
	// 3. 타입에 따른 분기 (Switch)
	switch (packet_type) {
		case GameData_MovePacket: {
			// as_MovePacket()을 통해 실제 테이블로 캐스팅 (추가 복사 없음)
			auto move_ptr = game_packet->data_as_MovePacket();
			float x = move_ptr->x();
			float y = move_ptr->y();
			// 로직 처리...
			break; 
		}
		case GameData_ChatPacket: {
			auto chat_ptr = game_packet->data_as_ChatPacket();
			std::string message = char_ptr->message()->str();
			break;
		}
		default:
			break;
	}
}
```

#### Union 구조 설계 예시 (.fbs)
```fbs
namespace Game;

table MovePacket { x:float; y:float; }
table ChatPacket { message:string; }

// Union 정의
union GameData { MovePacket, ChatPacket }

// 최상위 루트 테이블
table GamePakcet {
	data:GameData; // 여기에 union이 담김
} 

root_type GamePacket;
```

#### 워커 스레드 적용 시 주의점 (Zero-copy의 함정)
Flatbuffers는 메모리 포인터를 직접 사용하므로 데이터의 생명주기가 매우 중요함.
1. 포인터 직접 참조: `game_packet=>data_as_MovePacket()`이 반환하는 값은 새로운 객체가 아니라 수신 링버퍼 내부의 특정 주소를 가리키는 포인터 일 뿐임.
2. 안전성: 워커가 이 포인트를 들고 있는 동안 `NetworkManager`가 링버퍼의 해당 구역을 덮어쓰면 데이터가 깨짐.
	- 하지만 '전표를 잡은 워커가 끝까지 처리한다'는 구조에서는 로직이 끝날 때까지 링버퍼가 안전하므로 안심하고 사용해도 됨.

#### 검증(Verifier)
네트워크는 신뢰할 수 없음. 잘못된 데이터가 들어왔을 때 서버가 크래시 나는 것을 막으려면 파싱 전에 `Verifier`를 돌리는 것이 좋음.
```cpp
flatbuffers::Verifier verifier(buffer_ptr, size);
if (!VerifyGamePakcetBuffer(verifier)) {
	// 잘못된 패킷, 세션 차단 등의 로직 필요
	return;
}
```
*성능에 아주 민감하다면 내부망 패킷일 경우 생략하기도 하지만, 외부 노출 서버라면 필수*

