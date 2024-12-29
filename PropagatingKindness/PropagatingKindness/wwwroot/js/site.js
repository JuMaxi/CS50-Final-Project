// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let elements = document.getElementsByClassName('passwd-toggle');

for (let i = 0; i < elements.length; i++)
{
    elements[i].addEventListener('click', function () {
        let passwdFieldId = elements[i].getAttribute('passwd-field');
        const passwordField = document.getElementById(passwdFieldId);
        //const passwordField = document.getElementById('Password');
        const passwordType = passwordField.getAttribute('type');
        if (passwordType === 'password') {
            passwordField.setAttribute('type', 'text');
            this.innerHTML = '<i class="bi bi-eye-fill"></i>';
        } else {
            passwordField.setAttribute('type', 'password');
            this.innerHTML = '<i class="bi bi-eye-slash-fill"></i>';
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    let dropdownToggle = document.getElementById("dropdownToggle");
    let dropdownMenu = document.querySelector("#userDropdown .user-dropdown-menu");

    if (dropdownToggle != null) {
        dropdownToggle.addEventListener("click", function (e) {
            e.preventDefault();
            dropdownMenu.classList.toggle("show");
        });

        // Close the dropdown if clicked outside
        document.addEventListener("click", function (e) {
            if (!dropdownToggle.contains(e.target) && !dropdownMenu.contains(e.target)) {
                dropdownMenu.classList.remove("show");
            }
        });
    }

    let advertThumbnails = document.getElementsByClassName('thumbnail');
    for (let i = 0; i < advertThumbnails.length; i++) {
        advertThumbnails[i].addEventListener('click', function () {
            let imageSrc = advertThumbnails[i].firstElementChild.getAttribute('src');
            document.getElementById('main-advert-image').src = imageSrc;
        });
    }
});

