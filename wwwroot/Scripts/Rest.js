async function POST(url = "", data = {}) {
    var json = JSON.stringify(data);
    const response = await fetch(
        url,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: json
        }
    )
    if (response.ok) {
        var json = await response.json();
        return json;
    } else {
        console.log(`Http-Error: ${response.status}`);
    }
}

async function GET(url = "") {
    const response = await fetch(
        url,
        {
            method: "GET"
        }
    )
    if (response.ok) {
        var json = await response.json();
        return json;
    } else {
        console.log(`Http-Error: ${response.status}`);
    }
}

async function PUT(url = "", data = {}) {
    var json = JSON.stringify(data);
    const response = await fetch(
        url,
        {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: json
        }
    )
    if (response.ok) {
        var json = await response.json();
        return json;
    } else {
        console.log(`Http-Error: ${response.status}`);
    }
}

async function DELETE(url = "", data = {}) {
    var json = JSON.stringify(data);
    const response = await fetch(
        url,
        {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json"
            },
            body: json
        }
    )
    if (response.ok) {
        var json = await response.json();
        return json;
    } else {
        console.log(`Http-Error: ${response.status}`);
    }
}

const REST = {
    POST: POST,
    GET: GET,
    PUT: PUT,
    DELETE: DELETE
};