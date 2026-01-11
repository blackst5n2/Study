using System;

// 그리디(Greedy) 동전 거스름돈 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 항상 가장 좋은(최적) 선택이 전체적으로도 최적인 경우
// - 예시: 동전 거스름돈, 회의실 배정, 활동 선택, 배낭 문제(특정 조건) 등

class GreedyCoinChange
{
    // 그리디 동전 거스름돈 예제 클래스
    // 거스름돈 문제 (그리디)
    // 동전 거스름돈 문제: 최소 동전 개수 구하기 (그리디)
    public static int CoinChange(int[] coins, int amount)
    {
        Array.Sort(coins);
        Array.Reverse(coins); // 큰 동전부터 사용 (내림차순)
        int count = 0;
        foreach (int coin in coins)
        {
            if (amount >= coin)
            {
                count += amount / coin; // 해당 동전 최대 사용
                amount %= coin; // 남은 금액 갱신
            }
        }
        return amount == 0 ? count : -1; // 거슬러줄 수 없으면 -1 반환
    }

    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[] coins = { 500, 100, 50, 10 }; // 동전 종류
        int amount = 1260; // 거슬러 줄 금액
        int result = CoinChange(coins, amount); // 그리디 알고리즘 호출
        if (result != -1)
            Console.WriteLine($"필요한 동전 개수: {result}");
        else
            Console.WriteLine("거슬러줄 수 없습니다.");
    }
}
