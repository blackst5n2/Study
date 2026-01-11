using System;

// 슬라이딩 윈도우(Sliding Window) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 연속된 구간(부분 배열/문자열)에서 합, 최대/최소, 조건 만족 구간 등 효율적으로 구할 때
// - 투 포인터와 함께 자주 사용
// - 예시: 최대 부분합, 연속된 조건 구간, 문자열 패턴 탐색 등

class SlidingWindow_MaxSum
{
    // 고정 길이 k의 부분배열 최대합
    // 고정 길이 k의 부분배열 최대합을 구하는 함수
    public static int MaxSum(int[] arr, int k)
    {
        int n = arr.Length, sum = 0, maxSum = int.MinValue;
        // 첫 윈도우 합 계산
        for (int i = 0; i < k; i++) sum += arr[i];
        maxSum = sum;
        // 윈도우를 한 칸씩 이동하며 최대합 갱신
        for (int i = k; i < n; i++)
        {
            sum += arr[i] - arr[i - k]; // 새 원소 추가, 맨 앞 원소 제거
            if (sum > maxSum) maxSum = sum;
        }
        return maxSum;
    }
    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[] arr = { 2, 1, 5, 1, 3, 2 }; // 입력 배열
        int k = 3; // 부분 배열 길이
        Console.WriteLine($"길이 {k}의 부분배열 최대합: {MaxSum(arr, k)}");
    }
}
