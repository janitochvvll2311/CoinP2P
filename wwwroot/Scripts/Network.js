function getElement(id = "") {
    return document.getElementById(id);
}

function random() {
    const number = Math.floor(Math.random() * 2147483647);
    return number;
}

let isite = getElement("isite");
let iremote = getElement("iremote");
let inbox = getElement("inbox");
let inickname = getElement("inickname");
let imessage = getElement("imessage");

let nonces = [];
let sockets = [];
let parser = new DOMParser();

function clean() {
    sockets = sockets.filter((v, i, a) => {
        return v.readyState == WebSocket.CONNECTING || v.readyState == WebSocket.OPEN;
    });
    con_count.value = sockets.length;
}

function print(message) {
    const doc = parser.parseFromString(message, "text/html");
    inbox.appendChild(doc.body);
}

function connectMe() {
    var socket = new WebSocket(`wss://${iremote.value}/ConnectMe`);
    socket.onopen = (e) => {
        alert(`Connection OPEN: ${socket.url}`);
        sockets.push(socket);
    };
    socket.onclose = (e) => {
        alert(`Connection ClOSE: ${socket.url}`);
        clean();
    };
    socket.onmessage = (e) => {
        const message = JSON.parse(e.data);
        if (message && nonces.indexOf(message.Nonce) < 0) {
            nonces.push(message.Nonce);
            print(message.Message);
        }
    };
}

function connectTo() {
    REST.POST(`https://${isite.value}/ConnectTo`, iremote.value);
}

function send() {
    if (sockets.length > 0) {
        const message = `${inickname.value}> ${imessage.value}`;
        print(message)
        var nonce = random();
        while (nonces.indexOf(nonce) >= 0)
            nonce = random();
        const data = JSON.stringify({
            Nonce: nonce,
            Message: message
        });
        sockets.forEach((v, i, a) => {
            if (v.readyState == WebSocket.OPEN)
                v.send(data);
        });
    }
}