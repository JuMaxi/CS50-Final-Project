// Initialize variables
let mostRecentDateTime = null;
let fetchTimeout = null;
let lastChat = -1;

function createMessageBalloon(msg) {
    let html = '';
    const icon = msg.status == 3 ? "bi-eye-fill" : "bi-check";
    if (msg.isSent) {
        html = `
        <div class="chat-message sent">
                <div class="chat-bubble sent">
                    ${msg.message}
                    <i class="bi ${icon} bubble-icon chat-bubble-status-sent"></i>
                </div>
                <img src="${msg.userPhoto}" alt="User Avatar" class="avatar">
            </div>
        `;
    } else {
        html = `
        <div class="chat-message received">
                <img src="${msg.userPhoto}" alt="User Avatar" class="avatar">
                <div class="chat-bubble received">
                    ${msg.message}
                    <i class="bi ${icon} bubble-icon chat-bubble-status-received"></i>
                </div>
            </div>
        `;
    }

    const container = document.createElement('div');
    container.innerHTML = html;
    return container.firstElementChild;
}

function renderHeader(response) {
    const html = `<a href="/Advert/View/${response.advert.advertId}" class="text-reset"><img src="${response.advert.photos[0]}" alt="Header Image">
            <span class="mb-0 fs-4 align-middle">${response.advert.name}</span></a>`;
    document.getElementById("chat-header").innerHTML = html;
}

function clearHeader() {
    const html = `<h5 class="mb-0"><em>Loading...</em></h5>`;
    document.getElementById("chat-header").innerHTML = html;
}

function clearMessages() {
    document.getElementById("chat-messages").innerHTML = "";
}

function disableSending() {
    var input = document.getElementById('message-input');
    var button = document.getElementById('button-send');
    input.setAttribute("disabled", "disabled");
    button.setAttribute("disabled", "disabled");
}

function enableSending() {
    var input = document.getElementById('message-input');
    var button = document.getElementById('button-send');
    input.removeAttribute("disabled", "disabled");
    button.removeAttribute("disabled", "disabled");
}

function addMessage(element) {
    const messagesDiv = document.getElementById("chat-messages");
    let h3nomsg = document.getElementById('chat-no-messages');
    if (h3nomsg !== null && h3nomsg !== undefined)
        messagesDiv.innerHTML = '';
    messagesDiv.appendChild(element);
}

function sendMessage() {
    disableSending();
    var input = document.getElementById('message-input');

    fetch(`/Chat/SendMessage/${lastChat}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Message: input.value })
    })
    .then(response => {
        if (!response.ok) {
            console.error("Failed to fetch messages", response.status);
            return Promise.reject();
        }
        return response.json();
    })
    .then(data => {
        const messageElement = createMessageBalloon(data);
        mostRecentDateTime = new Date();
            //document.getElementById("chat-messages").appendChild(messageElement);
        addMessage(messageElement);
        input.value = '';
        enableSending();
    })
    .catch(error => {
        alert(`Error sending message: ${error}`);
        input.value = '';
        enableSending();
    });
}

// Function to fetch messages from the endpoint
function getMessages(id) {
    // Clear any existing timeout
    if (fetchTimeout) {
        clearTimeout(fetchTimeout);
        fetchTimeout = null;
    }

    let newChat = id != lastChat;
    if (newChat) {
        disableSending();
        clearHeader();
        clearMessages();
        lastChat = id;
    }

    fetch(`/Chat/GetMessages/${id}`)
        .then(response => {
            if (!response.ok) {
                console.error("Failed to fetch messages", response.status);
                return Promise.reject();
            }
            return response.json();
        })
        .then(data => {
            if (newChat)
                renderHeader(data);
            const messagesDiv = document.getElementById("chat-messages");

            if (data.messages && Array.isArray(data.messages) && data.messages.length > 0) {

                // Filter new messages based on mostRecentDateTime
                const newMessages = data.messages.filter(message => {
                    const messageDateTime = new Date(message.dateTime);
                    return !mostRecentDateTime || messageDateTime > new Date(mostRecentDateTime);
                });

                // Add new messages to the chat-messages div
                for (let i = 0; i < newMessages.length; i++) {
                    let messageElement = createMessageBalloon(newMessages[i]);
                    //messagesDiv.appendChild(messageElement);
                    addMessage(messageElement);
                }
                //newMessages.forEach(message => {
                //    const messageElement = createMessageBalloon(message);
                //    messagesDiv.appendChild(messageElement);
                //});

                // Update the most recent DateTime
                if (newMessages.length > 0) {
                    mostRecentDateTime = newMessages[newMessages.length - 1].dateTime;
                }
            } else { 
                messagesDiv.innerHTML = '<h3 id="chat-no-messages">No messages yet</h3><h5>Why don\'t you break the ice?</h5>';
            }

            if (newChat)
                enableSending();
        })
        .catch(error => {
            console.error("Error fetching messages:", error);
        })
        .finally(() => {
            // Set a new timeout to call this function again after a few seconds
            fetchTimeout = setTimeout(() => getMessages(id), 3000); // This should be parametrizable somehow
        });
}

document.addEventListener("DOMContentLoaded", function () {
    // Allow sending messages with Enter 
    let msgbox = document.getElementById('message-input');
    msgbox.addEventListener("keyup", function (event) {
        if (event.key === "Enter") {
            sendMessage();
        }
    });

    // Load the latest chat
    let chats = document.getElementsByClassName('chat-conversation');
    if (chats && chats.length > 0) {
        latestChat = chats[0].getAttribute('chat-id');
        getMessages(latestChat);
    }
});

