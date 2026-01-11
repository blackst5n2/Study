#include <iostream>
#include <vector>
#include <thread>
#include <netinet/in.h>
#include <netinet/tcp.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <chrono>
#include <cstring>
#include <sys/socket.h>
#include <fcntl.h>
#include <algorithm>
#include "Protocol_generated.h"

// --- 테스트 설정 ---
const int SESSION_COUNT = 1000;         // 총 세션 수
const int THREAD_COUNT = 8;             // 클라이언트 스레드 수
const int PACKETS_PER_SESSION = 1000;   // 세션당 패킷 수 (총 100만 개)
const char* SERVER_IP = "127.0.0.1";
const int SERVER_PORT = 8080;
const int YIELD_INTERVAL = 5;           // 5000번(세션단위 루프 기준 조정) 혹은 적절한 주기에 1번 yield

#pragma pack(push, 1)
struct PacketHeader {
    uint16_t magic = 0xAAFF;
    uint16_t type = 1;
    uint32_t size;
};
#pragma pack(pop)

// FlatBuffers 패킷 생성 (재사용용)
std::vector<uint8_t> CreateSerializedPacket() {
    flatbuffers::FlatBufferBuilder fbb(1024);
    auto id = fbb.CreateString("test_user");
    auto pw = fbb.CreateString("test_password");
    auto login = Protocol::CreateC_Login(fbb, id, pw, std::chrono::system_clock::to_time_t(std::chrono::system_clock::now()));
    auto packet = Protocol::CreateGamePacket(fbb, Protocol::PacketPayload_C_Login, login.Union());
    fbb.Finish(packet);

    uint32_t payloadSize = fbb.GetSize();
    PacketHeader header;
    header.magic = 0xAAFF;
    header.type = static_cast<uint16_t>(Protocol::PacketPayload_C_Login);
    header.size = sizeof(PacketHeader) + payloadSize;

    std::vector<uint8_t> fullPacket(header.size);
    std::memcpy(fullPacket.data(), &header, sizeof(PacketHeader));
    std::memcpy(fullPacket.data() + sizeof(PacketHeader), fbb.GetBufferPointer(), payloadSize);

    return fullPacket;
}

void WorkerThread(int threadId, std::vector<int> mySockets, const std::vector<uint8_t>& packetData) {
    uint8_t dummyBuf[4096];
    int sendCounter = 0;

    for (int sock : mySockets) {
        int opt = 1;
        setsockopt(sock, IPPROTO_TCP, TCP_NODELAY, (const char*)&opt, sizeof(opt));
        fcntl(sock, F_SETFL, fcntl(sock, F_GETFL, 0) | O_NONBLOCK);
    }

    // 전송 루프
    for (int i = 0; i < PACKETS_PER_SESSION; ++i) {
        for (int sock : mySockets) {
            send(sock, packetData.data(), packetData.size(), 0);
            
            // 수신 버퍼 비우기 (TCP Window 유지)
            while (recv(sock, dummyBuf, sizeof(dummyBuf), 0) > 0);

            // 약 5000번의 전송(send 호출)마다 한 번씩 CPU 양보
            if (++sendCounter >= 5000) {
                std::this_thread::yield();
                sendCounter = 0;
            }
        }
    }

    // 전송 완료 후 서버가 남은 패킷을 다 처리할 수 있도록 드레인(Drain)
    for (int j = 0; j < 50; ++j) {
        for (int sock : mySockets) {
            while (recv(sock, dummyBuf, sizeof(dummyBuf), 0) > 0);
        }
        std::this_thread::sleep_for(std::chrono::milliseconds(10));
    }
}

int main() {
    // 1. 시스템 제한 확인 안내 (ulimit -n 2048 필수)
    std::vector<uint8_t> masterPacket = CreateSerializedPacket();
    std::cout << "[*] Master Packet Created (" << masterPacket.size() << " bytes)" << std::endl;

    std::vector<int> allSockets;
    sockaddr_in serverAddr{};
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(SERVER_PORT);
    inet_pton(AF_INET, SERVER_IP, &serverAddr.sin_addr);

    std::cout << "[*] Connecting " << SESSION_COUNT << " sessions..." << std::endl;
    for (int i = 0; i < SESSION_COUNT; ++i) {
        int sock = socket(AF_INET, SOCK_STREAM, 0);
        if (connect(sock, (struct sockaddr*)&serverAddr, sizeof(serverAddr)) == -1) {
            close(sock);
            continue;
        }
        allSockets.push_back(sock);
    }

    std::vector<std::vector<int>> threadSockets(THREAD_COUNT);
    for (int i = 0; i < (int)allSockets.size(); ++i) {
        threadSockets[i % THREAD_COUNT].push_back(allSockets[i]);
    }

    std::cout << "[*] Starting E2E Measurement Test (5000-send yield applied)..." << std::endl;
    
    // --- 측정 시작 ---
    auto start = std::chrono::high_resolution_clock::now();

    std::vector<std::thread> workers;
    for (int i = 0; i < THREAD_COUNT; ++i) {
        workers.emplace_back(WorkerThread, i, threadSockets[i], std::ref(masterPacket));
    }

    for (auto& t : workers) {
        if (t.joinable()) t.join();
    }

    // --- 측정 종료 ---
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> diff = end - start;

    double totalPackets = (double)allSockets.size() * PACKETS_PER_SESSION;
    std::cout << "\n========================================" << std::endl;
    std::cout << " 클라이언트 관점 E2E 측정 결과" << std::endl;
    std::cout << " 실제 연결 세션: " << allSockets.size() << " 개" << std::endl;
    std::cout << " 총 처리 패킷: " << (int)totalPackets << " 개" << std::endl;
    std::cout << " 소요 시간: " << diff.count() << " s" << std::endl;
    std::cout << " 시스템 전체 TPS: " << totalPackets / diff.count() << " req/s" << std::endl;
    std::cout << "========================================\n" << std::endl;

    for (int sock : allSockets) close(sock);
    return 0;
}