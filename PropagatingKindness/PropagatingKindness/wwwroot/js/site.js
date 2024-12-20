﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
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


