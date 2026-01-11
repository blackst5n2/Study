using System;
using System.Collections.Generic;

// 오브젝트 풀(Object Pool) 패턴 예제
//
// [게임 개발에서 언제 사용?]
// - 생성/파괴 비용이 큰 객체(총알, 이펙트 등)를 재사용하고 싶을 때
// - 성능 최적화, GC(가비지 컬렉션) 부담 감소
//
// [특징]
// - 미리 객체를 만들어 두고, 필요할 때 꺼내쓰고 반환

class Bullet { public int Id; public void Fire() => Console.WriteLine($"총알 {Id} 발사!"); }

class ObjectPool<T> where T : new()
{
    private Queue<T> pool = new Queue<T>();
    public ObjectPool(int initialCount)
    {
        for (int i = 0; i < initialCount; i++) pool.Enqueue(new T());
    }
    public T Get() => pool.Count > 0 ? pool.Dequeue() : new T();
    public void Release(T obj) => pool.Enqueue(obj);
}

class ObjectPoolPatternMain
{
    static void Main(string[] args)
    {
        var bulletPool = new ObjectPool<Bullet>(3);
        var b1 = bulletPool.Get(); b1.Id = 1; b1.Fire();
        var b2 = bulletPool.Get(); b2.Id = 2; b2.Fire();
        bulletPool.Release(b1);
        var b3 = bulletPool.Get(); b3.Id = 3; b3.Fire();
        // b1 객체가 재사용됨
    }
}
