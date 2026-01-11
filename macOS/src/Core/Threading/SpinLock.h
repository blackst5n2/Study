#pragma once
#include <atomic>

namespace Core {

class SpinLock {
public:
    // 복사 및 이동 금지 (락 객체는 고유해야 함)
    SpinLock(const SpinLock&) = delete;
    SpinLock& operator=(const SpinLock&) = delete;
    SpinLock() = default;

    void lock() {
        // test_and_set은 값을 ture로 만들고 "이전 값"을 반환함.
        // 누군가 이미 잠갔다면(ture였다면) 루프를 돌며 기다림.
        // memory_order_acquire: 이 이후의 메모리 연산이 락 획득 전으로 앞질러가지 못하게 막음.
        while (m_flag.test_and_set(std::memory_order_acquire));
    }

    void unlock() {
        // 값을 false로 만들어서 다른 쓰레드가 들어올 수 있게 함.
        // memory_order_release: 이 이전의 모든 메모리 연산이 완료된 후 락을 해제하도록 보장.
        m_flag.clear(std::memory_order_release);
    }

private:
    std::atomic_flag m_flag = ATOMIC_FLAG_INIT;
};

}