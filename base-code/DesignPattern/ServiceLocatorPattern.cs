using System;
using System.Collections.Generic;

// 서비스 로케이터(Service Locator) 패턴 예제
//
// [게임 개발에서 언제 사용?]
// - 전역적으로 자주 쓰는 서비스(사운드, 로깅, UI 등)를 효율적으로 관리하고 싶을 때
// - 의존성 주입(DI) 대체 또는 간단한 서비스 관리
//
// [특징]
// - 서비스 등록/조회/교체가 쉬움, 전역 접근 가능

interface IService { void Execute(); }
class SoundService : IService { public void Execute() => Console.WriteLine("사운드 실행"); }
class LogService : IService { public void Execute() => Console.WriteLine("로그 기록"); }

class ServiceLocator
{
    private static Dictionary<Type, IService> services = new Dictionary<Type, IService>();
    public static void Register<T>(IService service) where T : IService => services[typeof(T)] = service;
    public static T Get<T>() where T : IService => (T)services[typeof(T)];
}

class ServiceLocatorPatternMain
{
    static void Main(string[] args)
    {
        ServiceLocator.Register<SoundService>(new SoundService());
        ServiceLocator.Register<LogService>(new LogService());
        ServiceLocator.Get<SoundService>().Execute(); // 사운드 실행
        ServiceLocator.Get<LogService>().Execute();   // 로그 기록
    }
}
