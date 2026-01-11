# 큐 (Queue) 구현
#### 큐 (Queue): FIFO (First In, First Out)
큐는 줄을 서는 것과 같음. 가장 먼저 들어온 데이터가 가장 먼저 나옴.

##### 핵심 구현: 삽입(Enqueue)과 삭제(Dequeue)가 리스틔 서로 다른 끝에서 일어나도록 함.

| 큐 용어         | 연결 리스트 함수                | 시간 복잡도                 |
| ------------ | ------------------------ | ---------------------- |
| Enqueue (삽입) | `insertAtRear` (맨 뒤에 추가) | $O(1)$ (Tail 포인터 사용 시) |
| Dequeue (삭제) | `deleteFront` (맨 앞에서 제거) | $O(1)$                 |

```cpp
class Queue {
private:
	Node* front; // 큐의 시작 (삭제 지점)
	Node rear; // 큐의 끝 (삽입 지점)

public:
	Queue() : front(nullptr), rear(nullptr) {}

	void enqueue(int val) {
		Node* newNode = new Node(val);
		
		if (rear == nullptr) { // 큐가 비어있는 경우 (front와 rear 모두 설정)
			front = rear = newNode;
		} else {
			// 기존 rear의 다음을 새 노드로 연결하고, rear를 새 노드로 업데이트
			rear->next = newNode;
			rear = newNode;
		}
	}
	
	int dequeue() {
		if (front == nullptr) {
			throw std::runtime_error("Queue Underflow");
		}
		
		int dequeuedValue = front->data;
		Node* temp = front;
		front = front->next; // front를 다음 노드로 이동
		
		if (front == nullptr) { // 만약 마지막 노드를 삭제 했다면 rear도 nullptr로 설정
			rear = nullptr;
		}
		
		delete temp;
		return dequeuedValue;
	}
};
```

# 구현 체크포인트
1. 스택: `push`와 `pop`이 모두 앞에서만 이루어져 $O(1)$이 소요되는지 확인.
2. 큐: `enqueue`와 `dequeue` 연산을 모두 $O(1)$에 수행하기 위해 `front`와 `rear` 두 개의 포인터를 사용해야 함을 이해.
3. 엣지 케이스 처리: 큐가 빌 때 (`front == nullptr`) 또는 (`rear == nullptr`) 발생하는 특수 상황을 처리.
