Blazor.addEventListener('enhancedload', () => {
    hljs.highlightAll();

    if (window.location.hash === "") {
        let pageContentElement = document.getElementById("pagecontent");

        pageContentElement.classList.add("fade-slide-in");

        pageContentElement.addEventListener("animationend", () => {
            pageContentElement.classList.remove("fade-slide-in");
        }, { once: true });

        window.scrollTo(0, 0);
    }
});