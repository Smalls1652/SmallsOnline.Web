Blazor.addEventListener('enhancedload', () => {
    hljs.highlightAll();
    
    let pageContentElement = document.getElementById("pagecontent");

    pageContentElement.classList.add("fade-slide-in");
    
    pageContentElement.addEventListener("animationend", () => {
        pageContentElement.classList.remove("fade-slide-in");
    }, { once: true });
});
