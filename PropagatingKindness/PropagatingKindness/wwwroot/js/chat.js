// Initialize variables
let mostRecentDateTime = null;
let fetchTimeout = null;
let lastChat = -1;

function createMessageBalloon(msg) {
    let html = '';
    const icon = msg.Status == 3 ? "bi-eye-fill" : "bi-check";
    if (msg.IsSent) {
        html = `
        <div class="chat-message sent">
                <div class="chat-bubble sent">
                    ${msg.Message}
                    <i class="bi ${icon} bubble-icon chat-bubble-status-sent"></i>
                </div>
                <img src="${msg.UserPhoto}" alt="User Avatar" class="avatar">
            </div>
        `;
    } else {
        html = `
        <div class="chat-message received">
                <img src="${msg.UserPhoto}" alt="User Avatar" class="avatar">
                <div class="chat-bubble received">
                    ${msg.Message}
                    <i class="bi ${icon} bubble-icon chat-bubble-status-received"></i>
                </div>
            </div>
        `;
    }

    const container = document.createElement('div');
    container.innerHTML = html;
    return container.firstChild;
}

// Function to fetch messages from the endpoint
function getMessages(id) {
    // Clear any existing timeout
    if (fetchTimeout) {
        clearTimeout(fetchTimeout);
        fetchTimeout = null;
    }

    if (id != lastChat) {
        // Clear the chat
        // Update the chat header (advert)
        lastChat = id;
    }

    fetch(`/Chat/GetMessages/${id}`)
        .then(response => {
            if (!response.ok) {
                console.error("Failed to fetch messages", response.status);
                return Promise.reject();
            }
            //return response.json();
        })
        .then(data => {
            if (data.Messages && Array.isArray(data.Messages)) {
                const messagesDiv = document.getElementById("chat-messages");

                // Filter new messages based on mostRecentDateTime
                const newMessages = data.Messages.filter(message => {
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
            }
        })
        .catch(error => {
            console.error("Error fetching messages:", error);
        })
        .finally(() => {
            // Set a new timeout to call this function again after a few seconds
            fetchTimeout = setTimeout(() => getMessages(id), 5000);
        });
}

