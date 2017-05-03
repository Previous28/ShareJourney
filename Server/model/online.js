const mongoose = require('mongoose')
const Schema = mongoose.Schema
const ObjectId = Schema.ObjectId

mongoose.connect('mongodb://localhost:27017/online')

const OnlineSchema = new Schema({
  userId: ObjectId
})

module.exports = mongoose.model('online', OnlineSchema)
