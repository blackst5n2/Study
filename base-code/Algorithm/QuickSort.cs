using System;

// 퀵 정렬(Quick Sort) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 대부분의 실전 상황에서 매우 빠른 정렬이 필요할 때
// - 표준 라이브러리 정렬의 기반이 되는 알고리즘
// - 예시: 대용량 배열 정렬, 실시간 처리, 라이브러리 내부 구현 등

class QuickSortExample
{
    // 퀵 정렬 함수
    public static void QuickSort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivotIdx = Partition(arr, left, right); // 피벗 기준 분할
            QuickSort(arr, left, pivotIdx - 1);  // 왼쪽 부분 정렬
            QuickSort(arr, pivotIdx + 1, right); // 오른쪽 부분 정렬
        }
    }

    // 분할 함수: 피벗보다 작은 값은 왼쪽, 큰 값은 오른쪽으로 이동
    static int Partition(int[] arr, int left, int right)
    {
        int pivot = arr[right]; // 마지막 원소를 피벗으로 선택
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                // arr[i]와 arr[j] 교환
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
        // 피벗을 올바른 위치로 이동
        int temp2 = arr[i + 1];
        arr[i + 1] = arr[right];
        arr[right] = temp2;
        return i + 1;
    }

    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[] arr = { 10, 7, 8, 9, 1, 5 };
        Console.WriteLine("정렬 전: " + string.Join(", ", arr));
        QuickSort(arr, 0, arr.Length - 1);
        Console.WriteLine("정렬 후: " + string.Join(", ", arr));
    }
}
