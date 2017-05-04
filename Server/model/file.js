const fs = require('fs')
let FileOP = {}

FileOP.delete = (path) => {
  return new Promise((resolve, reject) => {
    fs.unlink(path, (err) => {
      !err ? resolve() : reject(err)
    })
  })
}

FileOP.rename = (oldPath, newPath) => {
  return new Promise((resolve, reject) => {
    fs.rename(oldPath, newPath, (err) => {
      !err ? resolve() : reject(err)
    })
  })
}

module.exports = FileOP
