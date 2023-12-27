let linksLogo = Array.from(document.querySelectorAll(".logo a"));
linksLogo.forEach((linkLogo) => {
  linkLogo.setAttribute("target", "_blank");
});

let linksHead = document.querySelectorAll(".link ul li a");

linksHead.forEach((link) => {
  link.classList.remove("active");
  link.setAttribute("target", "_blank");
});

let dateone = Date.now();

let newDate = new Date(dateone);
let year = newDate.getFullYear();
console.log(year);

document.querySelector(".year").innerHTML = year;
