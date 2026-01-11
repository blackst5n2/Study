using System;

// 병합 정렬(Merge Sort) 예제
//
// [어떤 상황에서 사용하면 좋은가?]
// - 데이터 개수가 많고, 안정적인 O(n log n) 정렬이 필요할 때
// - 내부/외부 정렬, 대용량 데이터, 안정 정렬 요구 시
// - 예시: 대용량 파일 정렬, 데이터베이스 정렬 등

class MergeSortExample
{
    // 병합 정렬 함수
    public static void MergeSort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int mid = (left + right) / 2;
            MergeSort(arr, left, mid);
            MergeSort(arr, mid + 1, right);
            Merge(arr, left, mid, right);
        }
    }

    // 두 정렬된 부분을 병합하는 함수
    static void Merge(int[] arr, int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;
        int[] L = new int[n1];
        int[] R = new int[n2];
        for (int i = 0; i < n1; i++) L[i] = arr[left + i];
        for (int j = 0; j < n2; j++) R[j] = arr[mid + 1 + j];
        int i1 = 0, i2 = 0, k = left;
        while (i1 < n1 && i2 < n2)
        {
            if (L[i1] <= R[i2]) arr[k++] = L[i1++];
            else arr[k++] = R[i2++];
        }
        while (i1 < n1) arr[k++] = L[i1++];
        while (i2 < n2) arr[k++] = R[i2++];
    }

    // 프로그램 실행 예시(Main 함수)
    static void Main(string[] args)
    {
        int[] arr = { 8, 3, 2, 7, 4, 6, 5 };
        Console.WriteLine("정렬 전: " + string.Join(", ", arr));
        MergeSort(arr, 0, arr.Length - 1);
        Console.WriteLine("정렬 후: " + string.Join(", ", arr));
    }
}
