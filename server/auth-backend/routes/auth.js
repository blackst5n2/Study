const express = require('express');
const jwt = require('jsonwebtoken');
const bcrypt = require('bcrypt');
const SECRET = process.env.JWT_SECRET;
const { OAuth2Client } = require('google-auth-library');
const GOOGLE_CLIENT_ID = process.env.GOOGLE_CLIENT_ID || 'YOUR_GOOGLE_CLIENT_ID.apps.googleusercontent.com';
const googleClient = new OAuth2Client(GOOGLE_CLIENT_ID);

// === 닉네임/비밀번호 필터링 ===
const bannedWords = [
  'admin', '운영자', 'fuck', 'shit', 'sex', '시발', '병신', 'ㅅㅂ', '관리자', '운영', 'staff', 'fuckyou', 'fuck you', '씨발',
  '애플', '구글', '관리', 'gm', '운영진', 'fuckyou', 'fuck you'
];
function isNicknameValid(nickname) {
  if (typeof nickname !== 'string') return false;
  if (!nickname.trim() || nickname.length < 2 || nickname.length > 8) return false;
  if (!/^[가-힣a-zA-Z0-9]+$/.test(nickname)) return false;
  const lower = nickname.toLowerCase();
  if (bannedWords.some(word => lower.includes(word))) return false;
  return true;
}

// 비밀번호 규칙: 8~20자, 영문/숫자/특수문자 모두 포함
function isPasswordValid(password) {
  if (typeof password !== 'string') return false;
  if (password.length < 8 || password.length > 20) return false;
  if (!/[a-zA-Z]/.test(password)) return false;
  if (!/[0-9]/.test(password)) return false;
  if (!/[~!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(password)) return false;
  return true;
}

const cors = require('cors');
const helmet = require('helmet');
const csurf = require('csurf');

module.exports = (User) => {
  const router = express.Router();

  // === 글로벌 보안 미들웨어 ===
  router.use(cors({
    origin: [/^http:\/\/localhost(:\d+)?$/, /^https:\/\/yourdomain\.com$/], // 허용 도메인 수정 필요
    credentials: true
  }));
  router.use(helmet({
    contentSecurityPolicy: {
      directives: {
        defaultSrc: ["'self'"],
        scriptSrc: ["'self'", 'https://apis.google.com'],
        styleSrc: ["'self'", 'https://fonts.googleapis.com'],
        objectSrc: ["'none'"],
        upgradeInsecureRequests: [],
      }
    },
    xssFilter: true,
    crossOriginEmbedderPolicy: true,
    crossOriginResourcePolicy: true,
    crossOriginOpenerPolicy: true,
  }));
  // CSRF 토큰은 쿠키에 저장, 클라이언트에서 X-CSRF-Token 헤더로 전송
  router.use(csurf({ cookie: true }));


  // nodemailer 설정 (테스트용 Gmail SMTP)
  const nodemailer = require('nodemailer');
  const crypto = require('crypto');
  const transporter = nodemailer.createTransport({
    service: 'gmail',
    auth: {
      user: process.env.GMAIL_USER || 'your_gmail@gmail.com',
      pass: process.env.GMAIL_PASS || 'your_gmail_app_password'
    }
  });

  // 로그인
  router.post('/login', async (req, res) => {
    const { email, password } = req.body;
    const now = Date.now();
    const attempt = loginAttempts.get(email) || { count: 0, lastFail: 0 };
    if (attempt.count >= 5 && now - attempt.lastFail < 10 * 60 * 1000) {
      return res.status(429).json({ message: '로그인 시도 초과. 10분 후 다시 시도하세요.' });
    }
    const user = await User.findOne({ where: { email } });
    if (!user) {
      loginAttempts.set(email, { count: attempt.count + 1, lastFail: now });
      return res.status(401).json({ message: '로그인 실패' });
    }
    if (!user.isVerified) return res.status(403).json({ message: '이메일 인증 필요' });
    const isMatch = await bcrypt.compare(password, user.password);
    if (!isMatch) {
      loginAttempts.set(email, { count: attempt.count + 1, lastFail: now });
      return res.status(401).json({ message: '로그인 실패' });
    }
    loginAttempts.delete(email); // 성공 시 초기화
    // --- 실제 DB에서 role, id를 읽어 JWT에 포함 ---
    // Sequelize User 모델에 role, id 필드가 있다고 가정
    const role = user.role || 'user'; // 기본값 user
    const userId = user.id; // PK
    const claims = {
      email: user.email,
      nickname: user.nickname,
      role,
      userId
    };
    const token = jwt.sign(claims, SECRET, { expiresIn: '30m', issuer: 'AUTH_SERVER', audience: 'GAME_SERVER', header: { kid: 'main-key' } });
    // RefreshToken: 로그인 성공 시마다 새로 발급(다중 기기 지원)
    const newRefreshToken = require('crypto').randomBytes(32).toString('hex');
    const expiresAt = Date.now() + 30 * 24 * 60 * 60 * 1000; // 30일 후 만료
    const userAgent = req.headers['user-agent'] || '';
    const ip = req.headers['x-forwarded-for'] || req.connection.remoteAddress || '';
    const createdAt = Date.now();
    user.refreshTokens = Array.isArray(user.refreshTokens) ? user.refreshTokens : [];
    const agent = useragent.parse(userAgent);
const deviceType = agent.device.toString() !== 'Other' ? agent.device.toString() : (agent.os.toString().toLowerCase().includes('android') || agent.os.toString().toLowerCase().includes('ios') ? 'Mobile' : 'PC');
user.refreshTokens.push({ token: newRefreshToken, expiresAt, userAgent, ip, createdAt, deviceType, lastUsedAt: createdAt });
    // 만료된 토큰 자동 삭제
    user.refreshTokens = user.refreshTokens.filter(rt => !rt.expiresAt || rt.expiresAt > Date.now());
    if (user.refreshTokens.length > 5) user.refreshTokens = user.refreshTokens.slice(-5);
    await user.save();
    if (role === 'admin') {
      return res.json({ token, nickname: user.nickname, refreshToken: newRefreshToken, expiresAt, admin: true });
    }
    res.json({ token, nickname: user.nickname, refreshToken: newRefreshToken, expiresAt });
  });

  // 닉네임 중복 체크
  router.get('/check-nickname', async (req, res) => {
    const { nickname } = req.query;
    if (!nickname) return res.status(400).json({ message: '닉네임을 입력하세요.' });
    const exists = await User.findOne({ where: { nickname } });
    res.json({ exists: !!exists });
  });

  // 회원가입
  router.post('/signup', async (req, res) => {
    const { email, password, nickname, role } = req.body;
    if (!email || !password || !nickname) return res.status(400).json({ message: '모든 항목을 입력하세요.' });
    if (!isPasswordValid(password)) return res.status(400).json({ message: '비밀번호는 8~20자, 영문/숫자/특수문자를 모두 포함해야 합니다.' });
    const exists = await User.findOne({ where: { email } });
    if (exists) return res.status(409).json({ message: '이미 가입된 이메일' });
    // 닉네임 필터링
    if (!isNicknameValid(nickname)) {
      return res.status(400).json({ message: '사용할 수 없는 닉네임입니다.' });
    }
    const nicknameExists = await User.findOne({ where: { nickname } });
    if (nicknameExists) return res.status(409).json({ message: '이미 사용 중인 닉네임' });
    const hash = await bcrypt.hash(password, 10);
    const verifyToken = crypto.randomBytes(32).toString('hex');
    const refreshToken = require('crypto').randomBytes(32).toString('hex');
    const user = await User.create({
      email,
      password: hash,
      nickname,
      role: role || 'user', // 관리자 가입 시 role 지정 가능
      verifyToken,
      isVerified: false,
      refreshToken
    });
    // JWT 바로 발급(선택)
    const claims = {
      email: user.email,
      nickname: user.nickname,
      role: user.role,
      userId: user.id
    };
    const token = jwt.sign(claims, SECRET, { expiresIn: '30m', issuer: 'AUTH_SERVER', audience: 'GAME_SERVER', header: { kid: 'main-key' } });
    res.json({
      message: '회원가입 성공! 이메일 인증을 완료하세요.',
      user: {
        userId: user.id,
        email: user.email,
        nickname: user.nickname,
        role: user.role
      },
      token,
      refreshToken
    });
  });

  // 이메일 인증 엔드포인트
  router.get('/verify', async (req, res) => {
    const { token } = req.query;
    if (!token) return res.status(400).send('토큰이 없습니다.');
    const user = await User.findOne({ where: { verifyToken: token } });
    if (!user) return res.status(400).send('유효하지 않은 토큰입니다.');
    user.isVerified = true;
    user.verifyToken = null;
    await user.save();
    res.send('이메일 인증이 완료되었습니다! 이제 로그인할 수 있습니다.');
  });

  // 구글 OAuth 로그인 (콜백 지원)
  // 1. 구글 OAuth 인증 시작 (브라우저 리디렉션)
  router.get('/google', (req, res) => {
    const { callback } = req.query;
    const redirectUri = encodeURIComponent('http://localhost:4000/api/auth/google/callback');
    const state = encodeURIComponent(Buffer.from(JSON.stringify({ callback })).toString('base64'));
    const authUrl = `https://accounts.google.com/o/oauth2/v2/auth?` +
      `client_id=${GOOGLE_CLIENT_ID}` +
      `&redirect_uri=${redirectUri}` +
      `&response_type=code` +
      `&scope=openid%20email%20profile` +
      `&access_type=online` +
      `&state=${state}`;
    res.redirect(authUrl);
  });

  // 2. 구글 OAuth 콜백 (code → 토큰 교환 → 사용자 정보 → JWT 발급 → callback 리디렉션)
  router.get('/google/callback', async (req, res) => {
    const { code, state } = req.query;
    // 구글 콜백 파라미터 처리
    let callback = null;
    if (state) {
      try {
        const decoded = JSON.parse(Buffer.from(decodeURIComponent(state), 'base64').toString());
        callback = decoded.callback;
      } catch (e) { /* 무시 */ }
    }
    if (!code) return res.status(400).send('구글 인증 코드 없음');

    try {
      // code → 토큰 교환 (axios 사용)
      const axios = require('axios');
      const qs = require('querystring');
      const tokenRes = await axios.post('https://oauth2.googleapis.com/token',
        qs.stringify({
          code,
          client_id: GOOGLE_CLIENT_ID,
          client_secret: process.env.GOOGLE_CLIENT_SECRET,
          redirect_uri: 'http://localhost:4000/api/auth/google/callback',
          grant_type: 'authorization_code'
        }),
        {
          headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
          }
        }
      );
      const tokens = tokenRes.data;
      // id_token 검증 및 사용자 정보 추출
      const ticket = await googleClient.verifyIdToken({ idToken: tokens.id_token, audience: GOOGLE_CLIENT_ID });
      const payload = ticket.getPayload();
      const email = payload.email;
      const oauthId = payload.sub;
      // DB에서 사용자 확인/생성
      let user = await User.findOne({ where: { email, provider: 'google', oauthId } });
      if (!user) {
        user = await User.create({ email, nickname: null, provider: 'google', oauthId, password: '-' });
      }
      // 닉네임 미설정: 임시 토큰 발급 및 needNickname: true 반환
      const tempToken = jwt.sign({ email: user.email, provider: 'google', oauthId, setNickname: true }, SECRET, { expiresIn: '10m', header: { kid: 'main-key' } });
      if (callback) {
        const url = `${callback}${callback.endsWith('/') ? '' : '/'}?token=${tempToken}`;
        return res.redirect(url);
      }
      return res.json({ needNickname: true, token: tempToken });
    } catch (err) {
      console.error('GOOGLE TOKEN ERROR:', err);
      if (err.response && err.response.data) {
        console.error('GOOGLE TOKEN ERROR RESPONSE DATA:', err.response.data);
      }
      res.status(500).json({ message: '구글 콜백 처리 오류', error: err.message });
    }
  });

  // 기존 POST /google 라우트(앱에서 credential 직접 받는 경우)
  router.post('/google', async (req, res) => {
    const { credential, callback } = req.body;
    if (!credential) return res.status(400).json({ message: '구글 토큰이 필요합니다.' });
    try {
      // 구글 id_token에서 정보 추출(클라이언트 신뢰 금지)
      const ticket = await googleClient.verifyIdToken({ idToken: credential, audience: GOOGLE_CLIENT_ID });
      const payload = ticket.getPayload();
      const email = payload.email;
      const oauthId = payload.sub;
      // provider, email, oauthId만 활용
      let user = await User.findOne({ where: { email, provider: 'google', oauthId } });
      if (!user) {
        user = await User.create({ email, nickname: null, provider: 'google', oauthId, password: '-', refreshToken: require('crypto').randomBytes(32).toString('hex') });
      }
      if (!user.refreshToken) {
        user.refreshToken = require('crypto').randomBytes(32).toString('hex');
        await user.save();
      }
      if (!user.nickname) {
        // 임시 토큰(닉네임 등록용)
        const tempToken = jwt.sign({
          email: user.email,
          provider: 'google',
          oauthId,
          setNickname: true
        }, SECRET, { expiresIn: '10m', header: { kid: 'main-key' } });
        return res.json({ needNickname: true, token: tempToken, refreshToken: user.refreshToken });
      }
      // 정식 토큰(최소 정보만 포함)
      const token = jwt.sign({
        email: user.email,
        nickname: user.nickname,
        provider: 'google',
        oauthId
      }, SECRET, { expiresIn: '1h', header: { kid: 'main-key' } });
      if (callback) {
        const url = `${callback}${callback.endsWith('/') ? '' : '/'}?token=${token}`;
        return res.redirect(url);
      }
      res.json({ message: '구글 로그인 성공', token, nickname: user.nickname, refreshToken: user.refreshToken });
    } catch (err) {
      console.error('Google Token Verify Error:', err);
      res.status(401).json({ message: '구글 토큰 검증 실패', error: err.message });
    }
  });

  // JWT로 사용자 정보 반환
  router.get('/me', async (req, res) => {
    const auth = req.headers.authorization;
    if (!auth || !auth.startsWith('Bearer ')) return res.status(401).json({ message: '인증 필요' });
    const token = auth.replace('Bearer ', '');
    let payload;
    try {
      payload = jwt.verify(token, SECRET);
    } catch (err) {
      return res.status(401).json({ message: '인증 오류' }); // 상세 정보 숨김
    }
    // DB에서 닉네임 등 최신 정보 조회
    const user = await User.findOne({ where: { email: payload.email } });
    if (!user) return res.status(404).json({ message: '인증 오류' });
    res.json({ email: user.email, nickname: user.nickname, refreshToken: user.refreshToken });
  });

  // 구글 계정용 닉네임 설정 엔드포인트
  router.post('/set-nickname', async (req, res) => {
    try {
      // Authorization: Bearer <tempToken>
      const auth = req.headers.authorization;
      if (!auth || !auth.startsWith('Bearer ')) return res.status(401).json({ message: '인증 필요' });
      const token = auth.replace('Bearer ', '');
      let payload;
      try {
        payload = jwt.verify(token, SECRET);
      } catch (err) {
        return res.status(401).json({ message: '인증 오류' }); // 상세 정보 숨김
      }
      if (!payload.setNickname || !payload.email || !payload.provider || !payload.oauthId) {
        return res.status(400).json({ message: '인증 오류' });
      }
      const { nickname } = req.body;
      if (!nickname) return res.status(400).json({ message: '닉네임을 입력하세요.' });
      // 닉네임 필터링
      if (!isNicknameValid(nickname)) {
        return res.status(400).json({ message: '닉네임을 사용할 수 없습니다.' });
      }
      const exists = await User.findOne({ where: { nickname } });
      if (exists) return res.status(409).json({ message: '닉네임을 사용할 수 없습니다.' });
      // 닉네임 등록
      const user = await User.findOne({ where: { email: payload.email, provider: 'google', oauthId: payload.oauthId } });
      if (!user) return res.status(404).json({ message: '인증 오류' });
      user.nickname = nickname;
      await user.save();
      // 새 JWT 발급
      const newToken = jwt.sign({ email: user.email, nickname: user.nickname, provider: 'google', oauthId: user.oauthId }, SECRET, { expiresIn: '1h', header: { kid: 'main-key' } });
      res.json({ message: '닉네임 등록 완료', token: newToken, nickname: user.nickname });
    } catch (err) {
      console.error('닉네임 등록 서버 오류:', err);
      res.status(500).json({ message: '닉네임 등록 서버 오류', error: err.message, stack: err.stack });
    }
  });

  // RefreshToken으로 JWT 갱신(만료/회수/rotation/다중 기기)
  router.post('/refresh', async (req, res) => {
    const { refreshToken } = req.body;
    if (!refreshToken) return res.status(400).json({ message: 'refreshToken 필요' });
    // 토큰 구조 [{token, expiresAt}]
    const user = await User.findOne({ where: {} });
    if (!user || !Array.isArray(user.refreshTokens)) return res.status(401).json({ message: '유효하지 않은 refreshToken' });
    // 만료된 토큰 자동 삭제
    user.refreshTokens = user.refreshTokens.filter(rt => !rt.expiresAt || rt.expiresAt > Date.now());
    const idx = user.refreshTokens.findIndex(rt => rt.token === refreshToken);
    if (idx === -1) return res.status(401).json({ message: '유효하지 않은 refreshToken' });
    // 만료 확인
    if (user.refreshTokens[idx].expiresAt && user.refreshTokens[idx].expiresAt <= Date.now()) {
      user.refreshTokens.splice(idx, 1);
      await user.save();
      return res.status(401).json({ message: 'refreshToken 만료' });
    }
    // 기존 토큰 제거 및 lastUsedAt 갱신
    if (user.refreshTokens[idx]) user.refreshTokens[idx].lastUsedAt = Date.now();
    user.refreshTokens.splice(idx, 1);
    // 새 JWT/refreshToken 발급(RefreshToken rotation)
    const claims = {
      email: user.email,
      nickname: user.nickname,
      role: user.role,
      userId: user.id
    };
    const token = jwt.sign(claims, SECRET, { expiresIn: '30m', issuer: 'AUTH_SERVER', audience: 'GAME_SERVER', header: { kid: 'main-key' } });
    const newRefreshToken = require('crypto').randomBytes(32).toString('hex');
    const expiresAt = Date.now() + 30 * 24 * 60 * 60 * 1000;
    const userAgent = req.headers['user-agent'] || '';
    const ip = req.headers['x-forwarded-for'] || req.connection.remoteAddress || '';
    const createdAt = Date.now();
    const agent = useragent.parse(userAgent);
const deviceType = agent.device.toString() !== 'Other' ? agent.device.toString() : (agent.os.toString().toLowerCase().includes('android') || agent.os.toString().toLowerCase().includes('ios') ? 'Mobile' : 'PC');
user.refreshTokens.push({ token: newRefreshToken, expiresAt, userAgent, ip, createdAt, deviceType, lastUsedAt: createdAt });
    if (user.refreshTokens.length > 5) user.refreshTokens = user.refreshTokens.slice(-5);
    await user.save();
    res.json({ token, nickname: user.nickname, refreshToken: newRefreshToken, expiresAt });
  });

  // 로그아웃(RefreshToken 폐기, 다중 기기)
  router.post('/logout', async (req, res) => {
    const { refreshToken } = req.body;
    if (!refreshToken) return res.status(400).json({ message: 'refreshToken 필요' });
    const user = await User.findOne({ where: {} });
    if (user && Array.isArray(user.refreshTokens)) {
      user.refreshTokens = user.refreshTokens.filter(rt => rt.token !== refreshToken && (!rt.expiresAt || rt.expiresAt > Date.now()));
      await user.save();
    }
    res.json({ message: '로그아웃 완료' });
  });

  // 특정 기기/브라우저 강제 로그아웃(관리자/사용자)
  router.post('/logout-by-token', async (req, res) => {
    const { email, refreshToken } = req.body;
    if (!email || !refreshToken) return res.status(400).json({ message: '입력값 부족' });
    const user = await User.findOne({ where: { email } });
    if (!user || !Array.isArray(user.refreshTokens)) return res.status(404).json({ message: '사용자 없음' });
    const before = user.refreshTokens.length;
    user.refreshTokens = user.refreshTokens.filter(rt => rt.token !== refreshToken);
    await user.save();
    if (user.refreshTokens.length < before)
      return res.json({ message: '강제 로그아웃 처리 완료' });
    else
      return res.status(404).json({ message: '해당 토큰 없음' });
  });

  // 세션 목록 조회: 본인/관리자만 허용(JWT 필요)
  const useragent = require('useragent');
  router.get('/sessions', async (req, res) => {
    const { email } = req.query;
    const auth = req.headers['authorization'];
    if (!auth || !auth.startsWith('Bearer ')) return res.status(401).json({ message: '인증 필요' });
    let payload;
    try {
      payload = jwt.verify(auth.replace('Bearer ', ''), SECRET);
    } catch (e) {
      return res.status(401).json({ message: 'JWT 만료/오류' });
    }
    // 본인 또는 관리자만 가능
    if (payload.email !== email && payload.role !== 'admin') {
      return res.status(403).json({ message: '권한 없음' });
    }
    const user = await User.findOne({ where: { email } });
    if (!user || !Array.isArray(user.refreshTokens)) return res.status(404).json({ message: '사용자 없음' });
    // 만료된 세션 자동 제외
    user.refreshTokens = user.refreshTokens.filter(rt => !rt.expiresAt || rt.expiresAt > Date.now());
    await user.save();
    // 민감정보(token)는 일부만 마스킹해 반환
    const sessionList = user.refreshTokens.map(rt => ({
      token: rt.token.slice(0, 6) + '...' + rt.token.slice(-4),
      expiresAt: rt.expiresAt,
      userAgent: rt.userAgent,
      deviceType: rt.deviceType,
      ip: rt.ip,
      createdAt: rt.createdAt,
      lastUsedAt: rt.lastUsedAt
    }));
    res.json({ sessions: sessionList });
  });

    // 비밀번호 변경(모든 refreshToken 폐기)
  router.post('/change-password', async (req, res) => {
    const { email, oldPassword, newPassword } = req.body;
    if (!email || !oldPassword || !newPassword) return res.status(400).json({ message: '입력값 부족' });
    const user = await User.findOne({ where: { email } });
    if (!user) return res.status(404).json({ message: '사용자 없음' });
    const isMatch = await bcrypt.compare(oldPassword, user.password);
    if (!isMatch) return res.status(401).json({ message: '기존 비밀번호 불일치' });
    if (!isPasswordValid(newPassword)) return res.status(400).json({ message: '비밀번호 규칙 불일치' });
    user.password = await bcrypt.hash(newPassword, 10);
    user.refreshTokens = [];
    await user.save();
    res.json({ message: '비밀번호 변경 완료(재로그인 필요)' });
  });

  // 회원탈퇴(모든 refreshToken 폐기)
  router.post('/delete-account', async (req, res) => {
    const { email, password } = req.body;
    if (!email || !password) return res.status(400).json({ message: '입력값 부족' });
    const user = await User.findOne({ where: { email } });
    if (!user) return res.status(404).json({ message: '사용자 없음' });
    const isMatch = await bcrypt.compare(password, user.password);
    if (!isMatch) return res.status(401).json({ message: '비밀번호 불일치' });
    await user.destroy();
    res.json({ message: '회원탈퇴 완료' });
  });

  // 닉네임 등 중요 정보 변경(닉네임만 예시, 모든 refreshToken 폐기)
  router.post('/change-nickname', async (req, res) => {
    const { email, password, nickname } = req.body;
    if (!email || !password || !nickname) return res.status(400).json({ message: '입력값 부족' });
    const user = await User.findOne({ where: { email } });
    if (!user) return res.status(404).json({ message: '사용자 없음' });
    const isMatch = await bcrypt.compare(password, user.password);
    if (!isMatch) return res.status(401).json({ message: '비밀번호 불일치' });
    if (!isNicknameValid(nickname)) return res.status(400).json({ message: '닉네임 규칙 불일치' });
    user.nickname = nickname;
    user.refreshTokens = [];
    await user.save();
    res.json({ message: '닉네임 변경 완료(재로그인 필요)' });
  });

  // === 라우터 끝 ===
  return router;
};
