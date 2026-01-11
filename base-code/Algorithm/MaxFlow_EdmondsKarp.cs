using System;
using System.Collections.Generic;

// 최대 유량(Edmonds-Karp) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 네트워크에서 최대 흐름(유량)을 구하고 싶을 때
// - 이분 매칭, 최소 컷, 네트워크 설계 등
// - 예시: 배관 네트워크, 작업 할당, 교통 흐름 등

class MaxFlow_EdmondsKarp
{
    static int[,] capacity, flow;
    static List<int>[] adj;
    static int n;
    public static int EdmondsKarp(int s, int t)
    {
        int total = 0;
        while (true)
        {
            int[] prev = new int[n];
            for (int i = 0; i < n; i++) prev[i] = -1;
            Queue<int> q = new Queue<int>();
            q.Enqueue(s);
            while (q.Count > 0 && prev[t] == -1)
            {
                int v = q.Dequeue();
                foreach (var u in adj[v])
                {
                    if (prev[u] == -1 && capacity[v, u] - flow[v, u] > 0)
                    {
                        prev[u] = v;
                        q.Enqueue(u);
                    }
                }
            }
            if (prev[t] == -1) break;
            int increment = int.MaxValue;
            for (int v = t; v != s; v = prev[v])
                increment = Math.Min(increment, capacity[prev[v], v] - flow[prev[v], v]);
            for (int v = t; v != s; v = prev[v])
            {
                flow[prev[v], v] += increment;
                flow[v, prev[v]] -= increment;
            }
            total += increment;
        }
        return total;
    }
    static void Main(string[] args)
    {
        n = 4;
        capacity = new int[n, n];
        flow = new int[n, n];
        adj = new List<int>[n];
        for (int i = 0; i < n; i++) adj[i] = new List<int>();
        void AddEdge(int u, int v, int c) { capacity[u,v]=c; adj[u].Add(v); adj[v].Add(u); }
        AddEdge(0,1,3); AddEdge(0,2,2); AddEdge(1,2,1); AddEdge(1,3,2); AddEdge(2,3,4);
        int maxFlow = EdmondsKarp(0,3);
        Console.WriteLine($"최대 유량: {maxFlow}");
    }
}
