using System;
using System.Collections.Generic;

// 실전 몬스터 AI 예시 (행동 트리 기반)
//
// [설명]
// - 몬스터가 플레이어를 탐지하면 추격, 공격, 그렇지 않으면 순찰
// - 행동 트리(Sequence, Selector, Decorator 등)로 복합 행동 구현

abstract class BTNode { public abstract bool Execute(); }

// Sequence, Selector, Inverter, Repeater, Succeeder, Failer (간단 버전)
class Sequence : BTNode
{
    private List<BTNode> children = new List<BTNode>();
    public Sequence(params BTNode[] nodes) { children.AddRange(nodes); }
    public override bool Execute() { foreach (var c in children) if (!c.Execute()) return false; return true; }
}
class Selector : BTNode
{
    private List<BTNode> children = new List<BTNode>();
    public Selector(params BTNode[] nodes) { children.AddRange(nodes); }
    public override bool Execute() { foreach (var c in children) if (c.Execute()) return true; return false; }
}
class Inverter : BTNode
{
    private BTNode child;
    public Inverter(BTNode node) { child = node; }
    public override bool Execute() => !child.Execute();
}

// 행동(Leaf) 노드: 실전 상황을 시뮬레이션
class IsPlayerVisible : BTNode
{
    private Func<bool> _check;
    public IsPlayerVisible(Func<bool> check) { _check = check; }
    public override bool Execute()
    {
        bool result = _check();
        Console.WriteLine($"플레이어 탐지: {result}");
        return result;
    }
}
class IsInAttackRange : BTNode
{
    private Func<bool> _check;
    public IsInAttackRange(Func<bool> check) { _check = check; }
    public override bool Execute()
    {
        bool result = _check();
        Console.WriteLine($"공격 사거리: {result}");
        return result;
    }
}
class Patrol : BTNode { public override bool Execute() { Console.WriteLine("몬스터: 순찰"); return true; } }
class Chase : BTNode { public override bool Execute() { Console.WriteLine("몬스터: 추격"); return true; } }
class Attack : BTNode { public override bool Execute() { Console.WriteLine("몬스터: 공격!"); return true; } }

class SimpleMonsterBTMain
{
    static void Main(string[] args)
    {
        // 환경 시뮬레이션 변수
        bool playerVisible = false;
        bool inAttackRange = false;
        // 행동 트리 구성: (플레이어 탐지 → 추격 → (공격 사거리 → 공격)) 실패시 순찰
        BTNode tree = new Selector(
            new Sequence(
                new IsPlayerVisible(() => playerVisible),
                new Chase(),
                new Sequence(
                    new IsInAttackRange(() => inAttackRange),
                    new Attack()
                )
            ),
            new Patrol()
        );
        // 1. 플레이어 없음 → 순찰
        tree.Execute();
        // 2. 플레이어 발견, 사거리 밖 → 추격
        playerVisible = true;
        tree.Execute();
        // 3. 플레이어 발견, 사거리 안 → 공격
        inAttackRange = true;
        tree.Execute();
    }
}
