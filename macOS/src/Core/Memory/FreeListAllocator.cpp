#include "FreeListAllocator.h"
#include <cstdlib>
#include <cassert>
#include <algorithm>
#include <vector>
#include <iostream>
#include "../Utils/MemoryUtils.h"

namespace Core {

/*
FreeListAllocator::FreeListAllocator(size_t totalSize, size_t alignment)
    : m_totalSize(totalSize), m_alignment(alignment)
{
    // 1. OS로부터 메모리 할당
    m_startPtr = malloc(totalSize);
    assert(m_startPtr != nullptr && "FreeList Allocator failed to allocate memory.");

    // 2. 헤더 설정
    BlockHeader* initialHeader = (BlockHeader*)m_startPtr;
    initialHeader->size = totalSize; // 이 블록의 전체 물리적 크기
    initialHeader->previous_block_size = 0; // 이전 블록의 전체 물리적 크기
    initialHeader->is_free = true; // 초기 상태: 빈 블록

    m_freeListHead = (FreeListNode*)((uintptr_t)m_startPtr + sizeof(BlockHeader));
    m_freeListHead->next = nullptr;
    m_freeListHead->prev = nullptr;
}
*/

FreeListAllocator::FreeListAllocator(void* startPtr, size_t totalSize, size_t alignment) 
    : m_startPtr(startPtr), m_totalSize(totalSize), m_alignment(alignment), m_isExternallyAllocated(true)
{
    assert(m_startPtr != nullptr && "FreeList Allocator failed to allocate memory.");

    // 2. 헤더 설정
    BlockHeader* initialHeader = (BlockHeader*)m_startPtr;
    initialHeader->size = totalSize; // 이 블록의 전체 물리적 크기
    initialHeader->previous_block_size = 0; // 이전 블록의 전체 물리적 크기
    initialHeader->is_free = true; // 초기 상태: 빈 블록

    m_freeListHead = (FreeListNode*)((uintptr_t)m_startPtr + sizeof(BlockHeader));
    m_freeListHead->next = nullptr;
    m_freeListHead->prev = nullptr;
}

FreeListAllocator::~FreeListAllocator() {
    if (!m_isExternallyAllocated) {
        free(m_startPtr);
    }
    m_startPtr = nullptr;
    m_freeListHead = nullptr;
}

// 할당 및 해제는 현재 Coalescing 테스트를 통과할 수 있도록 구현되어야 함.
void* FreeListAllocator::allocate(size_t size, size_t alignment) {
    if (size == 0) {
        return nullptr;
    }
    
    // 1. 필요한 총 크기 계산
    // [Header] + [Alignment Padding] + [HeaderPtr(8바이트)] + [Data(size)]
    size_t headerPtrSpace = sizeof(BlockHeader*);
    size_t minRequired = sizeof(BlockHeader) + headerPtrSpace + size;

    FreeListNode* foundNode = findBestFit(minRequired, alignment);
    if (!foundNode) return nullptr;

    BlockHeader* header = (BlockHeader*)((uintptr_t)foundNode - sizeof(BlockHeader));
    size_t originalTotalSize = header->size;

    // 2. 사용자 데이터 주소(p) 결정
    // Header 바로 뒤가 아니라, HeaderPtrSpace만큼 띄운 곳을 기준으로 정렬
    uintptr_t baseAddr = (uintptr_t)header + sizeof(BlockHeader) + headerPtrSpace;
    uintptr_t alignedP = Utils::alignForward(baseAddr, alignment);

    // 3. 할당될 블록의 실제 점유 크기 계산 (헤더부터 데이터 끝까지)
    // 주의: splitting을 하더라도 다음 블록의 헤더 위치가 정렬에 영향을 받지 않도록 
    // 할당 블록의 끝을 적절히 맞춰줍니다.
    size_t allocatedBlockSize = (alignedP + size) - (uintptr_t)header;

    // 4. Splitting 여부 판단
    // 남은 공간이 (Header + FreeListNode) 이상일 때만 분할
    if (originalTotalSize>= allocatedBlockSize + sizeof(BlockHeader) + sizeof(FreeListNode)) {
        header->size = allocatedBlockSize;
        header->is_free = false;

        // 새 빈 블록(Next) 생성
        BlockHeader* nextHeader = (BlockHeader*)((uintptr_t)header + header->size);
        nextHeader->size = originalTotalSize - header->size;
        nextHeader->previous_block_size = header->size; // 핵심: 연골 고리 생성
        nextHeader->is_free = true;

        // 다음의 다음 블록에게도 알림 (존재한다면)
        BlockHeader* nextNextHeader = (BlockHeader*)((uintptr_t)nextHeader + nextHeader->size);
        if ((uintptr_t)nextNextHeader < (uintptr_t)m_startPtr + m_totalSize) {
            nextNextHeader->previous_block_size = nextHeader->size;
        }

        // FreeList 갱신 (foundNode 제거 후 nextHeader를 삽입)
        removeNode(foundNode);
        insertNode((FreeListNode*)((uintptr_t)nextHeader + sizeof(BlockHeader)));
    } else {
        // 분할하지 않고 통째로 사용
        header->is_free = false;
        removeNode(foundNode);
    }

    // Header* 기록 (p 바로 직전 8바이트)
    BlockHeader** headerStoredLoc = (BlockHeader**)(alignedP - headerPtrSpace);
    *headerStoredLoc = header;

    return (void*)alignedP;
}

void FreeListAllocator::deallocate(void* p) {
    if (p == nullptr) {
        return;
    }

    BlockHeader** headerStoredLoc = (BlockHeader**)((uintptr_t)p - sizeof(BlockHeader*));

    BlockHeader* targetHeader = *headerStoredLoc;

    assert(!targetHeader->is_free && "Double Free Detected!");
    targetHeader->is_free = true;

    // 1. 이전 블록과 병합 (Backward)
    if (targetHeader->previous_block_size > 0) {
        BlockHeader* prevHeader = (BlockHeader*)((uintptr_t)targetHeader - targetHeader->previous_block_size);
        if(prevHeader->is_free) {
            removeNode((FreeListNode*)((uintptr_t)prevHeader + sizeof(BlockHeader)));
            prevHeader->size += targetHeader->size;
            targetHeader = prevHeader; // 병합된 블록을 기준으로 함
        }
    }

    // 2. 다음 블록과 병합 (Forward)
    BlockHeader* nextHeader = (BlockHeader*)((uintptr_t)targetHeader + targetHeader->size);
    if ((uintptr_t)nextHeader < (uintptr_t)m_startPtr + m_totalSize) {
        if (nextHeader->is_free) {
            removeNode((FreeListNode*)((uintptr_t)nextHeader + sizeof(BlockHeader)));
            targetHeader->size += nextHeader->size;
        }
    }

    // 3. 최종 블록 정보 갱신 및 FreeList 삽입
    // 다음 블록에게 내 크기가 변했음을 알려줌
    BlockHeader* finalNextHeader = (BlockHeader*)((uintptr_t)targetHeader + targetHeader->size);
    if ((uintptr_t)finalNextHeader < (uintptr_t)m_startPtr + m_totalSize) {
        finalNextHeader->previous_block_size = targetHeader->size;
    }

    insertNode((FreeListNode*)((uintptr_t)targetHeader + sizeof(BlockHeader)));
}

void FreeListAllocator::removeNode(FreeListNode* node) {
    if (node == nullptr) {
        return;
    }

    // 1. 이전 노드(prev)의 next 포인터를 갱신
    if (node->prev != nullptr) {
        node->prev->next = node->next;
    } else {
        // 제거할 노드가 Head 노드라면, Free List의 Head를 다음 노드로 연결
        m_freeListHead = node->next;
    }

    // 2. 다음 노드(next)의 prev 포인터를 갱신
    if (node->next != nullptr) {
        node->next->prev = node->prev;
    }
}

void FreeListAllocator::insertNode(FreeListNode* node) {
    if (node == nullptr) {
        return;
    }

    // 기존 Head 앞에 삽입
    node->next = m_freeListHead;
    node->prev = nullptr;

    if (m_freeListHead != nullptr) {
        m_freeListHead->prev = node;
    }
    m_freeListHead = node;
}

FreeListNode* FreeListAllocator::findBestFit(size_t requiredSize, size_t alignment) {
    FreeListNode* bestFitNode = nullptr;
    size_t minRemainingSize = std::numeric_limits<size_t>::max();

    FreeListNode* current = m_freeListHead;

    // Free List 순회 (Best Fit)
    while (current != nullptr) {
        // 1. 현재 노드의 헤더 위치 파악
        BlockHeader* header = (BlockHeader*)((uintptr_t)current - sizeof(BlockHeader));
        
        // 2. 정렬을 위해 필요한 조정 값(Adjustment) 계산
        // 데이터 시작점 = current (이미 Header 바로 뒤)
        uintptr_t dataStart = (uintptr_t)current;
        uintptr_t alignedStart = Utils::alignForward(dataStart, alignment);
        size_t adjustment = alignedStart - dataStart;

        // 3. 이 블록에서 실제 가용한 크기 확인
        size_t effectiveSize = header->size - sizeof(BlockHeader);

        if (effectiveSize >= requiredSize + adjustment) {
            size_t remainingSize = effectiveSize - (requiredSize + adjustment);

            // 4. 더 작은 잔여 공간을 가진 블록 발견 시 갱신
            if (remainingSize < minRemainingSize) {
                minRemainingSize = remainingSize;
                bestFitNode = current;

                // 0이면 더 이상 찾을 필요 없는 Perfect Fit
                if (minRemainingSize == 0) break;
            }
        }
        current = current->next;
    }

    return bestFitNode;
}

#ifdef ENABLE_ALLOCATOR_STATS
size_t FreeListAllocator::getCurrentUsedSize() const {
    // TDD: 우선 0 반환
    return m_currentUsedSize;
}
#endif

}
