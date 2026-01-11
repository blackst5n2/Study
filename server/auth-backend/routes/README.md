# uth-backend

## 제공 라우트

- `POST /api/auth/login` : 로그인 (email, password)
- `POST /api/auth/signup` : 회원가입 (email, password, nickname)

- JWT 토큰은 `SUPER_SECRET_KEY`로 서명, 1시간 유효
- 사용자 정보는 메모리(users 배열)에 저장 (실서비스 시 DB 연동 필요)

---

## 시작 방법

```
npm install
node app.js
```

API 서버는 기본적으로 `http://localhost:4000`에서 실행됩니다.
프론트엔드(React)는 `http://localhost:3000`에서 개발하도록 CORS 허용됨.
