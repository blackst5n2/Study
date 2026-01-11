import React, { useEffect, useState } from 'react';

export default function Home() {
  const [nickname, setNickname] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    // JWT 토큰 확인 (jwt 또는 token)
    const token = localStorage.getItem('jwt') || localStorage.getItem('token');
    if (!token) {
      window.location.href = '/';
      return;
    }
    // 서버에서 닉네임 불러오기
    fetch('http://localhost:4000/api/auth/me', {
      headers: { 'Authorization': `Bearer ${token}` }
    })
      .then(res => {
        if (!res.ok) throw new Error('인증 오류');
        return res.json();
      })
      .then(data => {
        setNickname(data.nickname || '모험가');
        setLoading(false);
      })
      .catch(() => {
        setError('로그인이 필요합니다.');
        setLoading(false);
        setTimeout(() => window.location.href = '/', 1400);
      });
  }, []);

  return (
    <div className="home-bg" style={{minHeight:'100vh',display:'flex',justifyContent:'center',alignItems:'center',background:'radial-gradient(ellipse at 60% 40%, #a18cd1 0%, #232b4d 70%)'}}>
      <div style={{background:'rgba(44,32,68,0.97)',padding:'48px 38px 38px 38px',borderRadius:'28px',boxShadow:'0 12px 48px #000b, 0 2px 18px #a18cd1bb',textAlign:'center',border:'2px solid #b98cff66',minWidth:320}}>
        {loading ? (
          <div style={{color:'#ffe082',fontFamily:'MedievalSharp, UnifrakturCook, cursive',fontSize:'1.35rem',padding:'28px 0'}}>로딩 중...</div>
        ) : error ? (
          <div style={{color:'#ff5e5e',fontSize:'1.18rem',fontWeight:600}}>{error}</div>
        ) : (
          <>
            <div style={{fontSize:'2.3rem',marginBottom:'12px'}}><span role="img" aria-label="guild">🏰</span></div>
            <h1 style={{color:'#ffe082',fontFamily:'MedievalSharp, UnifrakturCook, cursive',fontSize:'2.3rem',marginBottom:'16px',textShadow:'0 2px 12px #a18cd1cc'}}>어서오세요, <span style={{color:'#b98cff',textShadow:'0 0 8px #b98cff77'}}>{nickname}</span>님!</h1>
            <div style={{color:'#fff',fontSize:'1.25rem',marginBottom:'12px',fontFamily:'UnifrakturCook, MedievalSharp, cursive'}}>WIW Guild에 오신 것을 환영합니다.<br/>모험의 시작을 응원합니다!</div>
            <div style={{marginTop:'22px'}}>
              <button onClick={() => {localStorage.clear(); window.location.href='/'}} style={{padding:'12px 30px',borderRadius:'10px',border:'none',background:'linear-gradient(90deg,#a18cd1 0%,#b98cff 100%)',color:'#2e2347',fontWeight:600,fontSize:'1.07rem',fontFamily:'MedievalSharp, UnifrakturCook, cursive',cursor:'pointer',boxShadow:'0 1px 7px #a18cd133'}}>로그아웃</button>
            </div>
          </>
        )}
      </div>
    </div>
  );
}
