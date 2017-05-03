const Record = require('../model/record')
const Online = require('../model/online')
const Favorite = require('../model/favorite')
const ObjectId = require('mongoose').Schema.ObjectId
let api = require('express').Router()

// 发表记录
api.post('/post', (req, res) => {
  Online.findOne({ _id: new ObjectId(req.body.onlineId) }).then(online => {
    if (!online || !req.body.title || !req.body.content) {
      res.json({ result: 'error' })
    } else {
      new Record({
        title: req.body.title,
        content: req.body.content,
        image: req.body.image,
        audio: req.body.audio,
        video: req.body.video,
        userId: online.userId,
        date: new Date().getTime()
      }).save().then(record => {
        res.json({ result: 'ok', record })
      })
    }
  })
})

// 删除记录
api.get('/delete', (req, res) => {
  Online.findOne({ _id: new ObjectId(req.params.onlineId) }).then(online => {
    if (!online) {
      res.json({ result: 'error' })
    } else {
      Record.findOne({ _id: new ObjectId(req.params.recordId) }).then(record => {
        if (record.userId != online.userId) {
          res.json({ result: 'error' })
        } else {
          Record.remove({ _id: new ObjectId(req.params.recordId) }).then(() => {
            res.json({ result: 'ok' })
          })
        }
      })
    }
  })
})

// 查看所有记录, TODO: 查询用户头像
api.get('/all', (req, res) => {
  Record.find({}).then(records => {
    let all = []
    for (let i = 0; i < records.length; ++i) {
      all.push({
        title: records[i].title,
        content: records[i].content,
        date: records[i].date,
        favoriteNum: records[i].favoriteNum
      })
    }
    res.json({ result: 'ok', all })
  })
})

// 查看某条记录, TODO: 查询点赞数据的逻辑
api.get('/detail', (req, res) => {
  Record.findOne({ _id: new ObjectId(req.params.ObjectId) }).then(record => {
    res.json({ result: 'ok', record })
  })
})

// TODO: 点赞
api.get('/favorite', (req, res) => {
  res.json({})
})

module.exports = api