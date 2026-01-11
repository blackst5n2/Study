자료구조의 꽃. 포인터 기반으로 구현되며, 탐색 효율($O(\log n)$)을 극대화하여 데이터베이스 인덱스와 파일 시스템 구조에 근본적인 영향을 주었음.

# 이진 탐색 트리 (BST) 구현
#### BST의 정의 및 구조
- 정의: 모든 노드가 다음의 규칙을 따르는 이진 트리임.
	1. 왼쪽 서브 트리: 루트 노드의 키보다 작은 키를 가진 노드들로 이루어져 있음.
	2. 오른쪽 서브 트리: 루트 노드의 키보다 큰 키를 가진 노드들로 이루어져 있음.
	3. 왼쪽 및 오른쪽 서브 트리 역시 모두 BST임.
- 구현: 포인터 기반으로 구현됨. (이전 1단계의 연결 리스트 지식 활용)

#### 기본 노드 구조 및 재귀 (Recursion)
BST는 재귀적 정의를 가지므로, 삽입, 탐색, 삭제 등 모든 연산은 재귀 함수를 통해 구현하는 것이 가장 직관적이고 효율적임.

##### 핵심 구현: 연결 리스트의 노드 정의에 `left`와 `right` 포인터를 추가함.
```cpp
struct Node {
	int key; // 노드의 값 (데이터)
	Node* left; // 왼쪽 자식 노드 포인터
	Node* right; // 오른쪽 자식 노드 포인터
	
	Node(int val) : key(val), left(nullptr), right(nullptr) {}
};
```

#### 탐색 연산 (Search - `search()`)
탐색의 BST의 존재 이유임. 최악의 경우를 제외하고 $O(\log n)$의 시간이 소요됨.

##### 핵심 로직:
1. 현재 노드(Root)와 비교함.
2. 찾는 값(`key`)이 현재 값보다 작으면 왼쪽 서브 트리로 이동 (`node->left`)
3. 찾는 값(`key`)이 현재 값보다 크면 오른쪽 서브 트리로 이동 (`node->right`)
```cpp
Node* search(Node* node, int key) {
	// 1. 기저 조건: 노드가 없거나 (못 찾음) 키를 찾았을 때
	if (node == nullptr || node->key == key) {
		return node;
	}
	
	// 2. 재귀 조건: 키가 작으면 왼쪽으로, 크면 오른쪽으로 이동
	if (key < node->key) {
		return search(node->left, key);
	} else {
		return search(node->right, key);
	}
}
```

#### 삽입 연산 (Insertion - `insert()`)
삽입은 탐색 로직과 동일하게 내려가다가, 더 이상 갈 곳이 없을 때(노드가 `nullptr`일 때) 새 노드를 생성함.
```cpp
Node* insert(Node* node, int key) {
	// 1. 기저 조건: 삽입할 위치를 찾았을 때
	if (node == nullptr) {
		return new Node(key);
	}
	
	// 2. 재귀 조건: 규칙에 따라 왼쪽 또는 오른쪽으로 이동
	if (key < node->key) [
		node->left = insert(node->left, key);
	] else if (key > node->key) {
		node->right = insert(node->right, key);
	}
	// 키가 이미 있다면(같다면) 아무것도 하지 않고 현재 노드를 반환
	
	return node;
}
```

#### 시행착오와 한계 (BST의 약점)
BST의 경우 최적의 경우 $O(\log n)의 성능을 제공하지만, 데이터가 이미 **정렬된 순서(예: 1, 2, 3, 4, 5)** 로 삽입되면 트리가 한쪽으로 치우치는 **편향(Skewed)** 현상이 발생함.
- 1 -> 2 -> 3 -> 4 -> 5 순으로 삽입될 경우, 트리의 모양은 결국 연결 리스트와 같아짐.
- 이 경우 탐색 시간 복잡도는 $O(n)$으로 최악의 성능을 가지게 됨.

# 구현 체크포인트
1. 재귀적 사고: 모든 연산을 재귀적으로 구현하며 트리의 본질을 이해.
2. 탐색 효율성: 이진 탐색 규칙을 통해 $(O\log n)$의 성능을 어떻게 얻는지 이해.
3. 한계 인식: 최악의 경우 $O(n)$이 되는 BST의 근본적인 문제점을 파악.
