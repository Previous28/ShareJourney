module.exports = (mongoose) => {
  const Schema = mongoose.Schema
  const ObjectId = Schema.ObjectId

  const RecordSchema = new Schema({
    date: String,
    title: String,
    image: String,
    audio: String,
    video: String,
    content: String,
    favoriteNum: Number,
    userId: ObjectId
  })
  
  return mongoose.model('record', RecordSchema)
}
