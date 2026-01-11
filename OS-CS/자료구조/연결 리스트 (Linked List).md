#### 기본 구조 정의
연결 리스트는 '노드(Node)'들의 집합이며, 각 노드는 데이터와 다음 노드를 가리키는 포인터를 가짐.

##### 핵심 구현: `struct` 또는 `class`를 사용하여 노드를 정의

```c++
struct Node {
	int data; // 노드가 저장할 데이터
	Node* next // 다음 노드를 가리키는 포인터
	
	// 생성자
	Node(int val) : data(val), next(nullptr) {}
};

class LinkedList {
private:
	Node* head; // 리스트의 시작을 가리키는 포인터
	
public:
	LinkedList() : head(nullptr) {}
	// TODO
}
```

#### 삽입 연산 (Insertion)
데이터를 리스트의 맨 앞에 삽입하느 것이 가장 기본적이며, $O(1)$의 시간이 소요됨.

##### 핵심 로직: 새로운 노드를 만들고, 그 노드의 `next`가 기존의 `head`를 가리키게 한 후, `head`를 새 노드로 변경함.

```cpp
void insertAtFront(int val) {
	// 1. 새 노드를 동적으로 생성 (메모리 할당)
	Node* newNode = new Node(val);
	
	// 2. 새 노드의 next 포인터가 현재 head를 가리키게 설정
	newNode->next = head;
	
	// 3. head 포인터를 새 노드로 업데이트
	head = newNode;
}
```

#### 탐색 및 출력 (Traversal)
리스트르 처음부터 끝까지 순회하는 기본 연산.

##### 핵심 로직: `head`부터 시작하여 한 칸씩 이동하며 (`current = current->next`), `next`가 `nullptr`이 될 때까지 반복함.

```cpp
void display() {
	Node* current = head; // head부터 순회 시작
	
	while (current != nullptr) {
		std::cout << current->data << " -> ";
		current = current->next; // 다음 노드로 이동
	}
	std::cout << "nullptr" << std::endl;
}
```


#### 삭제 연산 (Deletion)
맨 앞 노드를 삭제하는 것이 가장 효율적 ($O(1)$).

##### 가장 중요한 주의점 (메모리 해제): 삭제할 노드는 반드시 `delete` (C++) 또는 `free` (C)를 사용하여 운영체제에 메모리를 반환해야 함. 이를 잊으면 **메모리 누수(Memory Leak)** 가 발생함.

```cpp
void deleteFront() {
	if (head == nullptr) {
		std::cout << "List is empty." << std::endl;
		return;
	}
	
	// 1. 삭제할 노드를 임시 포인터에 저장
	Node* temp = head;
	
	// 2. head 포인터를 다음 노드로 이동
	head = head->next;
	
	// 3. 임시 포인터가 가리키던 노드의 메모리를 해제 (필수!)
	delete temp;
}
```

# 구현 체크포인트
다음 두 가지 핵심 지식을 습득하고 이해한 것.
1. 포인터 조작 능력: 포인터의 값(주소)을 능동적으로 변경하고, 한 노드가 다른 노드를 참조하게 만드는 방법을 터득함.
2. 동적 메모리 관리: 데이터를 저장하기 위해 필요한 순간에 메모리를 할당하고, 사용이 끝났을 때 정확하게 반환하는 습관을 들였음.
