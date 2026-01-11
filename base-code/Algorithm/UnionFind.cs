using System;

// 유니온 파인드(Disjoint Set, Union-Find) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 여러 원소가 같은 집합(그룹)에 속하는지 빠르게 판별하고 싶을 때
// - 집합의 합치기, 분리, 연결성 판별 등
// - 예시: 네트워크 연결, 최소 신장 트리(MST), 사이클 판별 등

class UnionFind
{
    private int[] parent;

    public UnionFind(int size)
    {
        parent = new int[size];
        for (int i = 0; i < size; i++)
            parent[i] = i;
    }

    public int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]); // 경로 압축
        return parent[x];
    }

    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);
        if (rootX != rootY)
            parent[rootY] = rootX;
    }

    static void Main(string[] args)
    {
        UnionFind uf = new UnionFind(5);
        uf.Union(0, 1);
        uf.Union(3, 4);
        uf.Union(1, 4);
        Console.WriteLine(uf.Find(0) == uf.Find(4) ? "0과 4는 같은 집합입니다." : "0과 4는 다른 집합입니다.");
        Console.WriteLine(uf.Find(2) == uf.Find(4) ? "2와 4는 같은 집합입니다." : "2와 4는 다른 집합입니다.");
    }
}
