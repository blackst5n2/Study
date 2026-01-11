using System;

// 세그먼트 트리(Segment Tree) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 구간(부분 배열)의 합/최솟값/최댓값 등 쿼리를 빠르게 처리하고 싶을 때
// - 배열에 대한 동적 쿼리, 구간 갱신, 구간 질의가 반복될 때
// - 예시: 구간 합, RMQ, 구간 업데이트, 온라인 쿼리 등

class SegmentTree
{
    // 세그먼트 트리 예제 클래스
    // 트리 배열, 원본 배열, 배열 크기
    int[] tree, arr;
    int n;
    // 생성자: 입력 배열로 세그먼트 트리 초기화
    public SegmentTree(int[] input)
    {
        n = input.Length;
        arr = input;
        tree = new int[4 * n]; // 트리 배열 크기 충분히 할당
        Build(1, 0, n - 1);
    }
    // 트리 구축 함수 (구간 최소값 저장)
    void Build(int node, int s, int e)
    {
        if (s == e) tree[node] = arr[s]; // 리프 노드
        else
        {
            int m = (s + e) / 2;
            Build(node * 2, s, m); // 왼쪽 자식
            Build(node * 2 + 1, m + 1, e); // 오른쪽 자식
            tree[node] = Math.Min(tree[node * 2], tree[node * 2 + 1]); // 자식의 최소값
        }
    }
    // 구간 [l, r]의 최소값 쿼리 함수
    public int Query(int l, int r) => Query(1, 0, n - 1, l, r);
    int Query(int node, int s, int e, int l, int r)
    {
        // 쿼리 구간과 노드 구간이 겹치지 않으면 무한대 반환
        if (r < s || e < l) return int.MaxValue;
        // 쿼리 구간이 노드 구간을 완전히 포함하면 해당 노드 값 반환
        if (l <= s && e <= r) return tree[node];
        int m = (s + e) / 2;
        // 왼쪽/오른쪽 자식 쿼리 결과의 최소값 반환
        return Math.Min(Query(node * 2, s, m, l, r), Query(node * 2 + 1, m + 1, e, l, r));
    }
    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[] arr = { 5, 2, 4, 3, 1, 7 }; // 입력 배열
        var st = new SegmentTree(arr); // 세그먼트 트리 생성
        Console.WriteLine($"[1,4] 구간 최소값: {st.Query(1,4)}"); // [1,4] 구간 최소값 쿼리
    }
}
