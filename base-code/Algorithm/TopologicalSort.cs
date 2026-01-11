using System;
using System.Collections.Generic;

// 위상 정렬(Topological Sort) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 순서가 정해진 작업(선행 관계, 작업 스케줄링, 강의 선수과목 등)이 있을 때 사용
// - 방향 비순환 그래프(DAG)에서만 적용 가능
// - 예시: 컴파일 의존성, 과목 이수, 작업 선행 관계, 빌드 시스템 등

class TopologicalSort
{
    // 위상 정렬 함수(Kahn's Algorithm)
    // n: 정점 개수, adj: 인접 리스트(방향 그래프)
    public static List<int> Sort(int n, List<int>[] adj)
    {
        int[] indegree = new int[n]; // 각 정점의 진입차수
        foreach (var list in adj)
            foreach (var v in list)
                indegree[v]++; // 진입차수 계산
        Queue<int> q = new Queue<int>(); // 진입차수 0인 정점 큐
        for (int i = 0; i < n; i++) if (indegree[i] == 0) q.Enqueue(i);
        List<int> order = new List<int>(); // 위상 정렬 결과
        while (q.Count > 0)
        {
            int u = q.Dequeue(); // 진입차수 0인 정점 꺼내기
            order.Add(u);
            foreach (var v in adj[u])
                if (--indegree[v] == 0) q.Enqueue(v); // 연결된 정점의 진입차수 감소, 0이면 큐에 추가
        }
        // 모든 정점을 방문했으면 결과 반환, 아니면 순환(사이클) 존재
        return order.Count == n ? order : null;
    }
    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int n = 6; // 정점 개수
        var adj = new List<int>[n]; // 인접 리스트
        for (int i = 0; i < n; i++) adj[i] = new List<int>();
        // 방향 간선 추가 (예: 선수과목 관계)
        adj[5].Add(2); adj[5].Add(0);
        adj[4].Add(0); adj[4].Add(1);
        adj[2].Add(3); adj[3].Add(1);
        var result = Sort(n, adj); // 위상 정렬 호출
        if (result != null)
            Console.WriteLine("위상 정렬 결과: " + string.Join(", ", result));
        else
            Console.WriteLine("순환이 있어 위상 정렬 불가");
    }
}
