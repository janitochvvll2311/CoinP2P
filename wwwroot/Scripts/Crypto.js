function getElement(id = "") {
    return document.getElementById(id);
}

let isite = getElement("isite");
let iprivate = getElement("iprivate");
let ipublic = getElement("ipublic");

let imessage = getElement("imessage");
let iencrypted = getElement("iencrypted");
let idecrypted = getElement("idecrypted");

let isignature = getElement("isignature");
let ivalid = getElement("ivalid");

async function generate() {
    const keys = await REST.GET(`https://${isite.value}/Generate`);
    iprivate.value = keys.private;
    ipublic.value = keys.public;
}

async function encrypt() {
    const encrypted = await REST.POST(`https://${isite.value}/Encrypt`, { message: imessage.value, key: ipublic.value });
    iencrypted.value = encrypted.data;
}

async function decrypt() {
    const decrypted = await REST.POST(`https://${isite.value}/Decrypt`, { data: iencrypted.value, key: iprivate.value });
    idecrypted.value = decrypted.message;
}

async function sign() {
    const signature = await REST.POST(`https://${isite.value}/Sign`, { message: imessage.value, key: iprivate.value });
    isignature.value = signature.value;
}

async function verify() {
    const valid = await REST.POST(`https://${isite.value}/Verify`, { message: imessage.value, signature: isignature.value, key: ipublic.value });
    ivalid.value = valid.value;
}