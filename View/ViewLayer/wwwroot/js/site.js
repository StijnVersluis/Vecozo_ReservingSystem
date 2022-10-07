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

$('#TeamSelectedModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var teamid = button.data('teamid') // Extract info from data-teamid attributes
    var teamname = button.data('teamname') // Extract info from data-teamname attributes
    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.

    var modal = $(this)
    modal.find('.modal-title').text('Team: ' + teamname)

    //fetch users
    var html = fetch(window.location.origin + "/Team/Getusers/" + teamid).then(resp => resp.text()).then(data => GenerateTeamModal(modal, data));
    console.log(html);
})

function GenerateTeamModal(modal, data) {

    let count = data.lastIndexOf("\,");
    let result = data.substring(0, count) + data.substring(count + 1)

    let userJson = JSON.parse(result.replaceAll("\\", ""))
    console.log(userJson);
    let userHtml = "<div class=\"list-group\">"
    let users = userJson.Users;
    for (var i = 0; i < Object.keys(users).length; i++) {
        console.log(users[i])
        userHtml +=
            "<div class=\"list-group-item\"><div class=\"ml-3\">"
            + "<input type=\"checkbox\" checked=\"true\" class=\"UserCheckBox form-check-input\" name=\"UserCheckBox"
            + users[i].Id
            + "\"><label class=\"form-check-label\" for=\"UserCheckBox"
            + users[i].Id
            + "\">"
            + users[i].Name
            + "</label></div></div>"
    }
    console.log(userHtml)
    modal.find('.modal-body').html(userHtml)
}