using System;
using System.Collections.Generic;

// A* (A-star) 최단 경로 알고리즘 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 휴리스틱(예상 거리) 정보를 활용해 최단 경로를 빠르게 찾고 싶을 때
// - 2차원 맵, 게임 AI, 경로 탐색, 네비게이션 등
// - 예시: 게임 캐릭터 이동, 로봇 경로 탐색, 지도 길찾기 등

class AStarExample
{
    // 2차원 격자에서 A* 알고리즘 예시
    public class Node : IComparable<Node>
    {
        public int X, Y, G, H; // X, Y: 좌표 / G: 시작~현재 / H: 휴리스틱(예상)
        public Node Parent;
        public int F => G + H; // F = 실제비용 + 예상비용
        public Node(int x, int y, int g, int h, Node parent)
        {
            X = x; Y = y; G = g; H = h; Parent = parent;
        }
        public int CompareTo(Node other) => F.CompareTo(other.F);
        public override bool Equals(object obj) => obj is Node n && X == n.X && Y == n.Y;
        public override int GetHashCode() => X * 10000 + Y;
    }

    static readonly int[] dx = { -1, 1, 0, 0 };
    static readonly int[] dy = { 0, 0, -1, 1 };

    // 맨해튼 거리 휴리스틱 함수
    static int Heuristic(int x, int y, int tx, int ty) => Math.Abs(x - tx) + Math.Abs(y - ty);

    // A* 알고리즘 함수
    public static List<(int, int)> AStar(int[,] map, (int, int) start, (int, int) goal)
    {
        int n = map.GetLength(0), m = map.GetLength(1);
        var open = new SortedSet<Node>();
        var closed = new HashSet<(int, int)>();
        var startNode = new Node(start.Item1, start.Item2, 0, Heuristic(start.Item1, start.Item2, goal.Item1, goal.Item2), null);
        open.Add(startNode);
        while (open.Count > 0)
        {
            var current = open.Min;
            open.Remove(current);
            if ((current.X, current.Y) == goal)
            {
                // 경로 복원
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
                if (map[nx, ny] == 1) continue; // 벽
                if (closed.Contains((nx, ny))) continue;
                int g = current.G + 1;
                int h = Heuristic(nx, ny, goal.Item1, goal.Item2);
                var neighbor = new Node(nx, ny, g, h, current);
                if (!open.TryGetValue(neighbor, out var exist) || g < exist.G)
                {
                    open.Remove(neighbor);
                    open.Add(neighbor);
                }
            }
        }
        return null; // 경로 없음
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
        var path = AStar(map, (0,0), (4,4));
        if (path != null)
            Console.WriteLine("경로: " + string.Join(" -> ", path));
        else
            Console.WriteLine("경로 없음");
    }
}
