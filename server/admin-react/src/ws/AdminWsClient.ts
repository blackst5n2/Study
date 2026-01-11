// 관리자 React 클라이언트(WebSocket) 예시: JWT 인증, JSON 명령 송수신

export type AdminCommand = {
  type: string;
  targetUserId?: string;
  message?: string;
};

export class AdminWsClient {
  private ws: WebSocket | null = null;
  private url: string;
  private jwt: string;
  private onMessage: (msg: any) => void;
  private onClose?: () => void;

  constructor(url: string, jwt: string, onMessage: (msg: any) => void, onClose?: () => void) {
    this.url = url;
    this.jwt = jwt;
    this.onMessage = onMessage;
    this.onClose = onClose;
  }

  connect() {
    this.ws = new WebSocket(this.url);
    this.ws.onopen = () => {
      // JWT 인증 헤더는 프로토콜상 직접 전송 불가 → 첫 메시지로 전송 (서버에서 처리 필요)
      this.sendRaw({ type: 'auth', token: this.jwt });
    };
    this.ws.onmessage = (ev) => {
      try {
        const msg = JSON.parse(ev.data);
        this.onMessage(msg);
      } catch {
        // 비표준 메시지 등 무시
      }
    };
    this.ws.onclose = () => {
      if (this.onClose) this.onClose();
    };
  }

  sendCommand(cmd: AdminCommand) {
    this.sendRaw(cmd);
  }

  private sendRaw(obj: any) {
    if (this.ws && this.ws.readyState === WebSocket.OPEN) {
      this.ws.send(JSON.stringify(obj));
    }
  }

  close() {
    if (this.ws) this.ws.close();
  }
}
