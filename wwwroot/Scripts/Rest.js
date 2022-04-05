async function POST(url = "", data = {}) {
    const response = await fetch(
        url,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        }
    )
    if (response.ok) {
        return JSON.parse(response.json);
    } else {
        console.log(`Http-Error: ${response.status}`);
    }
}

async function GET(url = "") {
    const response = await fetch(
        url,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        }
    )
    if (response.ok) {
        return JSON.parse(response.json);
    } else {
        console.log(`Http-Error: ${response.status}`);
    }
}

async function PUT(url = "", data = {}) {
    const response = await fetch(
        url,
        {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        }
    )
    if (response.ok) {
        return JSON.parse(response.json);
    } else {
        console.log(`Http-Error: ${response.status}`);
    }
}

async function DELETE(url = "", data = {}) {
    const response = await fetch(
        url,
        {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        }
    )
    if (response.ok) {
        return JSON.parse(response.json);
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