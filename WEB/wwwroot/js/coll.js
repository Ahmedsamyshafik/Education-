

let courseImage = document.querySelector(".fileInput");
let courseName = document.querySelector(".title");
let coursePrice = document.querySelector(".price");
let save = document.getElementById("saveButton");
let lectures = document.querySelector(".lectures");
lectures.style.cssText = "border-bottom:0 ;";

save.addEventListener("click", function () {
  if (
    (courseImage.value != "") &
    (courseName.value != "") &
    (coursePrice.value != "")
  ) {
    addInPage();
  }
});

let num = 1;

function addInPage() {
  let name = courseName.value;
  let price = coursePrice.value;
  let imageCOur = courseImage.value;

  // Create div with the class name "one"
  let div = document.createElement("div");
  div.className = "one";

  uploadImage(div);

  // Create div with the class name "hov"
  let divHov = document.createElement("div");
  divHov.className = "hov";
  div.appendChild(divHov);

  // Create div with the class name "title"
  let divTitle = document.createElement("div");
  divTitle.className = "title";
  let a = document.createElement("a");
  a.target = "_blank";

  // Create div for name and price
  let divNameAndPrice = document.createElement("div");
  let h3 = document.createElement("h3");
  let h3Text = document.createTextNode(name);
  h3.appendChild(h3Text);

  h3.style.cssText = "margin: 0; width: fit-content ; border:0";
  divNameAndPrice.appendChild(h3);

  // Create p element for price
  let priceh3 = document.createElement("p");
  let priceText = document.createTextNode(`Price: ${price}`);
  priceh3.appendChild(priceText);
  divNameAndPrice.appendChild(priceh3);

  // checkbox and lable
  let divCheck = document.createElement("div");
  divCheck.className = "lable";
  let checkboxInput = document.createElement("input");
  checkboxInput.type = "checkbox";

  let lableCheck = document.createElement("label");
  // setatrribu
  checkboxInput.setAttribute("id", `${num}`);
  lableCheck.setAttribute("for", `${num++}`);

  let textLable = document.createTextNode("Join");
  lableCheck.appendChild(textLable);
  divCheck.appendChild(checkboxInput);
  divCheck.appendChild(lableCheck);

  divNameAndPrice.style.cssText =
    "display: flex; flex-direction: row-reverse; justify-content: space-between; border-bottom: 2px solid var(--main-red)";
  a.appendChild(divNameAndPrice);
  a.href = "vedios.html";
  divTitle.appendChild(a);
  div.appendChild(divTitle);
  div.appendChild(divCheck);

  let p = document.createElement("p");
  let textp = document.createTextNode("اضغط هنا");
  p.appendChild(textp);
  a.appendChild(p);

  // Append the div to "lectures" element
  lectures.appendChild(div);

  courseImage.value = "";
  courseName.value = "";
  coursePrice.value = "";
}

function uploadImage(targetDiv) {
  let input = document.getElementById("fileInput");
  let file = input.files[0];
  let formData = new FormData();
  formData.append("image", file);

  var reader = new FileReader();
  reader.onload = function (e) {
    var image = document.createElement("img");
    image.style.padding = "0";
    image.src = e.target.result;
    targetDiv.appendChild(image);
  };

  reader.readAsDataURL(file);
}
