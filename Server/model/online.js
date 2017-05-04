const mongoose = require('mongoose')
const Schema = mongoose.Schema
const ObjectId = Schema.ObjectId

mongoose.Promise = global.Promise
mongoose.createConnection('mongodb://localhost:27017/')

const OnlineSchema = new Schema({
  userId: ObjectId
})

module.exports = mongoose.model('online', OnlineSchema)
