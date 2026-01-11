using System;

// 버블 정렬(Bubble Sort) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 간단한 정렬이 필요할 때, 정렬 원리를 이해하고 싶을 때
// - 데이터 개수가 매우 적을 때(실전에서는 거의 사용하지 않음)
// - 예시: 정렬 알고리즘 교육, 시각화 등

class BubbleSortExample
{
    // 버블 정렬 예제 클래스
    // 버블 정렬 함수
    // 버블 정렬 함수: 인접한 두 수를 비교해가며 정렬
    public static void BubbleSort(int[] arr)
    {
        int n = arr.Length;
        // 배열의 끝까지 반복
        for (int i = 0; i < n - 1; i++)
        {
            // 인접한 두 원소를 비교하여 큰 값을 뒤로 보냄
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    // 두 값을 교환(swap)
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }

    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[] arr = { 5, 2, 9, 1, 5, 6 }; // 정렬할 배열
        Console.WriteLine("정렬 전: " + string.Join(", ", arr));
        BubbleSort(arr); // 버블 정렬 호출
        Console.WriteLine("정렬 후: " + string.Join(", ", arr));
    }
}
