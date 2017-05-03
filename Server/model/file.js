const fs = require('fs')
let File = {}

File.delete = (path) => {
  return new Promise((resolve, reject) => {
    fs.unlink(path, (err) => {
      !err ? resolve() : reject(err)
    })
  })
}

File.rename = (oldPath, newPath) => {
  return new Promise((resolve, reject) => {
    fs.rename(oldPath, newPath, (err) => {
      !err ? resolve() : reject(err)
    })
  })
}

module.exports = File
