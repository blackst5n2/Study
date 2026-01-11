메모리의 연속성을 유지하면서 다차원 데이터를 효율적으로 다루는 것.

# 배열 및 행렬 (Tensor) 구현

#### 텐서의 본질: 연속적인 메모리
CPU와 GPU는 메모리가 듬성듬성 떨어진 연결 리스트보다, 연속적으로 붙어 있는 배열을 처리하는 속도가 압도적으로 빠름. 텐서는 결국 N차원의 데이터를 하나의 긴 1차원 메모리 블록에 저장하고, 수학적인 계산을 통해 원하는 위치를 찾아내는(인덱싱) 기법임.

#### 2차원 행렬 클래스 구현
가장 기본인 2차원 행렬(`Matrix`)을 구현하며, N차원 텐서로확장할수 있는 인덱싱 원리를 학습.

##### 핵심 구현: `width`와 `height` 정보를 저장하고, `float*` 또는 `int*` 포인터로 1차원 배열 메모리를 할당함.

```cpp
class Matrix {
private:
	int rows;
	int cols;
	float* data; // 실제 데이터가 저장될 1차원 메모리 블록
	
public:
	Matrix(int r, int c) : rows(r), cols(c) {
		// 동적 메모리 할당: rows * cols 크기만큼 연속적으로 할당
		data = new float[rows * cols];
	}
	
	~Matrix() {
		delete[] data; // 소멸 시 할당된 메모리 해제 (필수!)
	}

	// TODO
};
```

#### 핵심 수학: 1차원 인덱싱 공식
2차원 좌표 `(r, c)`를 1차원 배열의 인덱스 `i`로 변환하는 공식임. 이것이 텐서 연산의 핵심
$$Index(r,c) = r \times \text{cols} + c$$
- `r`: 행(row) 인덱스 (0부터 시작)
- `c`: 열(Column) 인덱스 (0부터 시작)
- `cols`: 행렬의 전체 열 개수 (너비)

##### 핵심 구현 `get` 또는 `set` 메소드에 이 공식을 적용함.
```cpp
// (r, c) 위치의 값에 접근 (배열의 값을 가져오거나 설정)
float& get(int r, int c) {
	// 경계 검사 (선택적이지만 안전성을 위해 필요)
	if (r >= rows || c >= cols || r < 0 || c < 0) {
		throw std::out_of_range("Index out of bounds");
	}
	
	// 1차원 인덱스 계산
	int index = r * cols + c;
	
	return data[index];
}
```

#### 행렬 곱셈 (Matrix Multiplication) 구현
행렬 곱셈은 AI 학습 과정(가중치 연산)과 3D 그래픽스 변환(Translation, Rotation, Scale)에 가장 많이 사용되는 연산임.

##### 핵심 수학: 행렬 $A$와 $B$의 곱 $C$에서 $C[i][j]$ 값은 **$A$의 $i$번째 행**과 **$B$의 $j$번째 열**의 **내적(Dot product)** 임.

###### 조건: $A(m \times n)$와 $B(n \times p)$는 가운데 차원 $n$이 일치해야 함. 결과 행렬 $C$는 $(m \times p)$ 차원을 가짐.
```cpp
// 결과 행렬 C = A * B
static Matrix multiply(const Matrix& A, const Matrix& B) {
	if (A.cols != B.rows) {
		throw std::invalid_argument("Matrix dimenstions must match for multiplication.");
	}
	
	Matrix C(A.rows, B.cols); // 결과 행렬 (A.row x B.cols)
	
	// 3중 루프가 핵심임.
	for (int i = 0; i < A.rows; ++i) { // A의 행 (Row)
		for (int j = 0; j < B.cols; ++j) { // B의 열 (Column)
			float sum = 0.0f;
			for (int k = 0; k < A.cols; ++k) { // 내적을 위한 공통 차원
				// A[i][k] * B[k][j]
				// 1차원 인덱싱 공식 사용: (r * cols + c)
				sum += A.data[i * A.cols + k] * B.data[k * B.cols + j];
			}
			C.get(i, j) = sum; // 결과 C[i][j]에 저장.
		}
	}
	return C;
}
```
