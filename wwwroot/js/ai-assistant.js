document.addEventListener('DOMContentLoaded', () => {
    const btn = document.getElementById('ai-assistant-btn');
    const panel = document.getElementById('ai-panel');
    const closeBtn = document.getElementById('ai-close-btn');
    const chatBody = document.getElementById('ai-chat-body');
    const input = document.getElementById('ai-input');
    const sendBtn = document.getElementById('ai-send-btn');

    let sessionId = 'session-' + Math.random().toString(36).substr(2, 9);

    if (!btn || !panel) return;

    btn.addEventListener('click', () => {
        panel.style.display = 'flex';
        // tiny delay to allow display:flex to apply before transition
        setTimeout(() => panel.classList.add('open'), 10);
        input.focus();
    });

    closeBtn.addEventListener('click', () => {
        panel.classList.remove('open');
        setTimeout(() => panel.style.display = 'none', 300);
    });

    function appendMessage(role, text, metadata = null) {
        const msgDiv = document.createElement('div');
        msgDiv.className = `ai-msg ${role}`;
        
        // Simple Markdown parsing for bold and links
        let formattedText = text
            .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
            .replace(/\[(\d+)\]/g, '<span class="ai-citation-badge" title="Source Document Reference">$1</span>')
            .replace(/\n/g, '<br/>');

        msgDiv.innerHTML = formattedText;

        if (metadata && role === 'assistant') {
            const confClass = metadata.confidence > 0.8 ? 'confidence-high' : 
                              (metadata.confidence > 0.5 ? 'confidence-med' : 'confidence-low');
            
            const confHtml = `<span class="ai-confidence ${confClass}">
                <i class="fas fa-check-circle"></i> ${(metadata.confidence * 100).toFixed(1)}% Confidence
            </span>`;
            msgDiv.innerHTML += confHtml;

            if (metadata.hallucination && metadata.hallucination.hallucination_risk > 0.4) {
                msgDiv.innerHTML += `<span class="ai-confidence confidence-low"><i class="fas fa-exclamation-triangle"></i> High Hallucination Risk</span>`;
            }
        }

        chatBody.appendChild(msgDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
    }

    function appendTypingIndicator() {
        const msgDiv = document.createElement('div');
        msgDiv.className = `ai-msg assistant typing-indicator`;
        msgDiv.id = 'ai-typing-indicator';
        msgDiv.innerHTML = `<div class="ai-typing"><span></span><span></span><span></span></div>`;
        chatBody.appendChild(msgDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
    }

    function removeTypingIndicator() {
        const ind = document.getElementById('ai-typing-indicator');
        if (ind) ind.remove();
    }

    async function sendMessage() {
        const text = input.value.trim();
        if (!text) return;

        input.value = '';
        input.disabled = true;
        sendBtn.disabled = true;

        appendMessage('user', text);
        appendTypingIndicator();

        // Dynamically get ContextType if on a specific dashboard, else General
        let contextType = "General";
        const path = window.location.pathname.toLowerCase();
        if (path.includes('patient')) contextType = "Patient";
        else if (path.includes('doctor') || path.includes('clinical')) contextType = "Clinical";
        else if (path.includes('billing') || path.includes('receptionist')) contextType = "Billing";
        else if (path.includes('admin')) contextType = "Admin";

        try {
            const response = await fetch('/api/ai/query', {
                method: 'POST',
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    question: text,
                    session_id: sessionId,
                    context_type: contextType
                })
            });

            removeTypingIndicator();

            if (!response.ok) {
                let errorMsg = "I'm having trouble connecting to the network right now.";
                try {
                    const errObj = await response.json();
                    if (errObj.message) errorMsg = errObj.message;
                } catch(e) {}
                appendMessage('assistant', errorMsg);
                return;
            }

            const data = await response.json();
            appendMessage('assistant', data.answer, data);

        } catch (error) {
            removeTypingIndicator();
            appendMessage('assistant', "An error occurred connecting to the MediAI core.");
            console.error(error);
        } finally {
            input.disabled = false;
            sendBtn.disabled = false;
            input.focus();
        }
    }

    sendBtn.addEventListener('click', sendMessage);
    input.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') sendMessage();
    });
});
