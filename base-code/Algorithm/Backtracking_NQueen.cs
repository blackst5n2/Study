using System;

// 백트래킹(Backtracking) N-Queen 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 모든 경우의 수를 탐색하되, 불필요한 가지는 미리 쳐내고 싶을 때
// - 예시: N-Queen, 순열/조합 생성, 부분집합, 미로찾기 등

class Backtracking_NQueen
{
    static int count = 0;
    public static void Solve(int n)
    {
        int[] board = new int[n];
        count = 0;
        PlaceQueen(board, 0, n);
        Console.WriteLine($"{n}x{n} N-Queen 해답 개수: {count}");
    }
    static void PlaceQueen(int[] board, int row, int n)
    {
        if (row == n) { count++; return; }
        for (int col = 0; col < n; col++)
        {
            if (IsSafe(board, row, col))
            {
                board[row] = col;
                PlaceQueen(board, row + 1, n);
            }
        }
    }
    static bool IsSafe(int[] board, int row, int col)
    {
        for (int i = 0; i < row; i++)
            if (board[i] == col || Math.Abs(board[i] - col) == row - i)
                return false;
        return true;
    }
    static void Main(string[] args)
    {
        Solve(8);
    }
}
