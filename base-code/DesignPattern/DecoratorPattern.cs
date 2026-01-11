using System;

// 데코레이터(Decorator) 패턴 예제
//
// [언제 사용하면 좋은가?]
// - 객체에 동적으로 새로운 기능을 추가하고 싶을 때
// - 상속 대신 조합(합성)으로 기능 확장
//
// [특징]
// - 기존 코드 변경 없이 기능 확장, 유연한 구조

// Component 인터페이스
interface IComponent { void Operation(); }

// ConcreteComponent
class ConcreteComponent : IComponent { public void Operation() => Console.WriteLine("기본 동작"); }

// Decorator 추상 클래스
abstract class Decorator : IComponent
{
    protected IComponent _component;
    public Decorator(IComponent component) { _component = component; }
    public virtual void Operation() => _component.Operation();
}

// ConcreteDecoratorA/B
class DecoratorA : Decorator
{
    public DecoratorA(IComponent component) : base(component) { }
    public override void Operation()
    {
        base.Operation();
        Console.WriteLine("+ 기능 A 추가");
    }
}

class DecoratorB : Decorator
{
    public DecoratorB(IComponent component) : base(component) { }
    public override void Operation()
    {
        base.Operation();
        Console.WriteLine("+ 기능 B 추가");
    }
}

// 사용 예시
class DecoratorPatternMain
{
    static void Main(string[] args)
    {
        IComponent comp = new ConcreteComponent();
        comp = new DecoratorA(comp);
        comp = new DecoratorB(comp);
        comp.Operation();
        // 출력: 기본 동작 -> + 기능 A 추가 -> + 기능 B 추가
    }
}
