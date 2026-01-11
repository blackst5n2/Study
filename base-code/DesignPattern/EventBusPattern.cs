using System;
using System.Collections.Generic;

// 이벤트 버스(Event Bus) 패턴 예제
//
// [게임 개발에서 언제 사용?]
// - 여러 시스템/오브젝트가 느슨하게 연결되어 이벤트로 소통할 때
// - UI, 게임 상태, 효과 등 다양한 곳에서 이벤트 기반 구조가 필요할 때
//
// [특징]
// - 발행자와 구독자 간 직접 참조 없이 이벤트로 연결

// 이벤트 버스
class EventBus
{
    private Dictionary<string, List<Action<object>>> listeners = new Dictionary<string, List<Action<object>>>();
    public void Subscribe(string eventName, Action<object> callback)
    {
        if (!listeners.ContainsKey(eventName)) listeners[eventName] = new List<Action<object>>();
        listeners[eventName].Add(callback);
    }
    public void Publish(string eventName, object data)
    {
        if (listeners.ContainsKey(eventName))
            foreach (var cb in listeners[eventName]) cb(data);
    }
}

// 사용 예시
class EventBusPatternMain
{
    static void Main(string[] args)
    {
        var bus = new EventBus();
        bus.Subscribe("PlayerDead", data => Console.WriteLine($"플레이어 사망! 원인: {data}"));
        bus.Publish("PlayerDead", "적 공격");
    }
}
