module.exports = (Online, FileOP) => {
  const multer = require('multer')
  const path = require('path')
  const image = multer({ dest: path.join(__dirname, '../static/image') })
  const audio = multer({ dest: path.join(__dirname, '../static/audio') })
  const video = multer({ dest: path.join(__dirname, '../static/video') })

  let api = require('express').Router()

  // 上传图片
  api.post('/image', image.single('image'), (req, res) => {
    req.file ? upload(req, res) : res.json({ result: 'error' })
  })

  // 上传音频
  api.post('/audio', audio.single('audio'), (req, res) => {
    req.file ? upload(req, res) : res.json({ result: 'error' })
  })

  // 上传视频
  api.post('/video', video.single('video'), (req, res) => {
    req.file ? upload(req, res) : res.json({ result: 'error' })
  })

  function upload(req, res) {
    Online.findById(req.params.ObjectId).then(online => {
      if (!online) {
        FileOP.delete(path.join(__dirname, '../' + req.file.path))
        res.json({ result: 'error' })
      } else {
        let oldPath = path.join(__dirname, '../' + req.file.path)
        let newPath = path.join(__dirname, '../' + req.file.path + req.file.originalname)
        FileOP.rename(oldPath, newPath).then(() => {
          res.json({
            result: 'ok',
            url: req.file.path + req.file.originalname
          })
        }).catch((err) => {
          console.log('File upload error.', err)
          res.json({ result: 'error' })
        })
      }
    })
  }

  return api
}
