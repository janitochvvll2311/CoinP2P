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

let iprivate = getElement("iprivate");
let ipublic = getElement("ipublic");

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

async function connectMe() {
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

async function connectTo() {
    var response = await REST.POST(`https://${isite.value}/ConnectTo`, iremote.value);
    alert(response.message);
}

async function generate() {
    const keys = await REST.GET(`https://${isite.value}/Generate`)
    iprivate.value = keys.private;
    ipublic.value = keys.public;
}

async function sign(message = "") {
    const signature = await REST.POST(`https://${isite.value}/Sign`, { remote: ipublic.value, message: message, key: iprivate.value });
    return signature.value;
}

async function send() {
    if (sockets.length > 0) {
        const message = `${inickname.value}> ${imessage.value}`;
        const signature = await sign(message);
        print(message)
        var nonce = random();
        while (nonces.indexOf(nonce) >= 0)
            nonce = random();
        nonces.push(nonce);
        const data = JSON.stringify({
            Nonce: nonce,
            Remote: ipublic.value,
            Message: message,
            Signature: signature
        });
        sockets.forEach((v, i, a) => {
            if (v.readyState == WebSocket.OPEN)
                v.send(data);
        });
    }
}

window.onunload = (e) => {
    sockets.forEach((v, i, a) => {
        v.close(1005, "");
    });
}