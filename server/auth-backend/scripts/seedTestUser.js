// auth-backend/scripts/seedTestUser.js
const sequelize = require('../db');
const UserModel = require('../models/user');
const bcrypt = require('bcrypt');

(async () => {
  const User = UserModel(sequelize);
  const testUser = {
    email: 'testuser@example.com',
    password: 'Test1234!',
    nickname: '테스트유저',
    isVerified: true,
    role: 'user'
  };
  try {
    const hash = await bcrypt.hash(testUser.password, 12);
    await User.destroy({ where: { email: testUser.email } });
    await User.create({ ...testUser, password: hash });
    console.log('테스트 계정이 성공적으로 생성되었습니다.');
  } catch (err) {
    console.error('테스트 계정 생성 실패:', err);
  } finally {
    await sequelize.close();
  }
})();
