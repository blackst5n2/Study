using System;
using System.Collections.Generic;

// BFS(너비 우선 탐색) 예제
// [어떤 상황에서 사용하면 좋은가?]
// - 그래프, 트리 구조에서 모든 정점(노드)을 방문하고 싶을 때
// - 연결성 판단, 최단거리, 미로 탐색, 컴포넌트 분리 등
// - 예시: 미로 탈출, 네트워크 연결, 섬 개수 세기, 경로 찾기 등

class BFSExample
{
    private List<int>[] adj;
    private int V;

    public BFSExample(int vertices)
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

    // BFS(너비 우선 탐색) 함수
    public void BFS(int start)
    {
        bool[] visited = new bool[V];
        Queue<int> queue = new Queue<int>();
        visited[start] = true;
        queue.Enqueue(start);
        Console.Write("BFS: ");
        while (queue.Count > 0)
        {
            int v = queue.Dequeue();
            Console.Write(v + " ");
            foreach (int neighbor in adj[v])
            {
                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    queue.Enqueue(neighbor);
                }
            }
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        BFSExample g = new BFSExample(5);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 3);
        g.AddEdge(1, 4);
        g.BFS(0);
    }
}
