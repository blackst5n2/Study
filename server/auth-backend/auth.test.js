// auth-backend/auth.test.js
const request = require('supertest');
const app = require('./app'); // app.js에서 Express app export 필요
const sequelize = require('./db');
const UserModel = require('./models/user');
const User = UserModel(sequelize);
const bcrypt = require('bcrypt');

describe('Auth API Integration', () => {
  const testUser = {
    email: 'testuser@example.com',
    password: 'Test1234!',
    nickname: '테스트유저',
    isVerified: true,
    role: 'user'
  };

  beforeAll(async () => {
    // 테스트 계정 생성 (비밀번호 해시)
    const hash = await bcrypt.hash(testUser.password, 12);
    await User.destroy({ where: { email: testUser.email } });
    await User.create({ ...testUser, password: hash });
  });

  afterAll(async () => {
    await User.destroy({ where: { email: testUser.email } });
  });

  it('로그인 성공 및 JWT 토큰 반환', async () => {
    const res = await request(app)
      .post('/api/auth/login')
      .send({ email: testUser.email, password: testUser.password });
    expect(res.statusCode).toBe(200);
    expect(res.body.token).toBeDefined();
    expect(res.body.nickname).toBe(testUser.nickname);
  });

  it('이메일 인증 안 된 경우 로그인 거부', async () => {
    // 미인증 계정 추가
    await User.create({ ...testUser, email: 'unverified@example.com', isVerified: false });
    const res = await request(app)
      .post('/api/auth/login')
      .send({ email: 'unverified@example.com', password: testUser.password });
    expect(res.statusCode).toBe(403);
    expect(res.body.message).toContain('이메일 인증');
    await User.destroy({ where: { email: 'unverified@example.com' } });
  });

  it('비밀번호 불일치 시 로그인 거부', async () => {
    const res = await request(app)
      .post('/api/auth/login')
      .send({ email: testUser.email, password: 'WrongPassword!' });
    expect(res.statusCode).toBe(401);
    expect(res.body.message).toContain('로그인 실패');
  });

  it('존재하지 않는 이메일 로그인 거부', async () => {
    const res = await request(app)
      .post('/api/auth/login')
      .send({ email: 'noexist@example.com', password: 'Test1234!' });
    expect(res.statusCode).toBe(401);
    expect(res.body.message).toContain('로그인 실패');
  });
});
