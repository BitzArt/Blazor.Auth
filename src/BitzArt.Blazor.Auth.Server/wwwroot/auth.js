
export async function requestAsync(url, method, body, outputType = "json") {

    const request = new Request(url, {
        method: method,
        body: body ? JSON.stringify(body) : null
    });

    if (body) {
        request.headers.set('Content-Type', 'application/json');
    }

    const response = await fetch(request);

    if (!response.ok) {
        throw new Error(`Server responded with non-success status code: '${response.status}'`);
    }

    switch (outputType) {
        case "text":
            return await response.text();
        case "json":
            return await response.json();
        case "blob":
            return await response.blob();
        case "arrayBuffer":
            return await response.arrayBuffer();
        case "formData":
            return await response.formData();
        default:
            return null;
    }
}
