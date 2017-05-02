const express = require('express')
const bodyParser = require('body-parser')
const path = require('path')
const http = require('http')
const port = '8080'

let app = express()

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: false }))
app.use('/static', express.static(path.join(__dirname, './static')))

app.set('port', port)

http.createServer(app).listen(port)
console.log("http://localhost:" + port)