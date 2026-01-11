#pragma once

#include "Allocator.h"
#include "../Threading/SpinLock.h"
#include <memory>
#include <iostream>

namespace Core {

class ThreadSafeProxy : public Allocator {
public:
    // 기존 할당자의 소유권을 전달받음.
    explicit ThreadSafeProxy(Allocator* target)
        : m_target(target) {}

    virtual void* allocate(size_t size, size_t alignment = 4) override {
        m_lock.lock();
        void* ptr = m_target->allocate(size, alignment);
        m_lock.unlock();
        return ptr;
    }

    virtual void deallocate(void* p) override {
        m_lock.lock();
        m_target->deallocate(p);
        m_lock.unlock();
    }

#ifdef ENABLE_ALLOCATOR_STATS
    virtual size_t getCurrentUsedSize() const override {
        m_lock.lock();
        size_t size = m_target->getCurrentUsedSize();
        m_lock.unlock();
        return size;
    }
#endif

private:
    Allocator* m_target;
    mutable SpinLock m_lock;
};

}
