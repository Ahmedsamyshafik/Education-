const form = document.getElementById("searchForm");
const table = document.getElementById("studentTable");

form.addEventListener("input", function (e) {
    const nameInput = document.getElementById("name");
    const degreeInput = document.getElementById("degree");

    const nameFilter = nameInput.value.toLowerCase();
    const degreeFilter = degreeInput.value.toLowerCase();

    const rows = table.getElementsByTagName("tr");

    for (let i = 1; i < rows.length; i++) {
        const name = rows[i].getElementsByTagName("td")[0].innerText.toLowerCase();
        const degree = rows[i]
            .getElementsByTagName("td")[1]
            .innerText.toLowerCase();

        if (
            (nameFilter === "" || name.includes(nameFilter)) &&
            (degreeFilter === "" || degree === degreeFilter)
        ) {
            rows[i].style.display = "";
        } else {
            rows[i].style.display = "none";
        }
    }
});

let scro = document.querySelector(".scrolling");

scro.addEventListener("click", function () {
    let x = document.querySelector(".listoftable");
    x.scrollIntoView({ behavior: "smooth" });
});