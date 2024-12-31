﻿// Initialize variables
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
    return container.firstChild;
}

function renderHeader(response) {
    const html = `<a href="/Advert/View/${response.advert.advertId}"><img src="${response.advert.photos[0]}" alt="Header Image">
            <h5 class="mb-0">${response.advert.Name}</h5></a>`;
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

function sendMessage() {
    disableSending();

    return new Promise((resolve, reject) => {
        fetch(`/Chat/SendMessage/${lastChat}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Message: input.value })
        })
        .then(response => {
            if (!response.ok) {
                return reject(`Failed with status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            const messageElement = createMessageBalloon(data);
            document.getElementById("chat-messages").appendChild(messageElement);
            enableSending();
        })
        .catch(error => {
            alert(`Error sending message: ${error}`);
            enableSending();
        });
    });
}

// Function to fetch messages from the endpoint
function getMessages(id) {
    // Clear any existing timeout
    if (fetchTimeout) {
        clearTimeout(fetchTimeout);
        fetchTimeout = null;
    }

    if (id != lastChat) {
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
            renderHeader(data);
            const messagesDiv = document.getElementById("chat-messages");

            if (data.messages && Array.isArray(data.messages) && data.messages.length > 0) {

                // Filter new messages based on mostRecentDateTime
                const newMessages = data.messages.filter(message => {
                    const messageDateTime = new Date(message.DateTime);
                    return !mostRecentDateTime || messageDateTime > new Date(mostRecentDateTime);
                });

                // Add new messages to the chat-messages div
                newMessages.forEach(message => {
                    const messageElement = createMessageBalloon(message);
                    messagesDiv.appendChild(messageElement);
                });

                // Update the most recent DateTime
                if (newMessages.length > 0) {
                    mostRecentDateTime = newMessages[newMessages.length - 1].DateTime;
                }
            } else { 
                messagesDiv.innerHTML = '<h3>No messages yet</h3><h5>Why don\'t you break the ice?</h5>';
            }
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
