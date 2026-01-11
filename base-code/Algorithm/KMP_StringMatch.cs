using System;

// KMP 문자열 패턴 매칭 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 긴 문자열에서 부분 문자열(패턴)을 빠르게 찾고 싶을 때
// - 예시: 텍스트 검색, 금지어 필터, DNA 서열 분석 등

class KMP_StringMatch
{
    public static int[] BuildLps(string pattern)
    {
        int[] lps = new int[pattern.Length];
        int len = 0;
        for (int i = 1; i < pattern.Length;)
        {
            if (pattern[i] == pattern[len])
                lps[i++] = ++len;
            else if (len != 0)
                len = lps[len - 1];
            else
                lps[i++] = 0;
        }
        return lps;
    }

    public static int KMPSearch(string text, string pattern)
    {
        int[] lps = BuildLps(pattern);
        int i = 0, j = 0;
        while (i < text.Length)
        {
            if (text[i] == pattern[j])
            {
                i++; j++;
                if (j == pattern.Length) return i - j;
            }
            else if (j != 0)
                j = lps[j - 1];
            else
                i++;
        }
        return -1;
    }

    static void Main(string[] args)
    {
        string text = "abxabcabcaby";
        string pattern = "abcaby";
        int idx = KMPSearch(text, pattern);
        if (idx != -1)
            Console.WriteLine($"패턴이 {idx}번 인덱스에서 시작합니다.");
        else
            Console.WriteLine("패턴이 없습니다.");
    }
}
