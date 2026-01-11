using System;

// DP(동적 프로그래밍) 최장 증가 부분 수열(LIS) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 수열에서 증가하는 부분수열의 최대 길이를 구하고 싶을 때
// - 예시: LIS, LCS, 부분수열, DP 심화 문제 등

class DP_LIS
{
    // 최장 증가 부분 수열(LIS)
    public static int LIS(int[] arr)
    {
        int n = arr.Length;
        int[] dp = new int[n];
        for (int i = 0; i < n; i++) dp[i] = 1;
        for (int i = 1; i < n; i++)
            for (int j = 0; j < i; j++)
                if (arr[j] < arr[i])
                    dp[i] = Math.Max(dp[i], dp[j] + 1);
        int max = 0;
        for (int i = 0; i < n; i++)
            if (dp[i] > max) max = dp[i];
        return max;
    }
    static void Main(string[] args)
    {
        int[] arr = { 10, 20, 10, 30, 20, 50 };
        Console.WriteLine($"LIS 길이: {LIS(arr)}");
    }
}
