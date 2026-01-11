import re
import os

current_script_path = os.path.abspath(__file__)

project_root = os.path.dirname(os.path.dirname(current_script_path))

def generate_packet_code():
    fbs_path = os.path.join(project_root, "Protocols/Protocol.fbs")
    output_path = os.path.join(project_root, "src/Core/Network/PacketList.h")
    handlers_cpp_path = os.path.join(project_root, "src/Core/Network/PacketHandlers.cpp")

    if not os.path.exists(fbs_path):
        print(f"Error: {fbs_path} not found")
        return

    with open(fbs_path, 'r') as f:
        content = f.read()

    # 1. PacketPayload 유니온 안의 패킷 이들름 추출
    match = re.search(r'union\s+PacketPayload\s*{([^}]+)}', content)
    if not match:
        print("Could not find PacketPayload union in fbs file.")
        return

    # 공백 제거 및 쉼표로 분리하여 패킷 리스트 생성
    packets = [p.strip() for p in match.group(1).split(',') if p.strip()]

    # 서버 수신용 (C_) 패킷만 필터링
    c_packets = [p for p in packets if p.startswith('C_')]

    # 2. 매크로 파일 생성 (PacketList.h)
    with open(output_path, 'w') as f:
        f.write('#pragma once\n\n')
        f.write("#define FOR_EACH_PACKET(V) \\\n")
        for p in c_packets:
            f.write(f"  V({p}) \\\n")
        f.write("\n")
    
    print(f"[*] Generated PacketList.h with {len(c_packets)} packets.")

    # 3. 미구현 핸들러 찾기
    implemented_content = ""
    if os.path.exists(handlers_cpp_path):
        with open(handlers_cpp_path, 'r', encoding='utf-8') as f:
            implemented_content = f.read()
        
    missing_handlers = []
    for p in c_packets:
        handler_name = f"Handle_{p}"
        if handler_name not in implemented_content:
            missing_handlers.append(p)
        
    # 4. 결과 출력
    if missing_handlers:
        print(f"[!] 미구현 핸들러 {len(missing_handlers)}개")
        for p in missing_handlers:
            print(f"   - {p} (Handle_{p})")
    else:
        print("\n[V] 모든 핸들러 구현 완료")
if __name__ == "__main__":
    generate_packet_code()