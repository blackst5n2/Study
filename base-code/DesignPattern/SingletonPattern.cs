using System;

// 싱글톤(Singleton) 패턴 예제
//
// [언제 사용하면 좋은가?]
// - 프로그램 전체에서 단 하나의 인스턴스만 허용하고 싶을 때
// - 전역 설정, 로그, DB 연결 등
//
// [특징]
// - 생성자를 private으로 막고, 정적 메서드로 인스턴스 반환
// - 멀티스레드 환경에서는 동기화 필요

class Singleton
{
    private static Singleton _instance;
    private static readonly object _lock = new object();
    // 외부에서 직접 생성 불가
    private Singleton() { }
    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Singleton();
                }
            }
            return _instance;
        }
    }
    public void DoSomething() => Console.WriteLine("싱글톤 동작!");

    // 사용 예시
    static void Main(string[] args)
    {
        var s1 = Singleton.Instance;
        var s2 = Singleton.Instance;
        Console.WriteLine(s1 == s2); // True
        s1.DoSomething();
    }
}
