using System;
using System.Collections.Generic;

// 최소 신장 트리(MST, Kruskal) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 그래프에서 모든 정점을 최소 비용으로 연결하고 싶을 때
// - 네트워크 설계, 도로/전선 연결, 비용 최소화 등
// - 예시: 통신망 구축, 전력망 설계, MST 관련 문제 등

class MST_Kruskal
{
    public class Edge : IComparable<Edge>
    {
        public int U, V, W;
        public Edge(int u, int v, int w) { U = u; V = v; W = w; }
        public int CompareTo(Edge other) => W.CompareTo(other.W);
    }

    static int[] parent;
    static int Find(int x) => parent[x] == x ? x : parent[x] = Find(parent[x]);
    static void Union(int x, int y) { parent[Find(x)] = Find(y); }

    public static int Kruskal(int n, List<Edge> edges)
    {
        edges.Sort();
        parent = new int[n];
        for (int i = 0; i < n; i++) parent[i] = i;
        int mst = 0, cnt = 0;
        foreach (var e in edges)
        {
            if (Find(e.U) != Find(e.V))
            {
                Union(e.U, e.V);
                mst += e.W;
                cnt++;
                if (cnt == n - 1) break;
            }
        }
        return mst;
    }

    static void Main(string[] args)
    {
        int n = 4;
        var edges = new List<Edge> {
            new Edge(0,1,1), new Edge(0,2,3), new Edge(1,2,1), new Edge(1,3,4), new Edge(2,3,1)
        };
        int result = Kruskal(n, edges);
        Console.WriteLine($"최소 신장 트리 가중치 합: {result}");
    }
}
