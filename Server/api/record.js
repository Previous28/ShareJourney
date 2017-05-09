module.exports = (Record, Online, Favorite, User) => {
  const ObjectId = require('mongoose').Schema.ObjectId
  let api = require('express').Router()

  // 发表记录
  api.post('/post', (req, res) => {
    Online.findById(req.body.onlineId).then(online => {
      if (!online || !req.body.title || !req.body.content) {
        res.json({ result: 'error' })
      } else {
        const now = new Date(Date.now())
        let date = '' + now.getFullYear()
        date += now.getMonth() < 9 ? '-0' + (now.getMonth() + 1) : '-' + (now.getMonth())
        date += now.getDate() < 10 ? '-0' + now.getDate() : '-' + now.getDate()
        date += now.getHours() < 10 ? ' 0' + now.getHours() : ' ' + now.getHours()
        date += ':' + now.getMinutes()
        new Record({
          title: req.body.title,
          content: req.body.content,
          userId: online.userId,
          favoriteNum: 0,
          image: '',
          audio: '',
          video: '',
          date: date
        }).save().then(record => res.json({ result: 'ok', record }))
      }
    }).catch(() => res.json({ result: 'error' }))
  })

  // 删除记录
  api.get('/delete', (req, res) => {
    Online.findById(req.query.onlineId).then(online => {
      if (!online) {
        res.json({ result: 'error' })
      } else {
        Record.findById(req.query.recordId).then(record => {
          if (record.userId.toString() !== online.userId.toString()) {
            res.json({ result: 'error' })
          } else {
            Record.findByIdAndRemove(req.query.recordId).then(() => {
              res.json({ result: 'ok' })
            })
          }
        }).catch(() => res.json({ result: 'error' }))
      }
    }).catch(() => res.json({ result: 'error' }))
  })

  // 查看所有记录
  api.get('/all', (req, res) => {
    Record.find({}).then(records => {
      let count = records.length;
      count ? '' : res.json({ result: 'ok', records })
      for (let i = 0; i < records.length; ++i) {
        ((i) => {
          User.findById(records[i].userId).then(user => {
            user ? records[i]._doc.userAvatar = user.avatar : ''
            user ? records[i]._doc.nickname = user.nickname : ''
            count > 1 ? --count : res.json({ result: 'ok', records })
          })
        })(i)
      }
    }).catch(() => res.json({ result: 'error' }))
  })

  // 查看某条记录
  api.get('/detail', (req, res) => {
    Record.findById(req.query.recordId).then(record => {
      record._doc.favoriter = []
      Favorite.find({ recordId: req.query.recordId }).then(favorites => {
        let count = favorites.length;
        count ? '' : res.json({ result: 'ok', record })
        for (let i = 0; i < favorites.length; ++i) {
          User.findById(favorites[i].userId).then(user => {
            user ? record._doc.favoriter.push(user.avatar) : ''
            count > 1 ? --count : res.json({ result: 'ok', record })
          })
        }
      }).catch(() => res.json({ result: 'error' }))
    }).catch(() => res.json({ result: 'error' }))
  })

  // 点赞
  api.get('/favorite', (req, res) => {
    Online.findById(req.query.onlineId).then(online => {
      if (!online) {
        res.json({ result: 'error' })
      } else {
        Favorite.findOne({ userId: online.userId, recordId: req.query.recordId }).then(favorite => {
          if (!favorite) {
            Record.findByIdAndUpdate(req.query.recordId, { $inc: { favoriteNum: 1 } }).then(() => {
              new Favorite({ userId: online.userId, recordId: req.query.recordId })
              .save().then(() => res.json({ result: 'ok' }))
            }).catch(() => res.json({ result: 'error' }))
          } else {
            res.json({ result: 'ok' })
          }
        })
      }
    }).catch(() => res.json({ result: 'error' }))
  })

  // 查找某个用户的所有记录
  api.get('/records-of-user', (req, res) => {
    if (!req.query.userId) {
      res.json({ result: 'error' })
    } else {
      Record.find({ userId: req.query.userId }).then(records => {
        res.json({ result: 'ok', records })
      }).catch(() => res.json({ result: 'error' }))
    }
  })

  return api
}
