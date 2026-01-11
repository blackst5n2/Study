using System;
using System.Collections.Generic;

// 행동 트리(Behaviour Tree) 패턴 예제
//
// [게임 개발에서 언제 사용?]
// - 복잡한 AI 행동(순차, 선택, 반복 등)을 계층적으로 표현하고 싶을 때
// - 몬스터, NPC, 보스 AI 등 다양한 행동 조합이 필요한 경우
//
// [특징]
// - 노드(Composite, Decorator, Leaf)로 트리 구조
// - Sequence(순차), Selector(선택), Action(행동) 등으로 조합

// 트리 노드 추상 클래스
abstract class BTNode { public abstract bool Execute(); }

// 순차(Sequence) 노드: 모든 자식이 성공해야 성공
class Sequence : BTNode
{
    private List<BTNode> children = new List<BTNode>();
    public Sequence(params BTNode[] nodes) { children.AddRange(nodes); }
    public override bool Execute()
    {
        foreach (var child in children)
            if (!child.Execute()) return false;
        return true;
    }
}

// 선택(Selector) 노드: 하나라도 성공하면 성공
class Selector : BTNode
{
    private List<BTNode> children = new List<BTNode>();
    public Selector(params BTNode[] nodes) { children.AddRange(nodes); }
    public override bool Execute()
    {
        foreach (var child in children)
            if (child.Execute()) return true;
        return false;
    }
}

// 데코레이터(Decorator) 노드: 자식 노드의 실행 결과를 변형
class Inverter : BTNode // 성공 <-> 실패 반전
{
    private BTNode child;
    public Inverter(BTNode node) { child = node; }
    public override bool Execute() => !child.Execute();
}

class Repeater : BTNode // 지정 횟수만큼 자식 반복
{
    private BTNode child;
    private int count;
    public Repeater(BTNode node, int n) { child = node; count = n; }
    public override bool Execute()
    {
        bool lastResult = false;
        for (int i = 0; i < count; i++)
            lastResult = child.Execute();
        return lastResult;
    }
}

class Succeeder : BTNode // 항상 성공 반환
{
    private BTNode child;
    public Succeeder(BTNode node) { child = node; }
    public override bool Execute() { child.Execute(); return true; }
}

class Failer : BTNode // 항상 실패 반환
{
    private BTNode child;
    public Failer(BTNode node) { child = node; }
    public override bool Execute() { child.Execute(); return false; }
}

// Leaf(행동) 노드 예시
class FindEnemy : BTNode { public override bool Execute() { Console.WriteLine("적 탐색"); return true; } }
class Attack : BTNode { public override bool Execute() { Console.WriteLine("공격!"); return true; } }
class Patrol : BTNode { public override bool Execute() { Console.WriteLine("순찰"); return false; } }

class BehaviourTreeMain
{
    static void Main(string[] args)
    {
        // 행동 트리: (적 탐색 → 공격) 실패시 순찰
        BTNode tree = new Selector(
            new Sequence(
                new FindEnemy(),
                new Attack()
            ),
            new Patrol()
        );
        tree.Execute();
        // 출력: 적 탐색 → 공격! (만약 실패시 순찰)

        // 고급 노드 예시: Inverter, Repeater, Succeeder, Failer
        Console.WriteLine("--- Inverter(반전) 테스트 ---");
        BTNode inverterTest = new Inverter(new Patrol()); // Patrol은 false → Inverter로 true
        inverterTest.Execute();

        Console.WriteLine("--- Repeater(3회 반복) 테스트 ---");
        BTNode repeaterTest = new Repeater(new Attack(), 3); // Attack 3회 실행
        repeaterTest.Execute();

        Console.WriteLine("--- Succeeder(항상 성공) 테스트 ---");
        BTNode succeederTest = new Succeeder(new Patrol()); // Patrol은 false지만 항상 true 반환
        succeederTest.Execute();

        Console.WriteLine("--- Failer(항상 실패) 테스트 ---");
        BTNode failerTest = new Failer(new Attack()); // Attack은 true지만 항상 false 반환
        failerTest.Execute();
    }
}
