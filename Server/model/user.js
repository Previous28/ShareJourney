const mongoose = require('mongoose')
const Schema = mongoose.Schema

mongoose.Promise = global.Promise
mongoose.createConnection('mongodb://localhost:27017/')

const UserSchema = new Schema({
  avatar: String,
  username: String,
  password: String,
  nickname: String
})

module.exports = mongoose.model('user', UserSchema)
