using System;

// 전략(Strategy) 패턴 예제
//
// [언제 사용하면 좋은가?]
// - 알고리즘(로직)을 런타임에 바꿔 끼우고 싶을 때
// - 다양한 정책/전략을 동적으로 적용해야 할 때
//
// [특징]
// - 인터페이스(추상) 기반, 실행 중 알고리즘 교체 가능

// 전략 인터페이스
interface IStrategy { void Execute(); }

// 구체 전략
class StrategyA : IStrategy { public void Execute() => Console.WriteLine("전략 A 실행"); }
class StrategyB : IStrategy { public void Execute() => Console.WriteLine("전략 B 실행"); }

// Context
class Context
{
    private IStrategy _strategy;
    public Context(IStrategy strategy) { _strategy = strategy; }
    public void SetStrategy(IStrategy strategy) { _strategy = strategy; }
    public void DoWork() => _strategy.Execute();
}

// 사용 예시
class StrategyPatternMain
{
    static void Main(string[] args)
    {
        var context = new Context(new StrategyA());
        context.DoWork(); // 전략 A 실행
        context.SetStrategy(new StrategyB());
        context.DoWork(); // 전략 B 실행
    }
}
