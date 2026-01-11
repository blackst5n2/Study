using System;

// 이진 탐색(Binary Search) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 정렬된 배열/리스트에서 빠르게 값을 찾고 싶을 때
// - 예시: 사전 검색, 파라메트릭 서치, lower/upper bound, 효율적 탐색 등

class BinarySearchExample
{
    // 이진 탐색 예제 클래스
    // 이진 탐색 함수 (정렬된 배열에서)
    // 이진 탐색 함수: 정렬된 배열에서 특정 값을 빠르게 찾음
    public static int BinarySearch(int[] arr, int target)
    {
        int left = 0, right = arr.Length - 1;
        // left와 right가 교차할 때까지 반복
        while (left <= right)
        {
            int mid = (left + right) / 2; // 중간 인덱스
            if (arr[mid] == target) return mid; // 값을 찾으면 인덱스 반환
            else if (arr[mid] < target) left = mid + 1; // 오른쪽 반 탐색
            else right = mid - 1; // 왼쪽 반 탐색
        }
        return -1; // 못 찾으면 -1 반환
    }

    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[] arr = { 1, 2, 5, 5, 6, 9 }; // 정렬된 배열
        int target = 5; // 찾을 값
        int idx = BinarySearch(arr, target); // 이진 탐색 호출
        if (idx != -1)
            Console.WriteLine($"{target}은(는) 인덱스 {idx}에 있습니다.");
        else
            Console.WriteLine($"{target}을(를) 찾을 수 없습니다.");
    }
}
