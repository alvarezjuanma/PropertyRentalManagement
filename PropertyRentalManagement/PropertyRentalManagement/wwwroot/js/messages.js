function viewMessage(messageId) {
    fetch(`/Messages/GetMessageById?id=${messageId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById("messageSender").innerText = data.senderName;
            document.getElementById("messageSubject").innerText = data.subject;
            document.getElementById("messageDate").innerText = new Date(data.sentAt).toLocaleString();
            document.getElementById("messageBody").innerText = data.body;
        })
        .catch(error => console.error("Error fetching message:", error));
}

function replyMessage() {
    const sender = document.getElementById("messageSender").innerText;
    const subject = document.getElementById("messageSubject").innerText;
    const recipientInput = document.getElementById("recipient");
    const subjectInput = document.getElementById("subject");

    // Prefill the recipient and subject fields for replying
    recipientInput.value = sender;
    subjectInput.value = subject.startsWith("Re: ") ? subject : "Re: " + subject;

    // Optionally, scroll to the compose section
    document.querySelector('.mailbox-compose').scrollIntoView({ behavior: 'smooth' });
}
