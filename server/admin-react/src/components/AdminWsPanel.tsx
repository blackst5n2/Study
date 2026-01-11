import React, { useEffect, useRef, useState } from 'react';
import { AdminWsClient } from '../ws/AdminWsClient';

interface AdminWsPanelProps {
  wsUrl: string;
  jwt: string;
}

export const AdminWsPanel: React.FC<AdminWsPanelProps> = ({ wsUrl, jwt }) => {
  const [connected, setConnected] = useState(false);
  const [authOk, setAuthOk] = useState(false);
  const [messages, setMessages] = useState<any[]>([]);
  const wsClientRef = useRef<AdminWsClient | null>(null);

  // 고급 예외처리 및 재연결 로직
  const reconnectTimeout = useRef<ReturnType<typeof setTimeout> | null>(null);
  const [reconnectCount, setReconnectCount] = useState(0);

  useEffect(() => {
    connectWs();
    return () => {
      wsClientRef.current?.close();
      if (reconnectTimeout.current) clearTimeout(reconnectTimeout.current);
    };
    // eslint-disable-next-line
  }, [wsUrl, jwt]);

  function connectWs() {
    setAuthOk(false);
    setConnected(false);
    wsClientRef.current = new AdminWsClient(
      wsUrl,
      jwt,
      (msg) => {
        if (msg.type === 'auth_ok') {
          setAuthOk(true);
        } else if (msg.type === 'auth_fail') {
          setAuthOk(false);
          wsClientRef.current?.close();
        } else {
          setMessages((prev) => [...prev, msg]);
        }
      },
      () => {
        setConnected(false);
        setAuthOk(false);
        // 자동 재연결 (최대 5회, 2초 간격)
        if (reconnectCount < 5) {
          reconnectTimeout.current = setTimeout(() => {
            setReconnectCount((c) => c + 1);
            connectWs();
          }, 2000);
        }
      }
    );
    wsClientRef.current.connect();
    setConnected(true);
  }

  // 명령 전송
  function sendKick(userId: string) {
    wsClientRef.current?.sendCommand({ type: 'kick', targetUserId: userId });
  }
  function sendNotice(msg: string) {
    wsClientRef.current?.sendCommand({ type: 'broadcast_notice', message: msg });
  }

  return (
    <div style={{ border: '1px solid #888', padding: 16, borderRadius: 8 }}>
      <h3>Admin WebSocket Panel</h3>
      <div>연결 상태: {connected ? '연결됨' : '끊김'} / 인증: {authOk ? '성공' : '실패'}</div>
      <button disabled={!authOk} onClick={() => sendKick(prompt('유저 ID?') || '')}>유저 강제 퇴장</button>
      <button disabled={!authOk} onClick={() => sendNotice(prompt('공지 내용?') || '')}>전체 공지</button>
      <div style={{ marginTop: 12, maxHeight: 200, overflow: 'auto', background: '#222', color: '#fff', padding: 8 }}>
        <b>메시지 로그:</b>
        <ul>
          {messages.map((msg, idx) => (
            <li key={idx}>{JSON.stringify(msg)}</li>
          ))}
        </ul>
      </div>
      {reconnectCount > 0 && !connected && reconnectCount < 5 && (
        <div style={{ color: 'orange' }}>재연결 시도 중... ({reconnectCount})</div>
      )}
      {reconnectCount >= 5 && !connected && (
        <div style={{ color: 'red' }}>연결 실패! 서버 상태/네트워크를 확인하세요.</div>
      )}
    </div>
  );
};
