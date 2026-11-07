for (const button of document.querySelectorAll('[data-password-toggle]')) {
    const input = button.parentElement.querySelector('input');
    button.hidden = false;
    button.addEventListener('click', () => {
        const show = input.type === 'password';
        input.type = show ? 'text' : 'password';
        button.textContent = show ? 'پنهان' : 'نمایش';
    });
}

for (const form of document.querySelectorAll('[data-resend-seconds]')) {
    const button = form.querySelector('button');
    const label = form.querySelector('[data-resend-label]');
    const time = form.querySelector('[data-resend-time]');
    const until = Date.now() + (Number(form.dataset.resendSeconds) || 0) * 1000;
    function tick() {
        const seconds = Math.max(0, Math.ceil((until - Date.now()) / 1000));
        button.hidden = seconds > 0;
        label.hidden = seconds === 0;
        time.textContent = `${Math.floor(seconds / 60)}:${String(seconds % 60).padStart(2, '0')}`;
        if (seconds > 0) window.setTimeout(tick, 250);
    }
    tick();
}
