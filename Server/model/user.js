module.exports = (mongoose) => {
  const Schema = mongoose.Schema

  const UserSchema = new Schema({
    avatar: String,
    username: String,
    password: String,
    nickname: String
  })

  return mongoose.model('user', UserSchema)
}
