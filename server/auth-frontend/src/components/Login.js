import React, { useState } from 'react';
import GoogleLoginButton from './GoogleLoginButton';
import NicknameModal from './NicknameModal';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [message, setMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const [showNicknameModal, setShowNicknameModal] = useState(false);
  const [googleToken, setGoogleToken] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage('');
    try {
      const res = await fetch('http://localhost:4000/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
      });
      const data = await res.json();
      if (res.ok && data.token) {
        setMessage('로그인 성공!');
        localStorage.setItem('jwt', data.token);
        if (data.nickname) {
          localStorage.setItem('nickname', data.nickname);
        } else {
          localStorage.setItem('nickname', '모험가');
        }
        window.location.href = '/home'; // 홈으로 이동
      } else {
        setMessage(data.message || '로그인 실패');
      }
    } catch (err) {
      setMessage('서버 오류');
    } finally {
      setLoading(false);
    }
  };

  const handleGoogleSuccess = async (credentialResponse) => {
    setMessage('');
    setLoading(true);
    try {
      const res = await fetch('http://localhost:4000/api/auth/google', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ credential: credentialResponse.credential })
      });
      const data = await res.json();
      if (res.ok) {
        if (data.needNickname) {
          setGoogleToken(data.token); // 서버가 임시 토큰(닉네임 등록용) 반환
          setShowNicknameModal(true);
        } else {
          setMessage('구글 로그인 성공!');
          localStorage.setItem('token', data.token);
          if (data.nickname) {
            localStorage.setItem('nickname', data.nickname);
          } else {
            localStorage.setItem('nickname', '모험가');
          }
          window.location.href = '/home'; // 홈으로 이동
        }
      } else {
        setMessage(data.message || '구글 로그인 실패');
      }
    } catch (err) {
      setMessage('서버 오류');
    } finally {
      setLoading(false);
    }
  };

  // 닉네임 등록 핸들러
  const handleNicknameSubmit = async (nickname) => {
    setLoading(true);
    try {
      const res = await fetch('http://localhost:4000/api/auth/set-nickname', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${googleToken}` },
        body: JSON.stringify({ nickname })
      });
      const data = await res.json();
      if (res.ok) {
        setMessage('닉네임 설정 완료!');
        localStorage.setItem('token', data.token); // 새 토큰 저장
        if (data.nickname) {
          localStorage.setItem('nickname', data.nickname);
        } else {
          localStorage.setItem('nickname', '모험가');
        }
        setShowNicknameModal(false);
        window.location.href = '/home'; // 홈으로 이동
      } else {
        setMessage(data.message || '닉네임 등록 실패');
      }
    } catch (err) {
      setMessage('서버 오류');
    } finally {
      setLoading(false);
    }
  };


  return (
    <div className="login-bg">
      {showNicknameModal && (
        <NicknameModal
          onSubmit={handleNicknameSubmit}
          onClose={() => setShowNicknameModal(false)}
          loading={loading}
        />
      )}
      <form onSubmit={handleSubmit} className="login-form fantasy-glow">
        <div className="login-logo">
          <span className="login-title">WIW Login</span>
        </div>
        <input className="fantasy-input" value={email} onChange={e => setEmail(e.target.value)} placeholder="이메일 (Email)" type="email" required />
        <input className="fantasy-input" type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="비밀번호 (Password)" required />
        <button className="fantasy-btn" type="submit">🔑 로그인</button>
        <div className="fantasy-message">{message}</div>
        <div className="google-login-wrap">
  <GoogleLoginButton onSuccess={handleGoogleSuccess} />
</div>
        <div className="fantasy-signup-link">
          <span>계정이 없으신가요? </span>
          <a href="/signup">회원가입</a>
        </div>
      </form>
      <style>{`
        .fantasy-signup-link {
          margin: 18px 0 0 0;
          text-align: center;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          font-size: 0.96rem;
          color: #ffe082cc;
        }
        .fantasy-signup-link a {
          color: #b98cff;
          text-decoration: underline;
          font-weight: bold;
          margin-left: 3px;
          transition: color 0.2s;
        }
        .fantasy-signup-link a:hover {
          color: #ffe082;
          text-shadow: 0 0 6px #b98cff;
        }

        @import url('https://fonts.googleapis.com/css2?family=UnifrakturCook:wght@700&family=MedievalSharp&display=swap');
        .login-bg {
          min-height: 100vh;
          background: linear-gradient(135deg, #232b4d 0%, #2e1947 100%), url('https://www.transparenttextures.com/patterns/diamond-upholstery.png');
          display: flex;
          align-items: center;
          justify-content: center;
        }
        .fantasy-glow {
          background: rgba(40,32,64,0.98);
          border-radius: 14px;
          box-shadow: 0 0 18px 3px #9a6aff77, 0 2px 10px #000a;
          padding: 18px 10px 14px 10px;
          max-width: 320px;
          min-width: 320px;
          width: 100%;
          border: 1.5px solid #6a37d1;
          animation: fantasy-fadein 1.2s cubic-bezier(.47,1.64,.41,.8);
          overflow: hidden;
          margin: 0 auto;
        }
        .login-logo {
          display: flex;
          align-items: center;
          justify-content: center;
          margin-bottom: 10px;
        }
        .login-title {
          font-family: 'UnifrakturCook', 'MedievalSharp', cursive;
          font-size: 1.15rem;
          color: #ffe082;
          letter-spacing: 1px;
          text-shadow: 0 0 5px #fff7, 0 1px 8px #9a6aff;
        }
        .fantasy-input {
          width: 100%;
          max-width: 320px;
          min-width: 0;
          margin: 7px 0;
          padding: 7px 10px;
          border-radius: 5px;
          border: 1px solid #7b5be6;
          background: #2e2347;
          color: #ffe082;
          font-size: 0.92rem;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          box-shadow: 0 1px 3px #6a37d133 inset;
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
          max-width: 320px;
          min-width: 0;
          margin-top: 10px;
          padding: 8px 0;
          background: linear-gradient(90deg, #6a37d1 0%, #b98cff 100%);
          color: #fff5d1;
          font-family: 'UnifrakturCook', 'MedievalSharp', cursive;
          font-size: 0.98rem;
          border: none;
          border-radius: 5px;
          box-shadow: 0 0 7px #9a6aff99, 0 2px 4px #000a;
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
        .fantasy-message {
          min-height: 22px;
          margin-top: 12px;
          color: #ffe082;
          text-align: center;
          font-family: 'MedievalSharp', 'UnifrakturCook', cursive;
          font-size: 1.03rem;
          text-shadow: 0 0 6px #9a6aff99;
        }
        .google-login-wrap {
          margin: 18px auto 0 auto;
          width: 100%;
          max-width: 100%;
          background: none;
          border-radius: 0;
          box-shadow: none;
          padding: 0;
          display: flex;
          justify-content: center;
        }
        @keyframes fantasy-fadein {
          from { opacity: 0; transform: translateY(40px) scale(0.96); }
          to { opacity: 1; transform: none; }
        }
        @media (max-width: 600px) {
          .fantasy-glow { padding: 18px 4vw 18px 4vw; max-width: 98vw; }
          .login-title { font-size: 1.3rem; }
        }
      `}</style>
    </div>
  );
}
