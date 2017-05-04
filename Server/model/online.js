module.exports = (mongoose) => {
  const Schema = mongoose.Schema
  const ObjectId = Schema.ObjectId

  const OnlineSchema = new Schema({
    userId: ObjectId
  })

  return mongoose.model('online', OnlineSchema)
}
