#pragma once

#include "Allocator.h"
#include <cstdint>
#include <cstddef>

namespace Core {
// 모든 메모리 블록에 붙는 메타데이터 (할당 여부 크기)
struct BlockHeader {
    size_t size; // 이 블록의 전체 크기 (데이터 + 헤더 + 메타데이터)
    size_t previous_block_size; // 바로 앞 블록의 전체 크기 (병합용 역추적)
    uint16_t alignment_padding; // Header 끝과 Data 시작점 사이의 거리
    bool is_free; // 현재 블록이 사용중인지
}; 

// 빈 블록일 경우에만 사용되는 양방향 연결 리스트 노드
// 빈 블록의 '데이터 영역'에 오버레이 됨
struct FreeListNode {
    FreeListNode* next; // 다음 빈 블록 포인터
    FreeListNode* prev; // 이전 빈 블록 포인터
};

class FreeListAllocator : public Allocator {
public:
    // FreeListAllocator(size_t totalSize, size_t alignment = 4);
    FreeListAllocator(void* startPtr, size_t totalSize, size_t alignment = 4);
    ~FreeListAllocator();

    void* allocate(size_t size, size_t alignment = 4) override;
    void deallocate(void* p) override;

#ifdef ENABLE_ALLOCATOR_STATS
    size_t getCurrentUsedSize() const override;
#endif

private:
    void* m_startPtr = nullptr;
    size_t m_totalSize = 0;
    size_t m_alignment = 0;

    FreeListNode* m_freeListHead = nullptr;

    // 할당 및 해제 시 블록 분할/병함에 사용될 헬퍼 함수
    void removeNode(FreeListNode* node);
    void insertNode(FreeListNode* node);
    FreeListNode* findBestFit(size_t size, size_t alignment = 4);

    // 주어진 Header로부터 다음 블록의 Header 포인터를 얻음
    inline BlockHeader* get_next_header(BlockHeader* header) {
        // 다음 Header 주소 = 현재 Header 주소 + 현재 블록 크기
        return (BlockHeader*)((uintptr_t)header + header->size);
    }

    bool m_isExternallyAllocated = false;

#ifdef ENABLE_ALLOCATOR_STATS
    size_t m_currentUsedSize = 0;
#endif

};

}
