using System;

// 동적 프로그래밍(DP) 피보나치 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 동일한 부분문제가 반복되는 경우(중복 호출 최소화)
// - 예시: 피보나치 수, 계단 오르기, 최소 비용 경로, 배낭 문제 등

class DynamicProgrammingFibonacci
{
    // 동적 프로그래밍(DP) 피보나치 예제 클래스
    // DP(바텀업) 방식의 피보나치 수열
    // DP(바텀업) 방식의 피보나치 수열 계산 함수
    public static int Fibonacci(int n)
    {
        if (n <= 1) return n;
        int[] dp = new int[n + 1]; // dp[i]: i번째 피보나치 수
        dp[0] = 0;
        dp[1] = 1;
        for (int i = 2; i <= n; i++)
            dp[i] = dp[i - 1] + dp[i - 2]; // 점화식 적용
        return dp[n];
    }

    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int n = 10; // 구할 피보나치 수
        Console.WriteLine($"Fibonacci({n}) = {Fibonacci(n)}");
    }
}
