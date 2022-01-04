function CallLoadingAnimation() {
    
    let parent = document.getElementById("loading");
    let input = document.getElementById("urlField");
     

    let elementsCollection = parent.getElementsByClassName("loader bodyElement");
    if (elementsCollection.length === 0 && isValidUrl(input.value)) {
        let css = document.createElement("link");
        css.rel = "stylesheet";
        css.href = "/css/Loading.css";
        css.type = "text/css";
        parent.appendChild(css);


        let header = document.createElement("h2");
        header.textContent = "Идет загрузка не паникуйте...";
        header.className = "elementInTheCenter";
        parent.appendChild(header);

        let loaderBody = document.createElement("div");
        loaderBody.className = "loader bodyElement";

        let firstLine = document.createElement("div");
        firstLine.className = "line one";

        let secondLine = document.createElement("div");
        secondLine.className = "line two";

        let thirdLine = document.createElement("div");
        thirdLine.className = "line three";

        loaderBody.appendChild(firstLine);
        loaderBody.appendChild(secondLine);
        loaderBody.appendChild(thirdLine);
        parent.appendChild(loaderBody);
    }
}

function isValidUrl(string) {
    let url;

    try {
        url = new URL(string);
    } catch (_) {
        return false;
    }

    return url.protocol === "http:" || url.protocol === "https:";
}