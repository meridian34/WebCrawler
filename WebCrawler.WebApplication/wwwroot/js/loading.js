function CallLoadingAnimation() {
    
    //let button = document.getElementById("searchButton");
    //button.disabled = true;

    let parent = document.getElementById("loading");

    let elementsCollection = parent.getElementsByClassName("loader bodyElement");
    if (elementsCollection.length === 0) {
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

    //window.location.reload();


    //document.getElementById("loading").innerHTML = `
    //<div>
    //    <link rel="stylesheet" href="/css/Loading.css" type="text/css" />
    //    <div class="loader body">
    //        <div class="line one"></div>
    //        <div class="line two"></div>
    //        <div class="line three"></div>
    //    </div>
    //</div>`;
}