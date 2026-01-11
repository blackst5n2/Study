using System;

// 삽입 정렬(Insertion Sort) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 데이터가 거의 정렬되어 있거나, 데이터 개수가 적을 때
// - 실전에서는 버블/선택보다 효율적이며, 부분 정렬에 강점
// - 예시: 작은 배열 정렬, 온라인 입력 정렬, 정렬 알고리즘 교육 등

class InsertionSortExample
{
    // 삽입 정렬 함수
    public static void InsertionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 1; i < n; i++)
        {
            int key = arr[i]; // 삽입할 값
            int j = i - 1;
            // key보다 큰 값들은 한 칸씩 뒤로 이동
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j--;
            }
            arr[j + 1] = key; // key를 올바른 위치에 삽입
        }
    }

    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[] arr = { 9, 3, 5, 1, 6 };
        Console.WriteLine("정렬 전: " + string.Join(", ", arr));
        InsertionSort(arr);
        Console.WriteLine("정렬 후: " + string.Join(", ", arr));
    }
}
