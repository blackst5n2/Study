require('dotenv').config();
const express = require('express');
const helmet = require('helmet');
const cors = require('cors');
const morgan = require('morgan');
const rateLimit = require('express-rate-limit');
const app = express();

// 로그인 라우트에만 rate limit 적용 (1분에 10회)
const loginLimiter = rateLimit({
  windowMs: 60 * 1000,
  max: 10,
  message: { message: '로그인 시도 횟수가 너무 많습니다. 잠시 후 다시 시도해 주세요.' }
});
app.use('/api/auth/login', loginLimiter);

// 보안/운영에 좋은 미들웨어 자동 적용
app.use(helmet());
app.use(cors({ origin: 'http://localhost:3000', credentials: true }));
app.use(morgan('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
const sequelize = require('./db');
const UserModel = require('./models/user');
const User = UserModel(sequelize);
const authRoutes = require('./routes/auth')(User);

app.use(cors({ origin: 'http://localhost:3000', credentials: true }));
app.use(express.json());
app.use('/api/auth', authRoutes);

app.get('/', (req, res) => {
  res.send('WIW Auth Backend is running!');
});

(async () => {
  try {
    await sequelize.sync();
    app.listen(4000, () => console.log('API 서버 실행중: http://localhost:4000'));
  } catch (err) {
    console.error('DB 연결 실패:', err);
  }
})();

module.exports = app;