using System;

// 트라이(Trie) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 많은 문자열 집합에서 빠른 검색, 자동완성, 접두사 판별 등에 사용
// - 예시: 사전 검색, 자동완성, 금지어 필터링, 문자열 집합 관리 등

class TrieNode
{
    public TrieNode[] Children = new TrieNode[26]; // 알파벳 소문자(a~z) 자식 노드 배열
    public bool IsEnd; // 단어의 끝 여부
}

class TrieExample
{
    // Trie의 루트 노드
    TrieNode root = new TrieNode();
    TrieNode root = new TrieNode();
    // 문자열 삽입 함수
    public void Insert(string word)
    {
        var node = root;
        foreach (char c in word)
        {
            int idx = c - 'a'; // 알파벳 인덱스 계산
            if (node.Children[idx] == null) node.Children[idx] = new TrieNode(); // 없으면 새로 생성
            node = node.Children[idx];
        }
        node.IsEnd = true; // 단어 끝 표시
    }
    // 문자열 검색 함수
    public bool Search(string word)
    {
        var node = root;
        foreach (char c in word)
        {
            int idx = c - 'a'; // 알파벳 인덱스 계산
            if (node.Children[idx] == null) return false; // 없으면 미존재
            node = node.Children[idx];
        }
        return node.IsEnd; // 단어 끝이면 true
    }
    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        var trie = new TrieExample();
        trie.Insert("hello"); // "hello" 삽입
        trie.Insert("world"); // "world" 삽입
        Console.WriteLine(trie.Search("hello")); // True (존재)
        Console.WriteLine(trie.Search("hell")); // False (미존재)
    }
}
