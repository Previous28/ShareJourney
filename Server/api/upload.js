const multer = require('multer')
const image = multer({ dest: '../static/image' })
const audio = multer({ dest: '../static/audio' })
const video = multer({ dest: '../static/video' })
const Online = require('../model/online')
const File = require('../model/file')
const path = require('path')

let api = require('express').Router()

function upload(req, res) {
  Online.findById(req.params.ObjectId).then(online => {
    if (!online) {
      File.delete(path.join(__dirname, '../' + req.file.path))
      res.json({ result: 'error' })
    } else {
      let oldPath = path.join(__dirname, '../' + req.file.path)
      let newPath = path.join(__dirname, '../' + req.file.path + req.file.originalname)
      File.rename(oldPath, newPath).then(() => {
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

module.exports = api
