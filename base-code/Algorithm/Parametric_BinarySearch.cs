using System;

// 파라메트릭 서치(Parametric Search) 이분 탐색 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 어떤 조건을 만족하는 최적의 값(최소/최대)을 빠르게 찾고 싶을 때
// - 예시: 최소 최대화, 예산 배분, 작업 분할, 최적화 문제 등

class Parametric_BinarySearch
{
    // 예시: 최소 가능한 최대값(배열 분할)
    public static int MinimizeMaxSum(int[] arr, int k)
    {
        int left = 0, right = 0;
        foreach (var a in arr) if (a > left) left = a; right += a;
        while (left < right)
        {
            int mid = (left + right) / 2;
            if (CanDivide(arr, k, mid)) right = mid;
            else left = mid + 1;
        }
        return left;
    }
    static bool CanDivide(int[] arr, int k, int maxSum)
    {
        int cnt = 1, sum = 0;
        foreach (var a in arr)
        {
            if (sum + a > maxSum)
            {
                cnt++;
                sum = 0;
            }
            sum += a;
        }
        return cnt <= k;
    }
    static void Main(string[] args)
    {
        int[] arr = { 7, 2, 5, 10, 8 };
        int k = 2;
        Console.WriteLine($"최소 가능한 최대 구간합: {MinimizeMaxSum(arr, k)}");
    }
}
