규칙(접근 제한)을 추가한 연결 리스트의 특수 형태.

# 스택 (Stack) 구현

#### 스택 (Stack): LIFO (Last In, First Out)
스택은 접시를 쌓는 것과 같음. 가장 나중에 넣은 데이터가 가장 먼저 나옴.

##### 핵심 구현: 연결 리스트의 맨 앞 노드에서만 삽입(Push)과 삭제(Pop)가 일어나도록 제한함.

| 스택 용어         | 연결 리스트 함수       | 시간 복잡도 |
| ------------- | --------------- | ------ |
| Push (삽입)     | `insertAtFront` | $O(1)$ |
| Pop (삭제)      | `deleteFront`   | $O(1)$ |
| Peek (최상위 확인) | `head` 데이터 반환   | $O(1)$ |

```cpp
class Stack {
private:
	Node* top // 스택의 최상단을 가리키는 포인터 (Linked List의 head와 동일)
	
public:
	Stack() : top(nullptr) {}
	
	void push(int val) {
		// Linked List의 insertAtFront 로직과 동일
		Node* newNode = new Node(val);
		newNode->next = top;
		top = newNode;
	}
	
	int pop() {
		if (top == nullptr) {
			throw std::runtime_error("Stack Underflow"); // 에외 처리
		}
		
		// Linked List의 deleteFront와 동일
		int poppedValue = top->data;
		Node* temp = top;
		top = top->next;
		delete temp;
		
		return poppedValue;
	}
};
```
