    #pragma once
    #include "PacketHandlers.h"
    #include "../Threading/WorkerContext.h"

    namespace Core {

    typedef void (*PacketHandlerFunc)(const WorkerContext&, const Protocol::GamePacket*);

    class Session; // 순환 참조 방지

    class PacketDispatcher {
    public:
        // 1. GetInstance 정의 (싱글톤)
        static PacketDispatcher& GetInstance() {
            static PacketDispatcher instance;
            return instance;
        }

        // 복사 및 대입 방지 (싱글톤의 기본)
        PacketDispatcher(const PacketDispatcher&) = delete;
        void operator=(const PacketDispatcher&) = delete;

        // 초기화
        void Init();

        // 패킷 수신
        inline void OnRecvPacket(const WorkerContext& ctx) {
            auto verifier = flatbuffers::Verifier(ctx.payload, ctx.payloadSize);
            if (!Protocol::VerifyGamePacketBuffer(verifier)) {
                return;
            }

            // 루트 객체 얻기
            auto gamePacket = Protocol::GetGamePacket(ctx.payload);
            
            // 핸들러 실행
            auto handler = m_handlers[static_cast<uint16_t>(ctx.type)];
            if (__builtin_expect(handler != nullptr, 1)) { // 분기 에측 최적화
                handler(ctx, gamePacket);
            } else {
                LOG_INFO("No handler registered for Type: ", (int)ctx.type);
            }
        }

    private:
        PacketDispatcher() { memset(m_handlers, 0, sizeof(m_handlers)); }
        PacketHandlerFunc m_handlers[256];
    };

    }
