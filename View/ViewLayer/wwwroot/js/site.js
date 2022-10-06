// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function CheckTeamInput() {
    if (document.getElementById("TeamCheckBox").checked) {
        //Teams on
        document.getElementById("TeamCheckBoxOff").classList.add("d-none")
        document.getElementById("TeamCheckBoxOn").classList.remove("d-none")
        document.getElementById("TeamList").classList.remove("d-none")
    } else {
        //Teams off
        document.getElementById("TeamCheckBoxOff").classList.remove("d-none")
        document.getElementById("TeamCheckBoxOn").classList.add("d-none")
        document.getElementById("TeamList").classList.add("d-none")
    }
}

function ChangeWorkzoneSelect(value) {
}