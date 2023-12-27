

// switch between login and signup
let signUp = document.querySelector("#signup");
let logIn = document.querySelector("#Login");
let formLogin = document.querySelector(".form-parent-login");
let formSignup = document.querySelector(".form-parent-signup");

signUp.addEventListener("click", function (e) {
  formLogin.style.display = "none";
  formSignup.style.display = "block";
});
logIn.addEventListener("click", function (e) {
  formSignup.style.display = "none";
  formLogin.style.display = "block";
});

// hide and show password

let allHide = document.querySelectorAll(".fa-eye-slash");

// 
allHide.forEach((hide) => {
  hide.addEventListener("click", function (e) {
    let foucs = e.currentTarget;
    let parentFoucs = foucs.parentElement.querySelector("input");
    // console.log(parentFoucs);
    if (parentFoucs.type === "password") {
      parentFoucs.type = "text";
      hide.classList.replace("fa-eye-slash", "fa-eye");
    } else {
      parentFoucs.type = "password";
      hide.classList.replace("fa-eye", "fa-eye-slash");
    }
  });
});

