using System;

// 상태(State) 패턴 예제
//
// [게임 개발에서 언제 사용?]
// - 캐릭터, 몬스터 등 오브젝트의 상태(이동, 점프, 공격 등)에 따라 동작이 달라질 때
// - 상태 전환이 자주 일어나고, 상태별로 행위가 다를 때
//
// [특징]
// - 상태별 클래스로 분리, 상태 전환을 객체로 관리

interface IState { void Handle(); }

class IdleState : IState { public void Handle() => Console.WriteLine("대기 상태"); }
class MoveState : IState { public void Handle() => Console.WriteLine("이동 상태"); }
class AttackState : IState { public void Handle() => Console.WriteLine("공격 상태"); }

class Character
{
    private IState state;
    public void SetState(IState s) { state = s; }
    public void Action() => state.Handle();
}

class StatePatternMain
{
    static void Main(string[] args)
    {
        var character = new Character();
        character.SetState(new IdleState()); character.Action(); // 대기 상태
        character.SetState(new MoveState()); character.Action(); // 이동 상태
        character.SetState(new AttackState()); character.Action(); // 공격 상태
    }
}
