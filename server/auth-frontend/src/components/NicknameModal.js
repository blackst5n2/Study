import React, { useState } from 'react';

export default function NicknameModal({ onSubmit, onClose, loading }) {
  const [nickname, setNickname] = useState('');
  const [checkMsg, setCheckMsg] = useState('');
  const [checking, setChecking] = useState(false);
  const [valid, setValid] = useState(null); // null|true|false

  // 닉네임 중복 체크
  // 닉네임 규칙 검사 함수 (프론트)
  function isNicknameValid(nickname) {
    if (typeof nickname !== 'string') return false;
    if (!nickname.trim() || nickname.length < 2 || nickname.length > 8) return false;
    if (!/^[가-힣a-zA-Z0-9]+$/.test(nickname)) return false;
    return true;
  }

  // 닉네임 중복 체크
  const checkNickname = async (value) => {
    if (!value) return;
    // 규칙 위반 시 fetch 호출하지 않고 안내
    if (!isNicknameValid(value)) {
      setValid(false);
      setCheckMsg('닉네임 규칙을 확인하세요.');
      return;
    }
    setChecking(true);
    setValid(null);
    setCheckMsg('');
    try {
      const resp = await fetch(`http://localhost:4000/api/auth/check-nickname?nickname=${encodeURIComponent(value)}`);
      const data = await resp.json();
      if (data.exists) {
        setValid(false);
        setCheckMsg('이미 사용 중인 닉네임입니다.');
      } else {
        setValid(true);
        setCheckMsg('사용 가능한 닉네임입니다.');
      }
    } catch {
      setValid(false);
      setCheckMsg('닉네임 중복 확인 실패');
    } finally {
      setChecking(false);
    }
  };

  const handleChange = (e) => {
    const value = e.target.value;
    setNickname(value);
    setValid(null);
    setCheckMsg('');
    // 입력 즉시 규칙 위반 안내
    if (value && !isNicknameValid(value)) {
      setValid(false);
      setCheckMsg('닉네임 규칙을 확인하세요.');
    }
  };

  const handleBlur = () => {
    if (nickname) checkNickname(nickname);
  };


  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!nickname) {
      setCheckMsg('닉네임을 입력하세요.');
      setValid(false);
      return;
    }
    await checkNickname(nickname);
    if (valid) {
      onSubmit(nickname);
    }
  };

  return (
    <div className="nickname-modal-bg">
      <div className="nickname-modal-card">
        <h2><span role="img" aria-label="user">🧙‍♂️</span> 닉네임 설정</h2>
        <form onSubmit={handleSubmit} autoComplete="off">
          <div className="nickname-input-wrap">
            <span className="nickname-input-icon">✨</span>
            <input
              className="nickname-input"
              type="text"
              value={nickname}
              onChange={handleChange}
              onBlur={handleBlur}
              placeholder="닉네임을 입력하세요"
              minLength={2}
              maxLength={8}
              required
              autoFocus
              disabled={loading}
            />
          </div>
          <div className="nickname-rule-msg">
            2~8자, 한글/영문/숫자만 사용 가능 (공백·특수문자 불가)
          </div>
          <div className={"nickname-check-msg " + (valid === true ? 'success' : valid === false ? 'error' : '')}>
            {checking ? '중복 확인 중...' : checkMsg}
          </div>
          <div className="nickname-btn-row">
            <button className="nickname-btn confirm" type="submit" disabled={loading || valid === false || checking}>확인</button>
            <button type="button" className="nickname-btn cancel" onClick={onClose} disabled={loading}>취소</button>
          </div>
        </form>
      </div>
      <style>{`
        .nickname-modal-bg {
          position: fixed; left: 0; top: 0; width: 100vw; height: 100vh;
          z-index: 1000; background: rgba(34,24,54,0.55);
          display: flex; align-items: center; justify-content: center;
          backdrop-filter: blur(2.5px) brightness(0.9);
        }
        .nickname-modal-card {
          background: linear-gradient(135deg, #2e2347 60%, #3d256a 100%);
          border-radius: 22px;
          box-shadow: 0 8px 36px #000c, 0 2px 18px #a18cd1bb;
          padding: 38px 24px 28px 24px;
          min-width: 320px; max-width: 95vw;
          color: #ffe082;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          border: 1.5px solid #b98cff66;
          animation: modal-pop 0.7s cubic-bezier(.47,1.64,.41,.8);
          box-sizing: border-box;
        }
        .nickname-modal-card h2 {
          margin-top: 0; margin-bottom: 22px;
          font-size: 1.38rem;
          color: #ffe082;
          letter-spacing: 1px;
          text-shadow: 0 0 8px #fff7, 0 2px 12px #9a6aff;
          display: flex; align-items: center; gap: 8px;
          justify-content: center;
        }
        .nickname-input-wrap {
          position: relative;
          margin-bottom: 8px;
          width: 100%;
          box-sizing: border-box;
        }
        .nickname-input-icon {
          position: absolute; left: 12px; top: 50%; transform: translateY(-50%);
          font-size: 1.1em; color: #b98cffcc;
          pointer-events: none;
        }
        .nickname-input {
          width: 100%;
          max-width: 100%;
          min-width: 0;
          padding: 10px 12px 10px 36px;
          border-radius: 8px;
          border: 1.5px solid #a18cd1;
          background: rgba(255,255,255,0.08);
          color: #ffe082;
          font-size: 1.07rem;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          box-shadow: 0 1px 3px #6a37d133 inset;
          outline: none;
          transition: border-color 0.2s, box-shadow 0.2s, background 0.2s;
          box-sizing: border-box;
        }
        .nickname-input:focus {
          border-color: #ffe082;
          box-shadow: 0 0 12px #ffe08266, 0 2px 8px #9a6aff55;
          background: rgba(255,255,255,0.17);
        }
        .nickname-check-msg {
          min-height: 20px;
          font-size: 1.04rem;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          color: #ffe082cc;
          transition: color 0.2s;
          margin-top: 4px; margin-bottom: 2px;
          text-align: center;
        }
        .nickname-check-msg.error {
          color: #ff5e5e;
        }
        .nickname-check-msg.success {
          color: #7fff7f;
        }
        .nickname-rule-msg {
          color: #ffe082cc;
          font-size: 0.98rem;
          margin-bottom: 2px;
          margin-top: -4px;
          text-align: center;
          letter-spacing: 0.01em;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          text-shadow: 0 1px 4px #2e234799;
        }
        .nickname-btn-row {
          display: flex; gap: 12px; margin-top: 20px; justify-content: center;
        }
        .nickname-btn {
          flex: 1 1 0;
          padding: 10px 0;
          border: none;
          border-radius: 8px;
          font-size: 1.05rem;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          font-weight: 600;
          cursor: pointer;
          transition: background 0.18s, color 0.18s, box-shadow 0.18s;
          box-shadow: 0 1px 7px #a18cd133;
        }
        .nickname-btn.confirm {
          background: linear-gradient(90deg, #a18cd1 0%, #b98cff 100%);
          color: #2e2347;
        }
        .nickname-btn.confirm:disabled {
          background: #b98cff66;
          color: #2e2347aa;
          cursor: not-allowed;
        }
        .nickname-btn.cancel {
          background: #444a5a;
          color: #ffe082cc;
        }
        .nickname-btn.cancel:disabled {
          background: #444a5a66;
          color: #ffe08255;
          cursor: not-allowed;
        }
        @keyframes modal-pop {
          from { opacity: 0; transform: scale(0.93) translateY(30px); }
          to { opacity: 1; transform: none; }
        }
        @media (max-width: 500px) {
          .nickname-modal-card { padding: 18px 4vw 18px 4vw; min-width: 0; }
          .nickname-modal-card h2 { font-size: 1.08rem; }
        }
      `}</style>
    </div>
  );
}
