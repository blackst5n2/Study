using System;
using System.Collections.Generic;

// 해시/HashSet 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 빠른 원소 검색, 중복 제거, 집합 연산 등
// - 예시: 중복 체크, 원소 존재 여부, 집합 연산 등

class HashSetExample
{
    static void Main(string[] args)
    {
        HashSet<string> set = new HashSet<string>();
        set.Add("apple");
        set.Add("banana");
        set.Add("apple"); // 중복 추가 안 됨
        Console.WriteLine(set.Contains("banana") ? "banana 있음" : "banana 없음");
        foreach (var item in set)
            Console.WriteLine(item);
    }
}
