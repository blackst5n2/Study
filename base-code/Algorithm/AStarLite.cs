using System;
using System.Collections.Generic;

// A* Lite (간소화 A*) 알고리즘 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 기본 A*의 구조를 최대한 단순화하여 구현하고 싶을 때
// - 휴리스틱 기반 탐색의 개념만 빠르게 실습하고 싶을 때
// - 예시: 교육용, 알고리즘 원리 학습, 빠른 프로토타입 등

class AStarLiteExample
{
    // 2차원 격자에서 A* Lite 예시 (open 리스트를 우선순위 큐 대신 리스트+정렬 사용)
    public class Node
    {
        public int X, Y, G, H;
        public Node Parent;
        public int F => G + H;
        public Node(int x, int y, int g, int h, Node parent)
        {
            X = x; Y = y; G = g; H = h; Parent = parent;
        }
    }

    static readonly int[] dx = { -1, 1, 0, 0 };
    static readonly int[] dy = { 0, 0, -1, 1 };

    static int Heuristic(int x, int y, int tx, int ty) => Math.Abs(x - tx) + Math.Abs(y - ty);

    // A* Lite 알고리즘 함수
    public static List<(int, int)> AStarLite(int[,] map, (int, int) start, (int, int) goal)
    {
        int n = map.GetLength(0), m = map.GetLength(1);
        var open = new List<Node>();
        var closed = new HashSet<(int, int)>();
        open.Add(new Node(start.Item1, start.Item2, 0, Heuristic(start.Item1, start.Item2, goal.Item1, goal.Item2), null));
        while (open.Count > 0)
        {
            open.Sort((a, b) => a.F.CompareTo(b.F)); // F값 기준 정렬
            var current = open[0];
            open.RemoveAt(0);
            if ((current.X, current.Y) == goal)
            {
                var path = new List<(int, int)>();
                while (current != null)
                {
                    path.Add((current.X, current.Y));
                    current = current.Parent;
                }
                path.Reverse();
                return path;
            }
            closed.Add((current.X, current.Y));
            for (int d = 0; d < 4; d++)
            {
                int nx = current.X + dx[d], ny = current.Y + dy[d];
                if (nx < 0 || ny < 0 || nx >= n || ny >= m) continue;
                if (map[nx, ny] == 1) continue;
                if (closed.Contains((nx, ny))) continue;
                int g = current.G + 1;
                int h = Heuristic(nx, ny, goal.Item1, goal.Item2);
                var neighbor = new Node(nx, ny, g, h, current);
                var exist = open.Find(n => n.X == nx && n.Y == ny);
                if (exist == null || g < exist.G)
                {
                    if (exist != null) open.Remove(exist);
                    open.Add(neighbor);
                }
            }
        }
        return null;
    }

    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[,] map = {
            {0,0,0,0,0},
            {1,1,0,1,0},
            {0,0,0,1,0},
            {0,1,1,1,0},
            {0,0,0,0,0}
        };
        var path = AStarLite(map, (0,0), (4,4));
        if (path != null)
            Console.WriteLine("경로: " + string.Join(" -> ", path));
        else
            Console.WriteLine("경로 없음");
    }
}
