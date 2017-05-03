const mongoose = require('mongoose')
const Schema = mongoose.Schema

mongoose.connect('mongodb://localhost:27017/user')

const UserSchema = new Schema({
  avatar: String,
  username: String,
  password: String,
  nickname: String
})

module.exports = mongoose.model('user', UserSchema)
