let api = require('express').Router()

// 登录
api.post('/signin', (req, res) => {
  res.json({})
})

// 注册
api.post('/signup', (req, res) => {
  res.json({})
})

// 退出
api.get('/signout', (req, res) => {
  res.json({})
})

module.exports = api