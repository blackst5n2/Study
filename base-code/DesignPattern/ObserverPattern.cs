using System;
using System.Collections.Generic;

// 옵저버(Observer) 패턴 예제
//
// [언제 사용하면 좋은가?]
// - 한 객체의 상태 변화에 따라 여러 객체가 자동으로 알림을 받아야 할 때
// - GUI, 이벤트 시스템, 데이터 변경 알림 등
//
// [특징]
// - 주체(Subject)와 구독자(Observer) 분리, 느슨한 결합

// Observer 인터페이스
interface IObserver { void Update(string msg); }

// Concrete Observer
class ConcreteObserver : IObserver
{
    private string _name;
    public ConcreteObserver(string name) { _name = name; }
    public void Update(string msg) => Console.WriteLine($"{_name} 알림: {msg}");
}

// Subject
class Subject
{
    private List<IObserver> observers = new List<IObserver>();
    public void Attach(IObserver observer) => observers.Add(observer);
    public void Detach(IObserver observer) => observers.Remove(observer);
    public void Notify(string msg)
    {
        foreach (var obs in observers) obs.Update(msg);
    }
}

// 사용 예시
class ObserverPatternMain
{
    static void Main(string[] args)
    {
        var subject = new Subject();
        var o1 = new ConcreteObserver("A");
        var o2 = new ConcreteObserver("B");
        subject.Attach(o1);
        subject.Attach(o2);
        subject.Notify("데이터 변경!");
        subject.Detach(o1);
        subject.Notify("다시 변경!");
    }
}
