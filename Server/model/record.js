const mongoose = require('mongoose')
const Schema = mongoose.Schema
const ObjectId = Schema.ObjectId

mongoose.Promise = global.Promise
mongoose.createConnection('mongodb://localhost:27017/')

const RecordSchema = new Schema({
  date: Date,
  title: String,
  image: String,
  audio: String,
  video: String,
  content: String,
  favoriteNum: Number,
  userId: ObjectId
})

module.exports = mongoose.model('record', RecordSchema)
