
const enviroment = {
    webSocketUrl_ssl: 'wss://localhost:7075',
}


window.addEventListener('load', () => {
    const socket = new SalkySocket();
    const toggle_btn = document.querySelector("#toggle");
    const ping_btn = document.querySelector("#ping");

    ping_btn.addEventListener('click', () => socket.sendMessage('teste/ping', 'post', null))

    var removeListener1 = socket.on('open', '', (event) => ping_btn.disabled = false)

    var removeListener2 = socket.on('close', '', (event) => ping_btn.disabled = true)

    var removeListener3 = socket.on('teste/ping', 'post', (data) => console.log(`pong received : ${data}`));

    var removeListener4 = socket.on('connected', 'post', (data) => console.log(`connected received : ${data}`));


    toggle_btn.addEventListener('click', () => {
        if (socket.connectionIsOpen) {
            socket.disconnect();
            toggle_btn.innerHTML = 'Connect'
        }
        else {
            socket.connect();
            toggle_btn.innerHTML = 'Disconnect'
        }
    });
});


class SalkySocket {
    routes = new MessageRouter();

    on(path, method, handler) {
        const key = this.genKey(path, method);
        return this.routes.on(key, handler);
    }
    genKey(path, method) {
        return (path + method).toLowerCase()
    }
    connect(userId) {
        if (this.connectionIsOpen) return;
        const ws = new WebSocket(`${enviroment.webSocketUrl_ssl}/${userId}`);
        ws.onclose = (e) => this.handleClose(e);
        ws.onerror = (e) => this.handleError(e);
        ws.onmessage = (e) => this.handleMessage(e);
        ws.onopen = (e) => this.handleOpen(e);
        this.ws = ws;
    }

    disconnect() {
        if (!this.connectionIsOpen) return;
        this.ws.close();
    }

    handleMessage(event) {
        const msg = JSON.parse(event.data);
        const key = this.genKey(msg.path, msg.method);
        this.routes.route(key, msg.data)
    }

    handleError(event) {
        this.routes.route('error', event);
    }

    handleOpen(event) {
        this.routes.route('open', event);
    }

    handleClose(event) {
        this.routes.route('close', event);
    }

    sendMessage(path, method, data) {
        const msg =
        {
            path: path,
            method: method,
            data: data
        };
        const jsonMsg = JSON.stringify(msg);
        this.ws.send(jsonMsg);
    }

    get connectionIsOpen() {
        return this.ws && this.ws.readyState === 1;
    }
}
class MessageRouter {
    routes = {};

    route(key, msg) {
        const handlers = this.routes[key];
        if (!handlers) return;
        handlers.forEach(objt => objt.handler(msg));
    }

    on(key, handler) {
        if (!this.routes[key]) this.routes[key] = [];
        const newHandler = this.createHandler(handler);
        this.routes[key].push(newHandler);
        return () => {
            const index = this.routes[key].findIndex(x => x.id === newHandler.id);
            this.routes[key].splice(index, 1);
        }
    }
    createHandler(handler) {
        const id = Math.round((Math.random() * Number.MAX_SAFE_INTEGER));
        return {
            handler,
            id
        }
    }
}