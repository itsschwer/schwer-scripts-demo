function loadProgress(unityInstance, progress) {
    if (!unityInstance.Module) return;

    if (!unityInstance.progress) {
        unityInstance.progress = document.createElement("div");
        unityInstance.progress.className = "progress";

        unityInstance.progress.clip = document.createElement("div");
        unityInstance.progress.clip.className = "clip";
        unityInstance.progress.clip.fill = document.createElement("div");
        unityInstance.progress.clip.fill.className = "fill";
        unityInstance.progress.clip.appendChild(unityInstance.progress.clip.fill);
        unityInstance.progress.appendChild(unityInstance.progress.clip);

        unityInstance.progress.svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
        unityInstance.progress.svg.setAttribute("viewBox", "0 0 80 80");
        unityInstance.progress.svg.rect = document.createElementNS("http://www.w3.org/2000/svg", "rect");
        unityInstance.progress.svg.rect.setAttribute("x", "8");
        unityInstance.progress.svg.rect.setAttribute("y", "8");
        unityInstance.progress.svg.rect.setAttribute("width", "64");
        unityInstance.progress.svg.rect.setAttribute("height", "64");
        unityInstance.progress.svg.appendChild(unityInstance.progress.svg.rect);
        unityInstance.progress.appendChild(unityInstance.progress.svg);

        unityInstance.container.appendChild(unityInstance.progress);
    }
    unityInstance.progress.clip.style.height = (100 * progress) + "%";
    if (progress == 1) unityInstance.progress.style.display = "none";
}