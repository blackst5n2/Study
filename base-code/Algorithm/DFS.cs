using System;
using System.Collections.Generic;

// DFS(깊이 우선 탐색) 예제
// [어떤 상황에서 사용하면 좋은가?]
// - 그래프, 트리 구조에서 모든 정점(노드)을 방문하고 싶을 때
// - 연결성 판단, 미로 탐색, 컴포넌트 분리 등
// - 예시: 미로 탈출, 네트워크 연결, 섬 개수 세기, 경로 찾기 등

class DFSExample
{
    private List<int>[] adj;
    private int V;

    public DFSExample(int vertices)
    {
        V = vertices;
        adj = new List<int>[V];
        for (int i = 0; i < V; i++)
            adj[i] = new List<int>();
    }

    public void AddEdge(int v, int w)
    {
        adj[v].Add(w);
        adj[w].Add(v); // 무방향 그래프
    }

    // DFS(깊이 우선 탐색) 함수
    public void DFS(int start)
    {
        bool[] visited = new bool[V];
        Console.Write("DFS: ");
        DFSUtil(start, visited);
        Console.WriteLine();
    }
    private void DFSUtil(int v, bool[] visited)
    {
        visited[v] = true;
        Console.Write(v + " ");
        foreach (int neighbor in adj[v])
        {
            if (!visited[neighbor])
                DFSUtil(neighbor, visited);
        }
    }

    static void Main(string[] args)
    {
        DFSExample g = new DFSExample(5);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 3);
        g.AddEdge(1, 4);
        g.DFS(0);
    }
}
