using System;
using System.Collections.Generic;

// 유한 상태 기계(Finite State Machine, FSM) 패턴 예제
//
// [게임 개발에서 언제 사용?]
// - 캐릭터 AI, 몬스터 행동, UI 상태 등 명확한 상태 전환이 필요한 경우
// - 상태와 이벤트(입력)에 따라 행동이 달라질 때
//
// [특징]
// - 상태(State), 이벤트(Event), 전이(Transition) 명확히 분리
// - 상태 전이 테이블 또는 switch-case로 구현

enum State { Idle, Move, Attack }
enum Event { SeeEnemy, LoseEnemy, ReachEnemy }

class FSM
{
    private State state = State.Idle;
    private Dictionary<(State, Event), State> transitions = new Dictionary<(State, Event), State>
    {
        {(State.Idle, Event.SeeEnemy), State.Move},
        {(State.Move, Event.ReachEnemy), State.Attack},
        {(State.Attack, Event.LoseEnemy), State.Idle},
    };
    public void HandleEvent(Event e)
    {
        if (transitions.TryGetValue((state, e), out var next))
        {
            Console.WriteLine($"상태 전이: {state} -> {next} (이벤트: {e})");
            state = next;
        }
        else
        {
            Console.WriteLine($"상태 유지: {state} (이벤트: {e})");
        }
    }
}

class FSMMain
{
    static void Main(string[] args)
    {
        var fsm = new FSM();
        fsm.HandleEvent(Event.SeeEnemy);    // Idle -> Move
        fsm.HandleEvent(Event.ReachEnemy);  // Move -> Attack
        fsm.HandleEvent(Event.LoseEnemy);   // Attack -> Idle
    }
}
