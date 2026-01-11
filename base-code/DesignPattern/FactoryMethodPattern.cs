using System;

// 팩토리 메서드(Factory Method) 패턴 예제
//
// [언제 사용하면 좋은가?]
// - 객체 생성 로직을 서브클래스에 위임하고 싶을 때
// - 객체 생성 구조가 자주 바뀌거나, 확장에 유연함이 필요할 때
//
// [특징]
// - 인터페이스/추상 클래스 기반, 객체 생성 책임 분리

// Product 인터페이스
interface IProduct { void DoWork(); }

// ConcreteProductA/B
class ProductA : IProduct { public void DoWork() => Console.WriteLine("ProductA 작업"); }
class ProductB : IProduct { public void DoWork() => Console.WriteLine("ProductB 작업"); }

// Creator 추상 클래스
abstract class Creator
{
    public abstract IProduct FactoryMethod();
    public void SomeOperation() => FactoryMethod().DoWork();
}

// ConcreteCreatorA/B
class CreatorA : Creator { public override IProduct FactoryMethod() => new ProductA(); }
class CreatorB : Creator { public override IProduct FactoryMethod() => new ProductB(); }

// 사용 예시
class FactoryMethodMain
{
    static void Main(string[] args)
    {
        Creator creator = new CreatorA();
        creator.SomeOperation(); // ProductA 작업
        creator = new CreatorB();
        creator.SomeOperation(); // ProductB 작업
    }
}
