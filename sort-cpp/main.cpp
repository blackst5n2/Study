#include <iostream>
#include <random>
#include <algorithm>

#define N 10

// 출력 헬퍼 함수
void printArray(int arr[]) {
    std::cout << "[ ";
    for (int i = 0; i < N; ++i) {
        std::cout << arr[i] << " ";
    }
    std::cout << "]";
    std::cout << std::endl;
}

// 배열 복사 헬퍼 함수
void copyArr(int* cpyArr ,int* arr) {
    memcpy(cpyArr, arr, sizeof(int) * N);
}

// 선택 정렬 (Selection Sort)
// 순서 1. 전체 배열의 첫 번째 왼소부터 마지막-1 원소까지 순회함. (정렬 완료된 부분은 제외).
// 순서 2. 현재 위치(i)부터 끝까지 배열을 탐색하여 가장 작은 원소의 인덱스를 찾음 (min_idx).
// 순서 3. i 위치의 원소와 찾은 min_idx 위치의 원소를 교환 함.
void selectionSort(int arr[]) {
    for (int i = 0; i < N - 1; ++i) {
        // 1. i 위치를 기준으로, 현재까지 찾은 최솟값의 인덱스를 i로 초기화
        int min_idx = i;

        // 2. i+1 부터 배열의 끝까지 탐색하며 최솟값을 찾음.
        for (int j = i + 1; j < N; ++j) {
            // 현재 왼소(arr[j])가 현재까지 찾은 최솟값(arr[min_idx])보다 작으면
            if (arr[j] < arr[min_idx]) {
                min_idx = j; // 최솟값 인덱스 업데이트
            }
        }
        
        // 3. 찾은 가장 작은 원소(arr[min_idx])를 현재 정렬 위치(arr[i])로 교환함.
        // 만약 i와 min_idx가 같다면 교환하지 않음.
        if (min_idx != i) {
            std::swap(arr[i], arr[min_idx]);
        }
    }
    
    std::cout << "선택 정렬(Selection Sort): ";
    printArray(arr);
}

// 삽입 정렬 (Insertion Sort)
void insertionSort(int arr[]) {
    // i = 1부터 시작: 첫 번째 원소(i=0)는 이미 정렬된 것으로 간준
    for (int i = 1; i < N; ++i) {
        // 1. 현재 삽입할 원소(Key)를 저장
        int key = arr[i];

        // 2. Key를 삽입할 위치를 찾기 위해, 정렬된 부분(i-1부터 0까지)을 역순으로 탐색
        int j = i - 1;
        while (j >= 0 && arr[j] > key) {
            arr[j + 1] = arr[j]; // 원소를 오른쪽으로 이동
            --j; // 다음 왼쪽 원소로 이동
        }

        // 4. while 루프가 끝나면 j+1 위치가 Key가 삽입될 최종 위치
        arr[j + 1] = key;
    }

    std::cout << "삽입 정렬(Insertion Sort): ";
    printArray(arr);
}

// 버블 정렬
void bubbleSort(int arr[]) {
    bool swapped; // 교환이 발생했는지 확인하는 플래그

    // i: 배열의 끝에서부터 정렬이 완료된 원소의 개수 (정렬 범위 축소)
    for (int i = 0; i < N - 1; ++i) {
        swapped = false; // 새로운 순회 시작 시 플래그 초기화

        // j: 비교할 인접한 두 원소 중 앞쪽 인덱스
        // n-1-i 까지만 순회: i개는 이미 정렬되어 있으므로 제외
        for (int j = 0; j < N - 1 - i; ++j) {

            // 1. 인접한 두 원소 비교 (arr[j]와 [arrj+1])
            // 앞 원소가 뒤 원소보다 크다면 (오름차순 기준에 맞지 않으면)
            if (arr[j] > arr[j + 1]) {
                // 2. 위치 교환(Swap)
                std::swap(arr[j], arr[j + 1]);
                swapped = true; // 교환이 발생했음을 기록
            }
        }

        // 3. 최적화: 안쪽 루프에서 한 번도 교환이 일어나지 않았다면,
        // 배열은 이미 정렬이 완료된 상태이므로 정렬을 중단함.
        if (swapped == false) {
            break;
        }
    }

    std::cout << "버블 정렬(Bubble Sort): ";
    printArray(arr);
}

// 병합 정렬 (Merge Sort)
// 병합 정렬은 재귀 함수와 정렬된 두 배열을 합치는 merge 함수 두 부분으로 구현됨.
int temp[N]; // 전역 임시 배열 (추가 공간)
void merge(int arr[], int start, int mid, int end) {
    int i = start;
    int j = mid + 1;
    int k = start;
    
    // 1. 두 부분 배열을 비교하여 작은 원소부터 임시 배열에 복사
    while (i <= mid && j <= end) {
        if (arr[i] <= arr[j]) { // 안정성을 위해 <= 사용
            temp[k++] = arr[i++];
        } else {
            temp[k++] = arr[j++];
        }
    }

    // 2. 왼쪽 부분 배열에 남아있는 원소를 모두 복사
    while (i <= mid) {
        temp[k++] = arr[i++];
    }

    // 3. 오른쪽 부분 배열에 남아있는 원소를 모두 복사
    while (j <= end) {
        temp[k++] = arr[j++];
    }

    // 4. 임시 배열의 내용을 원래 (arr)로 복사 (start부터 end까지)
    for (int idx = start; idx <= end; ++idx) {
        arr[idx] = temp[idx];
    }
}
// mergeSort 함수 (재귀 로직)
/**
 * 병합 정렬 알고리즘 구현. (재귀 함수)
 * @param arr 정혈할 정수형 벡터 (배열)
 * @param start 배열의 시작 인덱스
 * @param end 배열의 끝 인덱스
 */
void mergeSort(int arr[], int start, int end) {
    // 재귀 종료 조건: 부분 배열의 크기가 1이하일 때
    if (start >= end) {
        return;
    }

    // 1. 분할: 배열을 중간 지점으로 나눔
    int mid = start + ((end - start) / 2); // 오버플로우 방지

    // 2. 정복: 재귀적으로 왼쪽 부분과 오른쪽 부분을 정렬
    mergeSort(arr, start, mid); // 왼쪽 부분 정렬
    mergeSort(arr, mid + 1, end); // 오른쪽 부분 정렬
    
    // 3. 병합: 정렬된 두 부분을 합침
    merge(arr, start, mid, end);
}

// 퀵 정렬 (Quick sort)
/**
 * 중앙값 피벗 선택을 적용하는 함수
 * 세 원소 중 중앙값을 찾아 arr[start] 위치로 이동시킴.
 * arr[start]의 값이 partition 함수의 피벗이 됨.
 * @param arr 정렬할 배열
 * @param start 시작 인덱스
 * @param end 끝 인덱스
 */
void medianOfThree(int arr[], int start, int end) {
    int mid = start + (end - start) / 2; // 안전한 중간 인덱스 계산
    
    // 1. start, mid, end 세 인덱스의 원소들을 정렬함.
    // 목표: arr[mid]에 세 원소 중 중앙값을 위치시킴.

    // start와 mid를 비교하여 start가 mid보다 크면 교환 (start에 작은 값)
    if (arr[start] > arr[mid]) {
        std::swap(arr[start], arr[mid]);
    }

    // start와 end중 큰 값을 arr[end]에 위치
    if (arr[start] > arr[end]) {
        std::swap(arr[start], arr[end]);
    }

    // mid와 end중 큰 값을 arr[end]에 위치
    if (arr[mid] > arr[end]) {
        std::swap(arr[mid], arr[end]);
    }

    // 피벗 옮기기
    // Sentinel 역할 수행
    std::swap(arr[start], arr[mid]);
}

/**
 * Hoare Partition 구현
 * 피벗은 arr[start]로 설정
 */
int partition(int arr[], int start, int end) {
    // start와 end-1이 피벗 준비 단계에서 변경되었을 수 있으므로 end-1을 피벗으로 사용
    int pivot = arr[start]; // 피벗

    // i는 start에서 j는 end+1에서 시작하여 피벗을 기준으로 분할
    int i = start;
    int j = end + 1;

    while(true) {
        // i 포인터 이동: 피벗보다 크거나 같은 원소를 찾을 때까지 이동
        // arr[end] 경계를 넘지 않도록 i <= end 조건 추가
        do { i++; } while (i <= end && arr[i] < pivot);

        // j 포인터 이동: 피벗보다 작거나 같은 원소를 찾을 때까지 이동
        // arr[start] 경계를 넘지 않도록 j > start 조건 추가
        do { j--; } while (arr[j] > pivot);

        // 포인터가 교차하면 루프 종료
        if (i >= j) break;
        
        // i와 j가 멈춘 위치의 원소를 교환
        std::swap(arr[i], arr[j]);
    }

    // 피벗의 최종 위치 확정: arr[start](피벗)와 arr[j]를 교환
    std::swap(arr[start], arr[j]);

    // 분할된 피벗의 최종 위치를 반환
    return j;
}
/**
 * 퀵 정렬 알고리즘 (Sentinel + Median of Three)
 */
void quickSort(int arr[], int start, int end) {
    // 2개 이상의 원소가 있을 때만 분할을 수행
    if (start < end) {

        // 최소 3개 이상의 원소가 있을 때 중앙값 방식을 사용
        if (end - start + 1 >= 3) {
            medianOfThree(arr, start, end);
        }
        // 1. 배열을 피벗을 기준으로 분할하고 피벗의 위치를 받음
        int pi = partition(arr, start, end); // start와 end를 포함한 영역 분할

        // 2. 피벗의 원쪽 부분 배열 정렬 (start ~ pi-1)
        quickSort(arr, start, pi - 1);

        // 3. 피벗의 오른쪽 부분 배열 정렬 (pi+1 ~ end)
        quickSort(arr, pi + 1, end);
    }
}

// 힙 정렬 (Heap Sort)
/**
 * heapify 구현
 * @param arr 배열
 * @param n 힙의 크기
 * @param i 현재 노드의 인덱스
 */
void heapify(int arr[], int n, int i) {
    int largest = i; // 가장 큰 값을 현재 루트로 가정
    int left = 2 * i + 1; // 왼쪽 자식
    int right = 2 * i + 2; // 오른쪽 자식

    // 1. 왼쪽 자식이 힙 크기 내에 있고, 현재 largest 보다 크면 갱신
    if (left < n && arr[left] > arr[largest]) {
        largest = left;
    }

    // 2. 오른쪽 자식이 힙 크기 내에 있고, 현재 largest보다 크면 갱신
    if (right < n && arr[right] > arr[largest]) {
        largest = right;
    }

    // 3. largest가 루트 i가 아니면 (자식 중 더 큰 값이 있으면)
    if (largest != i) {
        // 루트와 가장 큰 자식을 스왑
        std::swap(arr[largest], arr[i]);

        // 스왑된 자식 노드부터 재귀적으로 heapify 호출
        heapify(arr, n, largest);
    }
}
// 힙 정렬 주 함수
void heapSort(int arr[], int n) {
    // --- 1. 최대 힙 구성 (O(n)) ---
    // 배열의 마지막 부모 노드부터 시작하여 위로 올라가며 heapify 호출
    // 마지막 부모 노드 인덱스: n/2 - 1
    for (int i = n / 2 - 1; i >= 0; i--) {
        heapify(arr, n, i);
    }

    // --- 2. 정렬 (O(n log n)) ---
    // 힙의 크기를 줄여가며 정렬 수행
    for (int i = n - 1; i > 0; i--) {
        // 루트(가장 큰 값)와 현재 배열의 마지막 원소(i)를 스왑
        std::swap(arr[0], arr[i]);

        // 힙의 크기를 1 줄이고 (i는 이미 정렬됨), 새로운 루트(0)를 대상으로 heapify 호출
        // 이제 힙의 크기는 i (0부터 i-1까지)
        heapify(arr, i, 0);
    }

    std::cout << "힙 정렬(Heap Sort): ";
    printArray(arr);
}

int main(int, char**){
    // 랜덤 초기화
    std::default_random_engine generator;
    std::uniform_int_distribution<int> distribution(0, 100);

    // 배열 초기화
    int arr[N];

    for (int i = 0; i < N; ++i) {
        arr[i] = distribution(generator);
    }

    // 배열 초기 상태
    std::cout << "초기 배열: [ ";
    for (int i = 0; i < N; ++i) {
        std::cout << arr[i] << " ";
    }
    std::cout << "]" << std::endl;
    
    // 선택 정렬
    int s_arr[N];
    copyArr(s_arr, arr);
    selectionSort(s_arr);

    // 삽입 정렬
    int i_arr[N];
    copyArr(i_arr, arr);
    insertionSort(i_arr);
    
    // 버블 정렬
    int b_arr[N];
    copyArr(b_arr, arr);
    bubbleSort(b_arr);

    // 병합 정렬
    int m_arr[N];
    copyArr(m_arr, arr);
    mergeSort(m_arr, 0, N - 1);
    std::cout << "병합 정렬(Merge Sort): ";
    printArray(m_arr);

    // 퀵 정렬
    int q_arr[N];
    copyArr(q_arr, arr);
    quickSort(q_arr, 0, N - 1);
    std::cout << "퀵 정렬(Quick Sort): ";
    printArray(q_arr);

    // 힙 정렬
    int h_arr[N];
    copyArr(h_arr, arr);
    heapSort(arr, N);

    std::cout << "Hello, from algorithms!\n";
}
