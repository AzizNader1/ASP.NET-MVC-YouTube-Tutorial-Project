
"use strict";

document.addEventListener("DOMContentLoaded", () => {
    const menuButton = document.querySelector("[data-menu-toggle]");
    const navigation = document.querySelector("[data-main-nav]");

    if (menuButton && navigation) {
        menuButton.addEventListener("click", () => {
            const isOpen = navigation.classList.toggle("is-open");

            menuButton.setAttribute("aria-expanded", String(isOpen));
            menuButton.setAttribute(
                "aria-label",
                isOpen ? "Close navigation" : "Open navigation"
            );
        });

        navigation.addEventListener("click", event => {
            if (event.target.closest("a")) {
                navigation.classList.remove("is-open");
                menuButton.setAttribute("aria-expanded", "false");
            }
        });
    }

    document.querySelectorAll("[data-dismiss-alert]").forEach(button => {
        button.addEventListener("click", () => {
            button.closest("[data-alert]")?.remove();
        });
    });

    document.querySelectorAll("[data-confirm]").forEach(element => {
        element.addEventListener("click", event => {
            const message =
                element.dataset.confirm ||
                "Are you sure you want to continue?";

            if (!window.confirm(message)) {
                event.preventDefault();
            }
        });
    });
});
