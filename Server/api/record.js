let api = require('express').Router()

// 发表记录
api.post('/post', (req, res) => {
  res.json({})
})

// 删除记录
api.get('/delete', (req, res) => {
  res.json({})
})

// 查看所有记录
api.get('/all', (req, res) => {
  res.json({})
})

// 查看某条记录
api.get('/detail', (req, res) => {
  res.json({})
})

module.exports = api