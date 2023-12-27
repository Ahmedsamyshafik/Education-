let add = document.querySelector(".fa-circle-plus");
console.log(add);
let AddDIv = document.querySelector(".add");
console.log(AddDIv);

add.addEventListener("click", function () {
  AddDIv.classList.toggle("active");
});
