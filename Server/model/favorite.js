const mongoose = require('mongoose')
const Schema = mongoose.Schema
const ObjectId = Schema.ObjectId

mongoose.Promise = global.Promise
mongoose.createConnection('mongodb://localhost:27017/')

const FavoriteSchema = new Schema({
  userId: ObjectId,
  recordId: ObjectId
})

module.exports = mongoose.model('favorite', FavoriteSchema)
