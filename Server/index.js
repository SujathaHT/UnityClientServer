var uuid = require('uuid-random')
const WebSocket = require('ws')
var fs = require('fs')
const { config } = require('process')

const wss = new WebSocket.WebSocketServer({ port: 8080 }, () => {
    console.log('ggj-2022-mpg-server started')
})

// Json object
var playersData = {
    "type":"playerData"
}

var serverConfigData = {}

fs.readFile("config.json", function (err, buf) {
    serverConfigData = JSON.parse(buf.toString())
    console.log(serverConfigData)
})

// Websocket functions
// OnConnect
wss.on('connection', function connection(client) {
    client.id = uuid();
    console.log('-connected: ' + client.id)
    var currentClient = playersData["" + client.id]

    // OnReceive
    client.on('message', (data) => {
        try {
            console.log("from: " + client.id + " - " + data)
            var dataJson = JSON.parse(data)
            if (dataJson.message_type == "login") {
                var authenticated = false;
                for (var index in serverConfigData) {
                    var playerConfig = serverConfigData[index]
                    if (dataJson.username === playerConfig.username && dataJson.password === playerConfig.password) {
                        authenticated = true;
                    }
                }

                if (authenticated) {
                    client.send('{"id":"' + client.id + '", "message_type":"login_result", "login_success":"true"}')
                    console.log("authentication success for: " + dataJson.username)
                }
                else {
                    client.send('{"id":"' + client.id + '", "message_type":"login_result", "login_success":"false"}')
                    console.log("authentication failed for: " + dataJson.username)
                }
            }
        }
        catch (e) {
            console.log(e)
        }
    })

    // OnClose
    client.on('close', () => {
        console.log('-closed: ' + client.id)
    })
})

// Start listening
wss.on('listening', () => {
    console.log('listening on 8080')
})