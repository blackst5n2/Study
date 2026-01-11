#include "ThreadPool.h"
#include "../Network/PacketDispatcher.h"
#include "ThreadManager.h"
#include "../Network/Session.h"
#include "../Memory/MemoryHelpers.h"
#include <iostream>
#include "../Logger.h"
#include "../Network/NetworkManager.h"
#include "WorkerContext.h"
#include <chrono>

namespace Core {

ThreadPool::ThreadPool(size_t threadCount) {
    m_threadCount = threadCount;
}

ThreadPool::~ThreadPool() {
}

void ThreadPool::Start() {
    m_workers = static_cast<std::thread*>(Core::Alloc(sizeof(std::thread) * m_threadCount));
    for (size_t i = 0; i < m_threadCount; ++i) {
        new (&m_workers[i]) std::thread(&ThreadPool::WorkerThread, this);
    }
}

void ThreadPool::WorkerThread() {
    // 매니저를 통해 스레드 정보를 등록
    ThreadManager::GetInstance().InitThread();
    PacketDescriptor desc;

    // 배치 처리를 위한 임시 저장소
    static constexpr size_t BATCH_SIZE = 64;
    PacketDescriptor descBatch[BATCH_SIZE];

    auto& networkMgr = NetworkManager::GetInstance();
    auto& sessionMgr = networkMgr.GetSessionManager();
    auto* recvQueue = NetworkManager::GetInstance().GetRecvQueue();

    while (true) {
        // Batch Pop 시도
        size_t count = 0;
        while (count < BATCH_SIZE) {
            if (recvQueue->pop(descBatch[count])) {
                count++;
            } else {
                break;
            }
        }
        
        if (count > 0) {
            for (size_t i = 0; i < count; ++i) {
                Session* session = sessionMgr.GetSession(descBatch[i].session_id);
                if (session) {
                    ProcessPacket(session, descBatch[i]);
                }
            }
        } else {
            // 1. Spin-wait
            for (int i = 0; i < 1000; ++i) {
                // 어셈블리 명령어로 CPU에게 힌트 (Mac/Linux 공용)
#if defined(__x86_64__)
                __asm__ __volatile__("pause" ::: "memory");
#elif defined(__aarch64__)
                __asm__ __volatile__("yield" ::: "memory"); // Apple Silicon은 yield가 pause 역할
#endif
                if (!recvQueue->empty()) break; 
            }
        }
    }

    // 종료 시 정리
    ThreadManager::GetInstance().DestroyThread();
}

void ThreadPool::ProcessPacket(Session* session, const PacketDescriptor desc) {
    if (!session) return;

    uint32_t recvLen = desc.length;

    uint8_t* tempBuf = static_cast<uint8_t*>(Core::GetLinearArena()->Allocate(recvLen));
    memcpy(tempBuf, reinterpret_cast<const void*>(desc.data_ptr), recvLen);

    uint8_t* targetPtr = tempBuf;
    uint32_t targetLen = recvLen;

    // 백로그 병합
    if (session->HasBacklog()) {
        uint32_t backlogSize = session->GetBacklogSize();
        uint32_t totalSize = backlogSize + recvLen;

        // LinearArena에 임시 병합 버퍼 할당
        uint8_t* mergedPtr = static_cast<uint8_t*>(Core::GetLinearArena()->Allocate(totalSize));

        // 기존 백로그 + 새로 받은 데이터 합치기
        memcpy(mergedPtr, session->GetBacklogPtr(), backlogSize);
        memcpy(mergedPtr + backlogSize, tempBuf, recvLen);

        targetPtr = mergedPtr;
        targetLen = totalSize;

        // 병합 완료 후 백로그 비우기
        session->ClearBacklog();
    }

    uint32_t processed = 0;
    while (targetLen - processed >= sizeof(PacketHeader)) {
        const PacketHeader* header = reinterpret_cast<const PacketHeader*>(targetPtr + processed);

        if (header->magic != 0xAAFF) {
            processed = targetLen;
            break;
        }

        uint32_t packetSize = header->size;

        if (packetSize < sizeof(PacketHeader) || packetSize > 65535 * 2) { // 최대 사이즈 제한 권장
            processed = targetLen; 
            break;
        }

        if (targetLen - processed >= packetSize) {
            HandleBusinessLogic(session, targetPtr + processed, packetSize);
            processed += packetSize;
        } else {
            break;
        }
    }

    Core::GetLinearArena()->Reset();

    // 남은 파편 처리
    uint32_t remaining = targetLen - processed;
    if (remaining > 0) {
        session->SaveBacklog(targetPtr + processed, remaining);
    }

    NetworkManager::GetInstance().AddConsumedBytes(recvLen);
}

void ThreadPool::HandleBusinessLogic(Session* session, uint8_t* raw_data, uint32_t size) {

    PacketHeader* header = reinterpret_cast<PacketHeader*>(raw_data);
    uint8_t* data = raw_data + sizeof(PacketHeader);
    size_t dataSize = size - sizeof(PacketHeader);
    auto type = header->type;

    // 스레드의 TLS 도구 가져오기
    LinearArena* myArena = Core::GetLinearArena();
    flatbuffers::FlatBufferBuilder* myFBB = Core::GetFBB();

    // 컨텍스트 구성
    WorkerContext ctx;
    ctx.session = session;
    ctx.payload = data;
    ctx.payloadSize = dataSize;
    ctx.type = type;
    ctx.arena = myArena;
    ctx.fbb = myFBB;
    
    // 패킷 디스패처 호출
    PacketDispatcher::GetInstance().OnRecvPacket(ctx);

    myFBB->Clear();
}

}
