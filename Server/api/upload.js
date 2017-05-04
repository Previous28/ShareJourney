module.exports = (Online, FileOP, User, Record) => {
  const multer = require('multer')
  const path = require('path')
  const image = multer({ dest: path.join(__dirname, '../static/image') })
  const audio = multer({ dest: path.join(__dirname, '../static/audio') })
  const video = multer({ dest: path.join(__dirname, '../static/video') })
  const avatar = multer({ dest: path.join(__dirname, '../static/avatar') })

  let api = require('express').Router()

  // 上传图片
  api.post('/image', image.single('image'), (req, res) => {
    req.file ? upload(req, res, 'image') : res.json({ result: 'error' })
  })

  // 上传音频
  api.post('/audio', audio.single('audio'), (req, res) => {
    req.file ? upload(req, res, 'audio') : res.json({ result: 'error' })
  })

  // 上传视频
  api.post('/video', video.single('video'), (req, res) => {
    req.file ? upload(req, res, 'video') : res.json({ result: 'error' })
  })

  // 上传头像
  api.post('/avatar', avatar.single('avatar'), (req, res) => {
    req.file ? upload(req, res, 'avatar') : res.json({ result: 'error' })
  })

  function upload(req, res, type) {
    Online.findById(req.query.onlineId).then(online => {
      if (!online) {
        FileOP.delete(path.join(__dirname, '../static/' + type, req.file.filename))
        res.json({ result: 'error' })
      } else {
        let oldPath = path.join(__dirname, '../static/' + type, req.file.filename)
        let newName = '/static/' + type + '/' + online.userId + '_' + Date.now() + '_' + req.file.originalname
        let newPath = path.join(__dirname, '../' + newName)
        FileOP.rename(oldPath, newPath).then(() => {
          if (type === 'avatar') {// 上传头像
            User.findByIdAndUpdate(online.userId, { $set: { avatar: newName } })
            .then(() => res.json({ result: 'ok', path: newName }))
          } else {// 上传其他
            let update = type === 'image' ? { $set: { image: newName } } :
              type === 'audio' ? { $set: { audio: newName } } : { $set: { video: newName } }
            Record.findByIdAndUpdate(req.query.recordId, update)
            .then(() => res.json({ result: 'ok', path: newName }))
          }
        }).catch((err) => {
          FileOP.delete(oldPath)
          console.log('File upload error.', err)
          res.json({ result: 'error' })
        })
      }
    })
  }

  return api
}
