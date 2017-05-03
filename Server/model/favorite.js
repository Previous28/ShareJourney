const mongoose = require('mongoose')
const Schema = mongoose.Schema
const ObjectId = Schema.ObjectId

mongoose.connect('mongodb://localhost:27017/favorite')

const FavoriteSchema = new Schema({
  userId: ObjectId,
  recordId: ObjectId
})

module.exports = mongoose.model('favorite', FavoriteSchema)
