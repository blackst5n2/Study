import React, { useState } from 'react';
import { Link } from 'react-router-dom';

export default function Signup() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [nickname, setNickname] = useState('');
  const [password2, setPassword2] = useState('');
  const [msg, setMsg] = useState('');
  const [nicknameCheck, setNicknameCheck] = useState(null); // null | true | false | 'loading'
  const [nicknameCheckMsg, setNicknameCheckMsg] = useState('');
  const [agree, setAgree] = useState(false);
  // debounce timer
  const nicknameTimer = React.useRef();

  // 닉네임 규칙 검사 함수 (공백 포함, 2~8자, 한글/영문/숫자만)
  function isNicknameValid(nick) {
    if (typeof nick !== 'string') return false;
    if (!nick || nick.length < 2 || nick.length > 8) return false;
    if (!/^[가-힣a-zA-Z0-9]+$/.test(nick)) return false;
    return true;
  }

  // 비밀번호 규칙 검사 함수 (8~20자, 영문/숫자/특수문자 포함)
  function isPasswordValid(pw) {
    if (typeof pw !== 'string') return false;
    if (pw.length < 8 || pw.length > 20) return false;
    if (!/[a-zA-Z]/.test(pw)) return false;
    if (!/[0-9]/.test(pw)) return false;
    if (!/[~!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(pw)) return false;
    return true;
  }

  const handleSubmit = async (e) => {
    e.preventDefault();

    setMsg('');
    if (!email || !nickname || !password || !password2) {
      setMsg('모든 항목을 입력하세요.');
      return;
    }
    if (!agree) {
      setMsg('이용약관 및 개인정보 처리방침에 동의해야 합니다.');
      return;
    }
    if (nicknameCheck === true) {
      setMsg('이미 사용 중인 닉네임입니다.');
      return;
    }
    if (!isPasswordValid(password)) {
      setMsg('비밀번호는 8~20자, 영문/숫자/특수문자를 모두 포함해야 합니다.');
      return;
    }
    if (password !== password2) {
      setMsg('비밀번호가 일치하지 않습니다.');
      return;
    }
    try {
      const res = await fetch('http://localhost:4000/api/auth/signup', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password, nickname })
      });
      const data = await res.json();
      if (res.ok) {
        setMsg('회원가입 성공! 로그인 페이지로 이동합니다.');
        setTimeout(() => { window.location.href = '/login'; }, 1200);
      } else {
        setMsg(data.message || '회원가입 실패');
      }
    } catch (err) {
      setMsg('서버 오류');
    }
  };

  return (
    <div className="signup-bg">
      <form onSubmit={handleSubmit} className="signup-form fantasy-glow">
        <div className="signup-logo">
          <span role="img" aria-label="dragon" style={{fontSize:'2.2rem',marginRight:'8px'}}>🐉</span>
          <span className="signup-title">WIW Guild Registration</span>
        </div>
        {/* 닉네임 규칙 검사 함수 (컴포넌트 상단에 위치) */}
        <input className="fantasy-input" value={nickname} onChange={e => {
          const value = e.target.value;
          setNickname(value);
          setNicknameCheck(null);
          setNicknameCheckMsg('');
          if (nicknameTimer.current) clearTimeout(nicknameTimer.current);
          if (!value) return;
          // 닉네임 규칙 검사 (2~8자, 한글/영문/숫자만, 공백 불가)
          if (!isNicknameValid(value)) {
            setNicknameCheck(true);
            setNicknameCheckMsg('닉네임 규칙을 확인하세요.');
            return;
          }
          setNicknameCheck('loading');
          nicknameTimer.current = setTimeout(async () => {
            try {
              const resp = await fetch(`http://localhost:4000/api/auth/check-nickname?nickname=${encodeURIComponent(value)}`);
              const data = await resp.json();
              if (data.exists) {
                setNicknameCheck(true);
                setNicknameCheckMsg('이미 사용 중인 닉네임입니다.');
              } else {
                setNicknameCheck(false);
                setNicknameCheckMsg('사용 가능한 닉네임입니다.');
              }
            } catch {
              setNicknameCheck(true);
              setNicknameCheckMsg('닉네임 중복 확인 실패');
            }
          }, 400);
        }} placeholder="닉네임 (Nickname)" type="text" minLength={2} maxLength={8} required />
        <div className="nickname-rule-msg" style={{textAlign:'left',marginLeft:2,marginTop:'2px',marginBottom:'2px',color:'#ffe082cc',fontSize:'0.97rem',fontFamily:'MedievalSharp, UnifrakturCook, cursive',textShadow:'0 1px 4px #2e234799'}}>2~8자, 한글/영문/숫자만 사용 가능 (공백·특수문자 불가)</div>
        <div className="nickname-check-msg" style={{ marginTop:2,minHeight: 18, textAlign: 'left', marginLeft: 2, fontSize: '0.98rem', fontFamily: 'MedievalSharp, UnifrakturCook, cursive', color: nicknameCheck === true ? '#ff5e5e' : nicknameCheck === false ? '#7fff7f' : '#ffe082cc' }}>
          {nicknameCheck === 'loading' ? '닉네임 중복 확인 중...' : nicknameCheckMsg}
        </div>
        <input className="fantasy-input" value={email} onChange={e => setEmail(e.target.value)} placeholder="이메일 (Email)" type="email" required />
        <input className="fantasy-input" type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="비밀번호 (Password)" required />
      <div style={{textAlign:'left',marginLeft:2,marginTop:'2px',marginBottom:'2px',color:'#ffe082cc',fontSize:'0.97rem',fontFamily:'MedievalSharp, UnifrakturCook, cursive',textShadow:'0 1px 4px #2e234799'}}>
        비밀번호는 8~20자, 영문/숫자/특수문자를 모두 포함해야 합니다.
      </div>
      {password && (
        <div style={{
          margin:'-2px 0 8px 2px',
          fontSize:'0.97rem',
          fontFamily:'MedievalSharp, UnifrakturCook, cursive',
          color: isPasswordValid(password) ? '#7fff7f' : '#ff5e5e',
          minHeight: '18px',
          textAlign:'left',
          fontWeight:500,
          textShadow:'0 1px 4px #2e234799'
        }}>
          {isPasswordValid(password)
            ? '비밀번호 규칙을 만족합니다.'
            : '비밀번호가 규칙에 맞지 않습니다.'}
        </div>
      )}
        <input className="fantasy-input" type="password" value={password2} onChange={e => setPassword2(e.target.value)} placeholder="비밀번호 확인 (Repeat Password)" required />
        {(password2 && password) && (
          <div style={{
            minHeight: '18px',
            margin: '4px 0 0 2px',
            fontSize: '0.98rem',
            fontFamily: 'MedievalSharp, UnifrakturCook, cursive',
            color: password === password2 ? '#7fff7f' : '#ff5e5e',
            textAlign: 'left',
            fontWeight: 500
          }}>
            {password === password2 ? '비밀번호가 일치합니다.' : '비밀번호가 일치하지 않습니다.'}
          </div>
        )}
        <div className="fantasy-terms-check">
          <label style={{ display: 'flex', alignItems: 'flex-start', gap: '8px', width: '100%', fontSize: '0.93rem', color: '#ffe082cc', fontFamily: 'MedievalSharp, UnifrakturCook, cursive', fontWeight: 400, lineHeight: 1.5, cursor: 'pointer', flexWrap: 'wrap' }}>
            <input type="checkbox" checked={agree} onChange={e => setAgree(e.target.checked)} style={{ accentColor: '#b98cff', width: 17, height: 17, marginTop: 2, flexShrink: 0, marginRight: 5 }} />
            <span style={{ display: 'inline', wordBreak: 'keep-all', color: '#ffe082cc' }}>
              <a href="/terms" target="_blank" rel="noopener noreferrer" style={{ color: '#b98cff', textDecoration: 'underline', margin: '0 2px' }}>이용약관</a> 및 <a href="/privacy" target="_blank" rel="noopener noreferrer" style={{ color: '#b98cff', textDecoration: 'underline', margin: '0 2px' }}>개인정보 처리방침</a>에 동의합니다.
            </span>
          </label>
        </div>
        <button className="fantasy-btn" type="submit" disabled={!agree} style={!agree ? { opacity: 0.6, cursor: 'not-allowed' } : {}}>🗡️ 가입하기</button>
        <div className="fantasy-message">{msg}</div>
        <div className="fantasy-login-link">
        <span>이미 계정이 있으신가요? </span>
        <Link to="/login">로그인</Link>
      </div>
      </form>
      
      <style>{`
        .fantasy-login-link {
          margin: 22px 0 0 0;
          text-align: center;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          font-size: 0.96rem;
          color: #ffe082cc;
        }
        .fantasy-login-link a {
          color: #b98cff;
          text-decoration: underline;
          font-weight: bold;
          margin-left: 3px;
          transition: color 0.2s;
        }
        .fantasy-login-link a:hover {
          color: #ffe082;
          text-shadow: 0 0 6px #b98cff;
        }

        @import url('https://fonts.googleapis.com/css2?family=UnifrakturCook:wght@700&family=MedievalSharp&display=swap');
        .signup-bg {
          min-height: 100vh;
          background: linear-gradient(135deg, #232b4d 0%, #2e1947 100%), url('https://www.transparenttextures.com/patterns/diamond-upholstery.png');
          display: flex;
          align-items: center;
          justify-content: center;
        }
        .fantasy-glow {
          background: rgba(40,32,64,0.98);
          border-radius: 16px;
          box-shadow: 0 0 28px 4px #9a6aff88, 0 2px 12px #000a;
          padding: 24px 18px 22px 18px;
          max-width: 360px;
          min-width: 360px;
          width: 100%;
          border: 2px solid #6a37d1;
          animation: fantasy-fadein 1.2s cubic-bezier(.47,1.64,.41,.8);
          overflow: hidden;
          margin: 0 auto;
        }
        .signup-logo {
          display: flex;
          align-items: center;
          justify-content: center;
          margin-bottom: 18px;
        }
        .signup-title {
          font-family: 'UnifrakturCook', 'MedievalSharp', cursive;
          font-size: 1.55rem;
          color: #ffe082;
          letter-spacing: 1px;
          text-shadow: 0 0 8px #fff7, 0 2px 12px #9a6aff;
        }
        .fantasy-input {
          width: 100%;
          max-width: 360px;
          min-width: 0;
          margin: 14px 0 0 0;
          height: 44px;
          padding: 0 14px;
          border-radius: 7px;
          border: 1.5px solid #7b5be6;
          background: #2e2347;
          color: #ffe082;
          font-size: 1.07rem;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          box-shadow: 0 2px 8px #6a37d155 inset;
          outline: none;
          transition: border-color 0.2s, box-shadow 0.2s;
          box-sizing: border-box;
          display: block;
          margin-left: auto;
          margin-right: auto;
        }
        .fantasy-input:focus {
          border-color: #ffe082;
          box-shadow: 0 0 12px #ffe08255, 0 2px 8px #9a6aff55;
        }
        .fantasy-btn {
          width: 100%;
          max-width: 360px;
          min-width: 0;
          margin-top: 16px;
          height: 44px;
          padding: 0;
          background: linear-gradient(90deg, #6a37d1 0%, #b98cff 100%);
          color: #fff5d1;
          font-family: 'UnifrakturCook', 'MedievalSharp', cursive;
          font-size: 1.15rem;
          border: none;
          border-radius: 7px;
          box-shadow: 0 0 12px #9a6aff99, 0 2px 8px #000a;
          cursor: pointer;
          transition: background 0.2s, box-shadow 0.2s;
          display: block;
          margin-left: auto;
          margin-right: auto;
        }
        .fantasy-btn:hover {
          background: linear-gradient(90deg, #b98cff 0%, #6a37d1 100%);
          box-shadow: 0 0 18px #ffe082bb, 0 2px 16px #9a6affbb;
        }
        .nickname-check-msg {
  margin-top: 3px;
  margin-bottom: -8px;
  min-height: 18px;
  font-weight: bold;
}
.fantasy-terms-check {
  margin: 12px 0 0 0;
  min-height: 20px;
  width: 100%;
}
.fantasy-message {
          min-height: 22px;
          margin-top: 12px;
          color: #ffe082;
          text-align: center;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          font-size: 1.03rem;
          text-shadow: 0 0 6px #9a6aff99;
        }
        @keyframes fantasy-fadein {
          from { opacity: 0; transform: translateY(40px) scale(0.96); }
          to { opacity: 1; transform: none; }
        }
        @media (max-width: 600px) {
          .fantasy-glow { padding: 18px 4vw 18px 4vw; max-width: 98vw; }
          .signup-title { font-size: 1.3rem; }
        }
      `}</style>
    </div>
  );
}
