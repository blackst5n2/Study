using System;
using System.Collections.Generic;

// 컴포넌트(Component) 패턴 예제 (Entity-Component-System의 핵심)
//
// [게임 개발에서 언제 사용?]
// - 다양한 기능(컴포넌트)을 조합해 유연하게 게임 오브젝트를 구성할 때
// - 상속 대신 조합(합성)으로 유연한 구조를 만들고 싶을 때
//
// [특징]
// - 각 기능을 별도 클래스로 분리, 런타임에 동적으로 추가/제거

// 컴포넌트 인터페이스
interface IComponent { void Update(); }

// 구체 컴포넌트
class MoveComponent : IComponent { public void Update() => Console.WriteLine("이동!"); }
class RenderComponent : IComponent { public void Update() => Console.WriteLine("렌더링!"); }

// 게임 오브젝트
class GameObject
{
    private List<IComponent> components = new List<IComponent>();
    public void AddComponent(IComponent comp) => components.Add(comp);
    public void Update()
    {
        foreach (var comp in components) comp.Update();
    }
}

// 사용 예시
class ComponentPatternMain
{
    static void Main(string[] args)
    {
        var player = new GameObject();
        player.AddComponent(new MoveComponent());
        player.AddComponent(new RenderComponent());
        player.Update(); // 이동! 렌더링!
    }
}
