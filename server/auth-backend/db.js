const { Sequelize } = require('sequelize');

const sequelize = new Sequelize(
  process.env.PG_DATABASE || 'test',
  process.env.PG_USER || 'test',
  process.env.PG_PASSWORD || 'test1234',
  {
    host: process.env.PG_HOST || 'localhost',
    port: process.env.PG_PORT || 5432,
    dialect: 'postgres',
    logging: false
  }
);

module.exports = sequelize;
