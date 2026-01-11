using System;
using System.Collections.Generic;

// 다익스트라(Dijkstra) 최단 경로 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 가중치가 양수인 그래프에서 한 정점에서 모든 정점까지의 최단 경로
// - 예시: 지도 길찾기, 네비게이션, 네트워크 패킷 전달 등

class ShortestPath_Dijkstra
{
    public class Edge { public int To, Weight; public Edge(int to, int w) { To = to; Weight = w; } }
    public static int[] Dijkstra(List<Edge>[] graph, int start)
    {
        int n = graph.Length;
        int[] dist = new int[n];
        for (int i = 0; i < n; i++) dist[i] = int.MaxValue;
        dist[start] = 0;
        var pq = new SortedSet<(int d, int v)>(Comparer<(int,int)>.Create((a,b)=>a.d==b.d?a.v-b.v:a.d-b.d));
        pq.Add((0, start));
        while (pq.Count > 0)
        {
            var (d, v) = pq.Min; pq.Remove(pq.Min);
            if (d > dist[v]) continue;
            foreach (var e in graph[v])
            {
                if (dist[e.To] > dist[v] + e.Weight)
                {
                    dist[e.To] = dist[v] + e.Weight;
                    pq.Add((dist[e.To], e.To));
                }
            }
        }
        return dist;
    }
    static void Main(string[] args)
    {
        int n = 5;
        var graph = new List<Edge>[n];
        for (int i = 0; i < n; i++) graph[i] = new List<Edge>();
        graph[0].Add(new Edge(1, 10)); graph[0].Add(new Edge(2, 3));
        graph[2].Add(new Edge(1, 1)); graph[2].Add(new Edge(3, 2));
        graph[1].Add(new Edge(3, 2)); graph[3].Add(new Edge(4, 7));
        int[] dist = Dijkstra(graph, 0);
        Console.WriteLine("0번에서 각 정점까지 최단 거리: " + string.Join(", ", dist));
    }
}
