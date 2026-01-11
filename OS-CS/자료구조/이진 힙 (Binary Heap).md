이진 힙은 배열을 기반으로 구현되는 특수한 트리 구조임. OS 스케줄러가 수많은 작업 중 다음으로 가장 중요한 작업을 $O(1)$에 찾고 $O(\log n)$ 의 시간에 재정렬하는 **우선순위 큐(Priority Queue)** 를 구현하는 데 필수적임.

# 이진 힙 (Binary Heap) 구현
#### 힙의 정의 및 구조
- 정의: 완전 이진 트리(Complete Binary Tree)의 한 종류.
	- 힙 속성:
		- 최대 힙 (Max Heap): 부모 노드의 값이 항상 자식 노드의 값보다 크거나 같음. (가장 큰 값이 루트에 있음)
		- 최소 힙 (Min Heap): 부모 노드의 값이 항상 자식 노드의 값보다 작거나 같음. (가장 작은 값이 루트에 있음)
	- 구현: 포인터 기반이 아닌 배열 기반으로 구현함.

#### 핵심 수학: 배열 인덱스 공식
힙은 트리를 배열에 저장하기 때문에, 부모-자식 관계를 인덱스 공식으로 계산함. **이 공식을 이해하는 것이 구현의 핵심**. (인덱스가 0부터 시작한다고 가정)

| 관계     | 인덱스 공식                                |
| ------ | ------------------------------------- |
| 부모 노드  | $Parent(i) = \lfloor(i - 1)/2\rfloor$ |
| 왼쪽 자식  | $LeftChild(i) = 2i + 1$               |
| 오른쪽 자식 | $RightChild(i) = 2i + 2$              |

#### 배열 기반 힙 구현 (Max Heap 기준)
##### 핵심 구현: `std::vector` (C++) 또는 동적 배열을 사용하여 데이터를 저장하고, 위 공식을 통해 관계를 파악함.

##### 힙에 삽입 (Insertion - `insert()`)
1. 새 요소를 배열의 맨 끝에 삽입함. (완전 이진 트리의 속성을 유지)
2. 삽입된 노드의 힙 속성이 위반될 수 있으므로, 해당 노드를 '힙 속성을 만족할 때까지' 부모와 교환하며 위로 올려보냄. (Up-Heap 또는 Heapify-Up 과정)

| 연산            | 시간 복잡도      |
| ------------- | ----------- |
| 삽입 (`insert`) | $O(\log n)$ |

##### 최댓값 추출 및 삭제 (Extraction - `extractMax()`)
1. 최댓값(루트, 즉 배열의 `data[0]`)을 추출함.
2. 배열의 맨 끝 요소를 루트(`data[0]`)로 옮김.
3. 루트 노드의 힙 속성이 위반될 수 있으므로, 해당 노드를 '힙 속성을 만족할 때까지' 더 큰 자식과 교환하며 아래로 내려 보냄. (Down-Heap 또는 Heapify-Down 과정)

| 연산                | 시간 복잡도      |
| ----------------- | ----------- |
| 추출 (`extractMax`) | $O(\log n)$ |

#### 구현의 핵심 로직
Up-heap (삽입 후 위로 올리기) 로직:
```cpp
void heapifyUp(int i) {
	// i > 0: 루트 노드가 아닐 때까지 반복
	while (i > 0 && data[i] > data[Parent(i)]) {
		// 자식이 부모보다 크다면 교환
		std::swap(data[i], data[Parent(i)]);
		i = Parent(i); // 인덱스를 부모 위치로 이동하여 반복
	}
}
```
Down-Heap (삭제 후 아래로 내리기) 로직:
```cpp
void heapifyDown(int i) {
	int maxIndex = i;
	int l = LeftChild(i);
	int r = RightChild(i);
	
	// 1. 왼쪽 자식과 비교
	if (l < data.size() && data[l] > data[maxIndex]) {
		maxIndex = l;
	}
	
	// 2. 오른쪽 자식과 비교 
	if (r < data.size() && data[r] > data[maxIndex]) {
		maxIndex = r;
	}
	
	// 3. 교환이 필요하다면
	if (maxIndex != i) {
		std::swap(data[i], data[maxIndex]);
		heapifyDown(maxIndex); // 재귀적으로 아래로 내려가며 반복
	}
}
```

# 구현 체크포인트
1. 메모리 효율성: 포인터 없이 배열만 사용했으므로 메모리 낭비가 거의 없음.
2. 속도: 삽입/삭제가 $O(\log n)$으로 매우 빠름.
3. 응용: OS 스케줄러가 다음 실행할 프로세스를 찾을 때, 이 `extractMax()` (혹은 `extractMin()`) 로직을 사용함.
