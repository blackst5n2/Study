using System;

// 선택 정렬(Selection Sort) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 데이터 개수가 적고, 구현이 간단한 정렬이 필요할 때
// - 실전에서는 거의 사용하지 않지만, 정렬 원리 교육에 적합
// - 예시: 정렬 알고리즘 교육, 시각화 등

class SelectionSortExample
{
    // 선택 정렬 함수
    public static void SelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIdx = i; // 현재 구간의 최소값 인덱스
            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minIdx]) minIdx = j;
            }
            // 최소값을 맨 앞으로 이동
            int temp = arr[i];
            arr[i] = arr[minIdx];
            arr[minIdx] = temp;
        }
    }

    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[] arr = { 7, 4, 5, 2, 9 };
        Console.WriteLine("정렬 전: " + string.Join(", ", arr));
        SelectionSort(arr);
        Console.WriteLine("정렬 후: " + string.Join(", ", arr));
    }
}
