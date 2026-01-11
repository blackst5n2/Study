import React from "react";
import CountingSortCode from "../Code/CountingSortCode";
import CountingSortAnimation from "../Animation/CountingSortAnimation";

import useModalBodyScrollLock from '../hooks/useModalBodyScrollLock';

export default function CountingSortModal({ open, onClose, arr, darkMode, theme }) {
  useModalBodyScrollLock(open);
  if (!open) return null;
  const textColor = theme ? theme.palette.text.primary : (darkMode ? '#e3f2fd' : '#222');
  const borderColor = theme ? theme.palette.primary.main : (darkMode ? '#90caf9' : '#1976d2');
  const overlayBg = darkMode ? 'rgba(10,18,30,0.78)' : 'rgba(30,40,60,0.48)';
  return (
    <div
      style={{
        position: 'fixed', top: 0, left: 0, width: '100vw', height: '100vh', background: overlayBg, zIndex: 1200, display: 'flex', alignItems: 'center', justifyContent: 'center', transition: 'background 0.3s', backdropFilter: 'blur(2.5px)'
      }}
      onClick={onClose}
    >
      <div
        style={{
          position: 'relative',
          background: darkMode ? 'linear-gradient(135deg, #23272f 80%, #181c21 100%)' : 'linear-gradient(135deg, #fafdff 80%, #e3f2fd 100%)',
          borderRadius: 22,
          boxShadow: darkMode ? '0 8px 40px 0 #000a, 0 1.5px 8px 0 #90caf955' : '0 8px 40px 0 #1a237e33, 0 1.5px 8px 0 #1976d255',
          padding: '38px 38px 26px 38px',
          maxWidth: 980,
          width: '97vw',
          minHeight: 320,
          maxHeight: '92vh',
          overflowY: 'auto',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          border: `1.8px solid ${borderColor}`,
          paddingBottom: 'max(24px, env(safe-area-inset-bottom))',
        }}
        onClick={e => e.stopPropagation()}
      >
        {/* 닫기 버튼 */}
        <button
          style={{
            position: 'absolute', top: 17, right: 22,
            background: darkMode ? 'rgba(40,60,90,0.82)' : 'rgba(240,248,255,0.82)',
            border: 'none', borderRadius: '50%', width: 44, height: 44, fontSize: 28,
            color: borderColor, cursor: 'pointer', fontWeight: 900, zIndex: 10,
            boxShadow: darkMode ? '0 1.5px 8px #222a' : '0 1.5px 8px #1976d233', transition: 'background 0.18s',
          }}
          onClick={onClose}
          aria-label="그래프 닫기"
          onMouseOver={e => e.currentTarget.style.background = (darkMode ? '#23272f' : '#e3f2fd')}
          onMouseOut={e => e.currentTarget.style.background = (darkMode ? 'rgba(40,60,90,0.82)' : 'rgba(240,248,255,0.82)')}
        >
          ×
        </button>
        {/* 알고리즘 이름 */}
        <div style={{
          margin: "0 0 28px 0",
          padding: "0 0 6px 0",
          textAlign: "center",
          fontWeight: 900,
          fontSize: 42,
          color: borderColor,
          letterSpacing: 2,
          textShadow: darkMode ? '0 2px 14px #90caf955' : '0 2px 14px #90caf955'
        }}>
          계수 정렬 <span style={{ fontSize: 26, color: textColor, fontWeight: 700, letterSpacing: 1 }}>(Counting Sort)</span>
        </div>
        {/* 설명 영역 */}
        <div style={{
          margin: "0 0 20px 0",
          padding: "18px 30px 12px 30px",
          background: darkMode ? '#2d2a1b' : "#fffde7",
          borderRadius: 14,
          fontWeight: 500,
          fontSize: 19,
          color: darkMode ? '#ffe082' : "#795548",
          letterSpacing: 1,
          textAlign: "center",
          border: `1.5px solid ${darkMode ? '#ffe082' : '#ffe082'}`,
          boxShadow: darkMode ? '0 4px 18px 0 #ffe08233' : "0 4px 18px 0 #ffe08233"
        }}>
          <span style={{ fontWeight: 700, color: "#ffb300", fontSize: 22, letterSpacing: 1 }}>
            언제 사용하나요?
          </span>
          <br />
          <span style={{ fontSize: 16.5 }}>
            정수 데이터, 데이터의 범위가 제한적일 때<br/>
            <span style={{ color: darkMode ? '#ff8a65' : '#b71c1c', fontWeight: 600 }}>음수/실수 데이터에는 부적합, 메모리 사용량 증가</span>
          </span>
        </div>
        {/* 초기 배열 영역 */}
        <div style={{
          margin: "0 0 34px 0",
          padding: "16px 28px 14px 28px",
          background: darkMode ? 'linear-gradient(90deg,#23272f 70%,#181c21 100%)' : "linear-gradient(90deg,#e3f2fd 70%,#fafdff 100%)",
          borderRadius: 12,
          fontWeight: 600,
          fontSize: 23,
          color: "#1565c0",
          letterSpacing: 2,
          textAlign: "center",
          boxShadow: "0 2px 12px 0 #90caf933",
          border: '1.2px solid #bbdefb',
        }}>
          초기 배열: [
          {arr && arr.map((v, i) => (
            <span key={i} style={{
              display: "inline-block",
              minWidth: 38,
              color: "#1976d2",
              fontWeight: 700,
              fontSize: 25,
              marginRight: i < arr.length - 1 ? 6 : 0
            }}>{v}{i < arr.length - 1 ? "," : ""}</span>
          ))}
          ]
        </div>
        {/* 그래프 시각화 - CountingSortAnimation */}
        <div style={{
          width: "100%",
          minHeight: 340,
          margin: '0 auto 48px auto',
          overflow: "visible",
          display: "flex",
          justifyContent: "center",
          alignItems: "center"
        }}>
          <CountingSortAnimation arr={arr} darkMode={darkMode} theme={theme} />
        </div>
        {/* 코드 영역 */}
        <div style={{
          width: "100%",
          marginBottom: 40,
          overflow: "visible",
          display: "flex",
          justifyContent: "center",
          alignItems: "center"
        }}>
          <CountingSortCode />
        </div>
      </div>
    </div>
  );
}
