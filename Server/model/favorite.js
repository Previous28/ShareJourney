module.exports = (mongoose) => {
  const Schema = mongoose.Schema
  const ObjectId = Schema.ObjectId

  const FavoriteSchema = new Schema({
    userId: ObjectId,
    recordId: ObjectId
  })
  
  return mongoose.model('favorite', FavoriteSchema)
}
