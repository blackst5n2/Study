using System;

// 분할 정복(Divide and Conquer) 거듭제곱 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 큰 문제를 쪼개서(분할) 각각 해결한 뒤(정복) 합치고 싶을 때
// - 예시: 거듭제곱, 병합 정렬, 퀵 정렬, 이진 탐색 등

class DivideAndConquer_Power
{
    // 분할 정복을 이용한 거듭제곱
    public static long Power(long x, int n)
    {
        if (n == 0) return 1;
        long half = Power(x, n / 2);
        if (n % 2 == 0) return half * half;
        else return half * half * x;
    }
    static void Main(string[] args)
    {
        long x = 2;
        int n = 10;
        Console.WriteLine($"{x}^{n} = {Power(x, n)}");
    }
}
