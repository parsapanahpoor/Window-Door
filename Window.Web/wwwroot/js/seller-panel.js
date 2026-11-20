(() => {
    const panel = document.querySelector(".seller-page-body");
    if (!panel) return;

    const pageTitle = document.querySelector(".seller-page-title")?.textContent?.trim() ?? "";

    // The shared glass heading already shows the page title. Legacy widget
    // headers that repeat it are noise on every seller screen.
    panel.querySelectorAll(".widget-caption, .widget-header > .flex-style > span:first-child, .well.with-header > .header > .flex-style > span:first-child").forEach((caption) => {
        if (pageTitle && caption.textContent.trim() === pageTitle) {
            const header = caption.closest(".widget-header, .header");
            caption.remove();
            if (header && !header.textContent.trim() && !header.querySelector("a, button, input, select")) {
                header.remove();
            }
        }
    });

    panel.querySelectorAll("table.seller-table").forEach((table) => {
        const headers = Array.from(table.querySelectorAll("thead th")).map((th) => th.textContent.trim());
        table.querySelectorAll("tbody tr").forEach((row) => {
            const cells = Array.from(row.children).filter((cell) => cell.tagName === "TD");
            cells.forEach((cell, index) => {
                if (!cell.hasAttribute("data-label")) {
                    cell.setAttribute("data-label", headers[index] ?? "");
                }

                const controls = cell.querySelectorAll("a, button, input[type=submit], input[type=button]");
                const text = cell.textContent.replace(/\s+/g, "");
                if (controls.length > 0 && text === Array.from(controls).map((control) => control.textContent.replace(/\s+/g, "")).join("")) {
                    cell.classList.add("seller-table-actions");
                    if (!cell.querySelector(".seller-row-actions")) {
                        const wrapper = document.createElement("div");
                        wrapper.className = "seller-row-actions";
                        while (cell.firstChild) wrapper.appendChild(cell.firstChild);
                        cell.appendChild(wrapper);
                    }
                }
            });
        });
    });

    panel.querySelectorAll('input[type="file"]').forEach((input) => {
        if (input.closest(".seller-document")) return; // the profile picker has its own preview script
        if (input.closest(".seller-file")) return;

        const name = document.createElement("span");
        name.className = "seller-file-name";
        name.hidden = true;
        const error = document.createElement("span");
        error.className = "seller-file-error";
        error.hidden = true;
        error.setAttribute("role", "alert");

        input.insertAdjacentElement("afterend", name);
        name.insertAdjacentElement("afterend", error);

        input.addEventListener("change", () => {
            const file = input.files?.[0];
            name.hidden = !file;
            error.hidden = true;
            name.textContent = file ? file.name : "";

            if (!file) return;
            const accept = (input.getAttribute("accept") ?? "").toLowerCase();
            if (accept) {
                const extension = "." + file.name.split(".").pop().toLowerCase();
                const allowed = accept.split(",").map((item) => item.trim().toLowerCase());
                if (!allowed.some((item) => item === extension || item === file.type.toLowerCase())) {
                    error.textContent = "نوع فایل انتخاب‌شده مجاز نیست.";
                    error.hidden = false;
                    input.value = "";
                    name.hidden = true;
                }
            }
        });
    });

    const notices = document.querySelector(".seller-operation-notices");
    if (notices) {
        notices.setAttribute("aria-live", "polite");
    }

    // Bootstrap 3 publishes modal events through jQuery.
    const modalOpeners = new WeakMap();
    window.jQuery(document)
        .on("show.bs.modal", ".modal", function (event) {
            const opener = event.relatedTarget ?? document.activeElement;
            if (opener instanceof HTMLElement && !this.contains(opener)) {
                modalOpeners.set(this, opener);
            }
        })
        .on("shown.bs.modal", ".modal", function () {
            window.jQuery(this)
                .find(".modal-body :input:visible:enabled:not([readonly]):not(:button)")
                .first()
                .trigger("focus");
        })
        .on("hidden.bs.modal", ".modal", function () {
            modalOpeners.get(this)?.focus();
            modalOpeners.delete(this);
        });
})();
