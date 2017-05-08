const crypto = require('crypto')

function md5 (str) {
  return crypto.createHash('md5').update(str).digest('hex')
}

module.exports = (User, Online) => {
  let api = require('express').Router()

  // 登录
  api.post('/signin', (req, res) => {
    if (req.body.username && req.body.password) {
      User.findOne({ username: req.body.username })
      .then(user => {
        if (user && md5(req.body.password) === user.password) {
          Online.findById(user._id).then(online => {
            if (!online) {
              new Online({ userId: user._id }).save().then(online => {
                res.json({
                  result: 'ok',
                  onlineId: online._id,
                  username: user.username,
                  avatar: user.avatar,
                  nickname: user.nickname,
                  userId: user._id
                })
              })
            } else {
              res.json({
                result: 'ok',
                onlineId: online._id,
                username: user.username,
                avatar: user.avatar,
                nickname: user.nickname,
                userId: user._id
              })
            }
          })
        } else {
          res.json({ result: 'error' })
        }
      })
    } else {
      res.json({ result: 'error' })
    }
  })

  // 注册
  api.post('/signup', (req, res) => {
    if (req.body.username && req.body.password &&
        req.body.nickname && req.body.nickname.length > 0 &&
        req.body.username.length > 7 && req.body.password.length > 7 &&
        req.body.username.length < 16 && req.body.password.length < 16) {
      User.findOne({ username: req.body.username }).then(user => {
        if (user) {
          res.json({ result: 'error' })
        } else {
          new User({
            username: req.body.username,
            password: md5(req.body.password),
            nickname: req.body.nickname,
            avatar: '/static/default-avatar.jpg'
          }).save().then(user => {
            new Online({ userId: user._id }).save().then(online => {
              res.json({
                result: 'ok',
                onlineId: online._id,
                username: user.username,
                avatar: user.avatar,
                nickname: user.nickname,
                userId: user._id
              })
            })
          })
        }
      })
    } else {
      res.json({ result: 'error' })
    }
  })

  // 退出
  api.get('/signout', (req, res) => {
    if (req.query.onlineId) {
      Online.findByIdAndRemove(req.query.onlineId).then(online => {
        res.json({ result: 'ok' })
      })
    } else {
      res.json({ result: 'error' })
    }
  })

  // 改昵称
  api.get('/nickname', (req, res) => {
    Online.findById(req.query.onlineId).then(online => {
      if (!online || req.query.nickname.length < 1) {
        res.json({ result: 'error' })
      } else {
        User.findByIdAndUpdate(online.userId, { $set: { nickname: req.query.nickname } })
        .then(() => res.json({ result: 'ok' }))
      }
    }).catch(() => res.json({ result: 'error' }))
  })

  return api
}
