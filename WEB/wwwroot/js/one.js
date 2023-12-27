
// collection متنوعات
// let collection = document.querySelector(".collection")
let linkInCollections= Array.from(document.querySelectorAll(".collection a"))
console.log(linkInCollections)
linkInCollections.forEach(link =>{
  link.setAttribute("target" ,"_blank")
})





// add video

    let add = document.querySelector(".fa-circle-plus")
    let AddDIv = document.querySelector(".add")

add.addEventListener("click" , function(){
  AddDIv.classList.toggle("active")
})
let addLink = document.querySelector(".link");
let addTitle = document.querySelector(".title");
let btn = document.querySelector(".btn");
let collection = document.querySelector(".collection");

let linkInCollection = JSON.parse(localStorage.getItem('linkCollection')) || [];

// Populate collection on page load
linkInCollection.forEach(linkData => {
    let a = createLinkElement(linkData.link, linkData.title);
    collection.appendChild(a);
});

btn.addEventListener("click", addINPage);

function addINPage() {
    if (addLink.value && addTitle.value && isValidURL(addLink.value)) {
        let a = createLinkElement(addLink.value, addTitle.value);
        linkInCollection.push({ link: addLink.value, title: addTitle.value });
        localStorage.setItem('linkCollection', JSON.stringify(linkInCollection));
        collection.appendChild(a);
        addLink.value = "";
        addTitle.value = "";
    }
}

function createLinkElement(link, title) {
    let a = document.createElement("a");
    let img = document.createElement("img");
    img.src = "images/youtube2.png";
    a.href = link;
    a.setAttribute("target", "_blank");
    a.appendChild(img);
    let h3 = document.createElement("h3");
    let textH3 = document.createTextNode(title);
    h3.appendChild(textH3);
    h3.style.overflow = "hidden";
    a.appendChild(h3);
    return a;
}

function isValidURL(url) {
    let pattern = /^(https?:\/\/)?([a-z\d.-]+)\.([a-z]{2,6})(\/([^\s]*))?$/;
    return pattern.test(url);
}

let clear = document.querySelector(".clear");
clear.addEventListener("click" , clearPage)

function clearPage() {
    // collection.innerHTML = "";
    localStorage.removeItem('linkCollection');
    location.reload()
}