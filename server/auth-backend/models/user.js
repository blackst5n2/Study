const { DataTypes } = require('sequelize');

module.exports = (sequelize) => {
  const User = sequelize.define('User', {
    // createdAt, updatedAt은 Sequelize가 자동 관리(옵션 미지정 시 true)

    id: {
      type: DataTypes.INTEGER,
      autoIncrement: true,
      primaryKey: true
    },
    email: {
      type: DataTypes.STRING,
      allowNull: false,
      unique: true
    },
    password: {
      type: DataTypes.STRING,
      allowNull: false
    },
    nickname: {
      type: DataTypes.STRING,
      allowNull: true
    },
    provider: {
      type: DataTypes.STRING,
      allowNull: true
    },
    oauthId: {
      type: DataTypes.STRING,
      allowNull: true
    },
    role: {
      type: DataTypes.STRING,
      allowNull: false,
      defaultValue: 'user',
    },
    isVerified: {
      type: DataTypes.BOOLEAN,
      allowNull: false,
      defaultValue: false
    },
    verifyToken: {
      type: DataTypes.STRING,
      allowNull: true
    },
    refreshTokens: {
      type: DataTypes.JSONB,
      allowNull: true,
      defaultValue: [] // [{token, expiresAt, userAgent, ip, createdAt, deviceType, lastUsedAt}]
    }
  });
  return User;
};
