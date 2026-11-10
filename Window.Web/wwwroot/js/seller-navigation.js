(() => {
    const button = document.getElementById("seller-more-button");
    const items = document.getElementById("seller-more-items");
    if (!button || !items) return;

    const navigation = button.closest("nav");
    const setExpanded = (expanded) => {
        items.hidden = !expanded;
        button.setAttribute("aria-expanded", String(expanded));
    };
    button.addEventListener("click", () => setExpanded(items.hidden));
    navigation.addEventListener("keydown", (event) => {
        if (event.key === "Escape" && !items.hidden) {
            event.preventDefault();
            setExpanded(false);
            button.focus();
        }
    });

    // Reserve the actual bar height, including wrapping and the phone's safe area.
    new ResizeObserver(() => {
        const height = Math.ceil(navigation.getBoundingClientRect().height);
        document.body.style.setProperty("--seller-navigation-height", height + "px");
    }).observe(navigation);
})();
