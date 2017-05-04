const port = 8080
const express = require('express')
const bodyParser = require('body-parser')
const path = require('path')
const http = require('http')
const mongoose = require('mongoose')

mongoose.Promise = global.Promise
mongoose.connect('mongodb://localhost:27017/')

const Favorite = require('./model/favorite')(mongoose)
const Online = require('./model/online')(mongoose)
const Record = require('./model/record')(mongoose)
const User = require('./model/user')(mongoose)
const FileOP = require('./model/file')

let app = express()

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: false }))
app.use('/static', express.static(path.join(__dirname, './static')))
app.use('/api/auth', require('./api/auth')(User, Online))
app.use('/api/record', require('./api/record')(Record, Online, Favorite, User))
app.use('/api/upload', require('./api/upload')(Online, FileOP))
app.listen(port)

console.log("http://localhost:" + port)
