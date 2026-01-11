using System;
using System.Collections.Generic;

// 커맨드(Command) 패턴 예제
//
// [게임 개발에서 언제 사용?]
// - 플레이어 입력, 매크로, 실행 취소(Undo), 재실행(Redo) 등 명령을 객체로 캡슐화할 때
// - 입력 처리, UI 버튼, 행동 기록 등
//
// [특징]
// - 요청(명령)을 객체로 캡슐화, 실행/취소/재실행 관리

interface ICommand { void Execute(); }

class JumpCommand : ICommand { public void Execute() => Console.WriteLine("점프!"); }
class ShootCommand : ICommand { public void Execute() => Console.WriteLine("총 발사!"); }

class InputHandler
{
    private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();
    public void SetCommand(string key, ICommand cmd) => commands[key] = cmd;
    public void HandleInput(string key) { if (commands.ContainsKey(key)) commands[key].Execute(); }
}

class CommandPatternMain
{
    static void Main(string[] args)
    {
        var handler = new InputHandler();
        handler.SetCommand("Space", new JumpCommand());
        handler.SetCommand("Ctrl", new ShootCommand());
        handler.HandleInput("Space"); // 점프!
        handler.HandleInput("Ctrl");  // 총 발사!
    }
}
