using System;

// 비트마스킹(Bitmasking) 부분집합 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 집합, 상태, 선택 여부를 2진수(비트)로 효율적으로 표현하고 싶을 때
// - 예시: 부분집합, 상태 DP, 조합 탐색, 비트 연산 활용 문제 등

class Bitmasking_Subset
{
    public static void PrintAllSubsets(int[] arr)
    {
        int n = arr.Length;
        int total = 1 << n;
        for (int mask = 0; mask < total; mask++)
        {
            Console.Write("{");
            for (int i = 0; i < n; i++)
                if ((mask & (1 << i)) != 0)
                    Console.Write(arr[i] + " ");
            Console.WriteLine("}");
        }
    }
    static void Main(string[] args)
    {
        int[] arr = { 1, 2, 3 };
        PrintAllSubsets(arr);
    }
}
