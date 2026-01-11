using System;
using System.Collections.Generic;

// 실전 몬스터 AI 예시 (FSM 기반)
//
// [설명]
// - 몬스터가 Idle, Patrol, Chase, Attack 상태를 가짐
// - 플레이어와의 거리, 체력 등 조건에 따라 상태 전이
// - FSM(상태 전이 테이블)로 구현

enum MonsterState { Idle, Patrol, Chase, Attack, Dead }

class MonsterAI_FSM
{
    private MonsterState state = MonsterState.Idle;
    private Dictionary<(MonsterState, string), MonsterState> transitions = new Dictionary<(MonsterState, string), MonsterState>
    {
        {(MonsterState.Idle, "SeePlayer"), MonsterState.Chase},
        {(MonsterState.Patrol, "SeePlayer"), MonsterState.Chase},
        {(MonsterState.Chase, "LostPlayer"), MonsterState.Patrol},
        {(MonsterState.Chase, "InAttackRange"), MonsterState.Attack},
        {(MonsterState.Attack, "PlayerDead"), MonsterState.Patrol},
        {(MonsterState.Attack, "LowHP"), MonsterState.Dead},
    };
    public void HandleEvent(string trigger)
    {
        if (transitions.TryGetValue((state, trigger), out var next))
        {
            Console.WriteLine($"상태 전이: {state} -> {next} (트리거: {trigger})");
            state = next;
        }
        else
        {
            Console.WriteLine($"상태 유지: {state} (트리거: {trigger})");
        }
    }
    public void Update()
    {
        // 상태별 행동 예시
        switch (state)
        {
            case MonsterState.Idle: Console.WriteLine("몬스터: 대기 중"); break;
            case MonsterState.Patrol: Console.WriteLine("몬스터: 순찰 중"); break;
            case MonsterState.Chase: Console.WriteLine("몬스터: 플레이어 추격!"); break;
            case MonsterState.Attack: Console.WriteLine("몬스터: 공격!"); break;
            case MonsterState.Dead: Console.WriteLine("몬스터: 사망"); break;
        }
    }
}

class SimpleMonsterAIMain
{
    static void Main(string[] args)
    {
        var ai = new MonsterAI_FSM();
        ai.Update();
        ai.HandleEvent("SeePlayer"); ai.Update();
        ai.HandleEvent("InAttackRange"); ai.Update();
        ai.HandleEvent("LowHP"); ai.Update();
    }
}
