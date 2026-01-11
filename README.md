### 공부하고 학습한 내용을 정리해두는 공간입니다.
예전에 학습한 것들과 현재 학습한 것들을 소스만 따로 뽑아서 올려뒀습니다. 예전부터 배워보고 싶은 것들이 있거나, 만들어보고 싶은 것들이 있으면 어떻게든 찾아내서 구현해보고 개발해보는 일들을 했습니다. 해당 결과물이 어느 플랫폼에서 좀 더 보기 편하고, 사용감이 괜찮은 지에 대해서 생각해 본 다음에 해당 플랫폼에 효과적인 기술스택을 적용하여 개발하는 것을 가장 기본으로 원칙으로 삼았습니다.

### 구현 목록 및 공부
#### 알고리즘 구현
C#을 이용하여 알고리즘을 학습했습니다. 추가로 react를 이용하여 해당 알고리즘을 시각적으로 표현하는 기능을 만들었습니다.
정렬 알고리즘은 최근에 C++에서 다시 추가로 구현해봤습니다.

#### 간단한 근태 관리 앱
flutter + sqlite를 이용한 간단한 출/퇴근 및 할일 기록 앱을 만들었습니다.
- 출/퇴근 시간을 직접 설정하고 출근/퇴근을 기록하는 기능
- 고정 할 일을 설정, 추가 할 일을 설정 및 생성하여 퀘스트 완료 형식으로 즉시 $\frac{\text{할 일 개수}}{\text{하루 급여}}$ 원 만큼을 적립 해주는 gamification 요소
- 세율 선택 및 계산 기능

![gt1](https://github.com/user-attachments/assets/fcbcc907-08e4-4308-bd4e-55cf1fedd06d)
![gt2](https://github.com/user-attachments/assets/930a1e01-7be5-4cca-8ea5-33b227f9d03a)
![gt3](https://github.com/user-attachments/assets/97a2862f-b8cf-4bbb-9907-5977cd3ef472)


#### todo list 앱
flutter + sqlite를 이용한 간단한 todo-list 앱을 만들었습니다.
- 할 일별 카테고리를 설정하는 기능
- 캘린더에서 날짜를 선택하고 해당 날짜에 할 일을 CRUD 하는 기능
- 반복 일정을 설정하는 기능
- 그래프로 통계치를 확인하는 기능

![dp1](https://github.com/user-attachments/assets/0ecb3855-413a-45bf-b64f-0bd1b49e141b)
![dp2](https://github.com/user-attachments/assets/b8f309ff-b6c7-471f-ad1b-bb45da934196)
![dp3](https://github.com/user-attachments/assets/dd4c369e-2235-4f98-be90-9e361b282c7a)
![dp4](https://github.com/user-attachments/assets/8b3707ef-c6a4-48e5-9c86-e9f6c07466ce)
![dp5](https://github.com/user-attachments/assets/b6f8280c-c849-45aa-92b3-8cc7b82cd246)
![dp6](https://github.com/user-attachments/assets/9bf3b8a7-6c94-406b-ab36-e2e13455d459)
![dp7](https://github.com/user-attachments/assets/22c0dbdd-3d16-4961-866f-b051966e9153)


#### 올리브영 근무 도우미 앱
flutter + sqlite를 이용한 간단한 올리브영 근무 도움 앱을 만들었습니다.
- 매장 구조도를 2D 형태로 간단하게 박스를 그려 넣어서 진열장을 만드는 기능
- 특정 품목을 추가 및 삭제 할 수 있는 기능
- 특정 품목을 추가할 때 진열장을 선택하면 해당 물건을 찾을 때 해당 진열장이 활성화 되는 기능
- 등록한 품목에 어떤 성분이 들어갔는지 검색할 수 있는 기능
- 간단한 정규표현식을 이용한 특정 고민(주름, 미백 등)을 입력하면 등록된 제품에서 추천해주는 기능
- 품목을 추가할 때 바코드로 스캔해서 등록할 수 있는 기능

![oy1](https://github.com/user-attachments/assets/39e531e0-fbb6-48f4-9428-955263abc7de)
![oy2](https://github.com/user-attachments/assets/9b2462ed-6646-44db-8af8-8f2d8d860436)
![oy3](https://github.com/user-attachments/assets/a3571978-0e7b-4731-b866-3d513a7b8003)
![oy4](https://github.com/user-attachments/assets/fc533b6c-46ac-48c7-aad1-a464a2e41c49)
![oy5](https://github.com/user-attachments/assets/034808e3-a7c5-4f83-8c3e-4e021f3c30c3)
![oy6](https://github.com/user-attachments/assets/4fa0ab7e-0cf0-4353-9373-db5874c1388a)
![oy7](https://github.com/user-attachments/assets/7fc14938-a097-4426-85b4-b3854a99e9a9)
![oy8](https://github.com/user-attachments/assets/900430ad-05c9-4367-b183-4158e05624f2)
![oy9](https://github.com/user-attachments/assets/5686e329-d915-43d9-8c5e-6cc28bd7d017)
![oy10](https://github.com/user-attachments/assets/4504ebee-ace6-42c3-be34-2bd29a596085)
![oy11](https://github.com/user-attachments/assets/8e79b5fa-2ca8-4a39-9b0a-ea7ae18e93c5)
![oy12](https://github.com/user-attachments/assets/d0ddd7e1-f79a-4d83-aeb9-08d706f3986f)



#### C# 기반 서버 및 웹 인증 서버
C# + PostgreSQL + Json + react + ASP.Net + node.js(express) + WPF를 이용해서 궁금했던 내용들에 대해서 구현해봤습니다.
- 회원 가입
- 미들웨어 인증
- JWT
- OAuth 2.0
- 계층형 아키텍처
- Json 파싱
- 어드민 페이지
- 통계 페이지
- 기본 뼈대 및 로직

#### C++ 기반 서버
고성능 서버를 학습하고 싶어서 macOS에서 구현해봤습니다.
현재 클라이언트 테스트 진행 중에 패킷이 다시 send되지 않는 오류를 해결하고 있습니다.
- kqueue
- mpmc queue
- remote free queue
- local arena (free list / linear)
- custom memory allocator
- mirror ring buffer
- flatbuffers

#### OS, CS 학습
이론적으로 부족한 부분들을 학습한 내용입니다.




















