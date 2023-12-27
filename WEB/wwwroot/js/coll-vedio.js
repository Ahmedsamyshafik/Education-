let fileInput = document.getElementById("fileInput");
let fileNameInput = document.getElementById("fileNameInput");
let videoContainer = document.getElementById("videoContainer");
let para = document.querySelector(".pragraph");
let num = 1;
document.getElementById("saveButton").addEventListener("click", function () {
  if (fileInput.value && fileNameInput.value && isValidURL(fileInput.value)) {
    let add = `
        <div class="links">
        <h1>${fileNameInput.value}</h1>
        <button onclick="openVideo('${fileInput.value}')">Open Video</button>
        </div>
    `;

    document
      .getElementById("videoContainer")
      .insertAdjacentHTML("beforeend", add);
    clearInputs();
  }
});

function openVideo(videoUrl) {
  let videoHtml = `
        <div style="position:relative;height:100%;"><iframe src="${videoUrl}" loading="lazy" style="border:0;position:absolute;top:0;height:100%;width:100%;" allow="accelerometer;gyroscope;autoplay;encrypted-media;picture-in-picture;" allowfullscreen="true"></iframe></div>
    `;
  let videoWindow = window.open("", "_blank");
  videoWindow.document.body.innerHTML = videoHtml;
}

let add = document.querySelector(".fa-circle-plus");
let AddDIv = document.querySelector(".add");
add.addEventListener("click", function () {
  AddDIv.classList.toggle("active");
});

function isValidURL(url) {
  let pattern = /^(https?:\/\/)?([a-z\d.-]+)\.([a-z]{2,6})(\/([^\s]*))?$/;
  return pattern.test(url);
}

function clearInputs() {
  fileInput.value = "";
  fileNameInput.value = "";
  para.value = "";
}
