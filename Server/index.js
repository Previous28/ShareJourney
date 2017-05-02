const express = require('express')
const bodyParser = require('body-parser')
const path = require('path')
const http = require('http')
const auth = require('./api/auth')
const record = require('./api/record')
const upload = require('./api/upload')
const port = 8080

let app = express()

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: false }))
app.use('/static', express.static(path.join(__dirname, './static')))
app.use('/api/auth', auth)
app.use('/api/record', record)
app.use('/api/upload', upload)
app.listen(port)

console.log("http://localhost:" + port)