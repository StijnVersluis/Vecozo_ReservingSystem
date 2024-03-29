﻿$(document).ready(function () {
    var date = new Date();
    date.setMilliseconds(null);
    date.setSeconds(null);
    date = date.toISOString().replace("T", " ").replace(":00.000Z", "")
    $("#DateSelectorInput").val(date);

    if (window.location.pathname == "/" && window.innerWidth > 576) {
        loadWorkzones();
        LoadImage();
    } else {
        $('#LoadingVisualWorkspots').alert('close')
    }
    if (window.location.pathname.indexOf("/Workzone/Edit/") == 0) {
        LoadXYImage()
    }
    if ($("#BhvRegisterDateTimeInput").length) {
        var date = new Date()
        var month = date.getMonth().toString().length == 2 ? date.getMonth() : "0" + date.getMonth()
        var day = date.getDate().toString().length == 2 ? date.getDate() : "0" + date.getDate()
        var hour = date.getHours().toString().length == 2 ? date.getHours() : "0" + date.getHours()
        var minutes = date.getMinutes().toString().length == 2 ? date.getMinutes() : "0" + date.getMinutes()
        var datestring = date.getFullYear() + "-" + month + "-" + day + "T" + hour + ":" + minutes
        //$("#BhvRegisterDateTimeInput").val(datestring)
    }

    $("input[name=datetime-start]").each(function () {
        console.log($(this))
        $(this).val($("#DateSelectorInput").val())
    })


});

$(document).ready(function () {
    var date = new Date();

    var currentDateFormat = date.toISOString().substring(0, 10) + " " + date.toLocaleString().substring(12, 17);
    $("#DateSelectorInput").val(currentDateFormat)
});

function StringIsEmpty(str) {
    return (str?.trim()?.length || 0) === 0;
}

function toLocalTime() {
    var date = new Date();
    var addZ = (n) => {
        return (n < 10 ? '0' : '') + n;
    }

    return date.getFullYear() + '-' +
        addZ(date.getMonth() + 1) + '-' +
        addZ(date.getDate());
}

function CheckTeamInput() {
    date = null;
    if (StringIsEmpty($("#DateSelectorInput").val())) {
        date = new Date().toLocaleString();
    } else {
        date = new Date(Date.parse($("#DateSelectorInput").val())).toLocaleString();
    }

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
    SwitchTeamWorkzonesImages();
    loadWorkzones();
}

const staticPopupModals = [
    {
        name: 'adHocModal',
        handler: null
    },

    {
        name: 'editTeamModal',
        handler: async (team) => {
            if (team != null) {
                team.users.forEach(user => AddUserToTeam(user.id, user.name, team.owner.id == user.id ? true : false));
            }
        }
    },

    {
        name: 'viewTeamModal',
        handler: async (team) => {
            GenerateTeamModal($('#viewTeamModal'), team);
        }
    }
];

staticPopupModals.forEach(async obj => {
    $(`#${obj.name}`).modal({ backdrop: 'static', keyboard: false }, 'show');
    if (obj.handler != null) {
        const teamId = $('.team-modal-hidden-id').val();
        if (teamId != null) {
            const team = await getTeamInfo($('.team-modal-hidden-id').val());
            obj.handler(team);
        }
    }
});

$("#DateSelectorInput").on('change', function (event) {
    if (StringIsEmpty(event.target.value)) return;
    console.log($("input[name=datetime-coming]"))
    $("input[name=datetime-start]").each(function () {
        console.log($(this))
        $(this).val($("#DateSelectorInput").val())
    })
    loadWorkzones();
});

$('#TeamSelectedModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var teamid = button.data('teamid') // Extract info from data-teamid attributes
    var teamname = button.data('teamname') // Extract info from data-teamname attributes
    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.

    var modal = $(this)
    modal.find('.modal-title').text('Team: ' + teamname)

    //fetch users
    var html = GetUsers(modal, teamid);
});

function GetUsers(modal, teamId) {
    return fetch(window.location.origin + "/Team/Users/" + teamId).then(resp => resp.text()).then(data => GenerateTeamModal(modal, data));
}

function GenerateTeamModal(modal, data) {
    let rawHtml = "<div class=\"list-group\">";

    if (data != null) {
        data.users.forEach(user => {
            var name = user.id == data.owner.id ? "(Admin) " + user.name : user.name
            rawHtml +=
                "<div class=\"list-group-item\"><label>"
                + name
                + "</label></div>"
        });
    } else {
        rawHtml += "Geen gegevens beschikbaar";
    }

    rawHtml += "</div>";
    modal.find('.team-modal-members').html(rawHtml);
}

$('#WorkzoneDefaultSelectedModal').on('shown.bs.modal', function (event) {
    var button = $(event.relatedTarget)
    var workzoneId = button.data('workzone-id')
    var workzoneName = button.data('workzone-name')
    var datetimeArrive = $("#DateSelectorInput").val()

    var modal = $(this)
    modal.find('.modal-title').text('Werkblok: ' + workzoneName)

    modal.find('#WorkzoneIdForm').val(workzoneId)

    var time = datetimeArrive.replace(/([0-9]{1,4}-[0-9]{1,2}-[0-9]{1,2})T/g, "")
    modal.find('#WorkzoneLeavingForm').attr({
        "min": time
    })
})


$('#WorkzoneTeamOnlySelectedModal').on('shown.bs.modal', function (event) {
    var button = $(event.relatedTarget)
    var modal = $(this);

    var workzoneName = button.data('workzoneName');
    var teamOnly = button.data('team-only');

    modal.find('.modal-title').text('Werkblok: ' + workzoneName);
    if (!teamOnly) {
        $("#teamOnlyBadge").css("display", "none");
    } else {
        $("#teamOnlyBadge").css("display", "block");
    }

    var arrivingTime = $('#DateSelectorInput').val();
    var workzoneId = button.data('workzoneId');

    var reserve = modal.find('.modal-reserve');
    var alert = modal.find('.alert');

    var team = $('.team-list-item-active')[0];
    if (team != null) {
        var teamId = $(team).attr('team-id');
        var teamName = $(team).attr('team-name');

        modal.find('#Workzone_id').val(workzoneId);
        modal.find('#TeamId').val(teamId);
        modal.find('#DateTime_Arriving').val(new Date(arrivingTime).toLocaleString());
        modal.find('.modal-team').text('Selected Team: ' + teamName);

        reserve.attr('disabled', false);
        alert.addClass('collapse');
    } else {
        modal.find('.modal-team').text('');
        reserve.attr('disabled', true);
        alert.removeClass('collapse');
    }
})

//$('#WorkzoneTeamOnlySelectedModal').on('hidden.bs.modal', function (e) {
//    clearSelectedTeams();
//})

$("#FilterUserInput").on("keyup", () => {
    var userbtns = $(".add-user-btn")
    var input = $("#FilterUserInput");
    var ft = fetch(window.location.origin + "/User/Filter?str=" + input.val())
        .then((response) => response.text())
        .then((data) => {
            $("#FilteredUserList").html(data)
            userbtns = $(".add-user-btn")
        });
})

document.querySelectorAll('.team-list-item').forEach(x => {
    x.addEventListener('click', _ => {
        var teamSelected = x.getAttribute('team-selected');
        var teamId = x.getAttribute('team-id');
        if (teamId == null || teamId == 0) return;

        clearSelectedTeams();

        if (teamSelected == 'true') {
            x.setAttribute('team-selected', 'false');
            x.classList.remove('team-list-item-active');
        } else if (teamSelected == 'false') {
            x.setAttribute('team-selected', 'true');
            x.classList.add('team-list-item-active')
        }
    })
})

function clearSelectedTeams() {
    document.querySelectorAll('.team-list-item').forEach(x => {
        x.setAttribute('team-selected', 'false');
        x.classList.remove('team-list-item-active');
    })
}

function getTimeOnly(datetime) {
    return datetime.replace(/([0-9]{1,4}-[0-9]{1,2}-[0-9]{1,2})T/g, "");
}

function AddUserToTeam(id, name, isAdmin) {
    var list = $("#TeamAddedUsers")
    var input = $("#AddedUserIds")
    var ids = $("#AddedUserIds")

    if (ids.val()?.indexOf(id) != -1) return
    if (input.val() != "") input.val(input.val() + "," + id)
    else input.val(id)
    if (list.html() == "") list.html(CreateUserAddedHtml(id, name, isAdmin))
    else {
        list.html(list.html() + CreateUserAddedHtml(id, name, isAdmin))
    }
}

function CreateUserAddedHtml(id, name, isAdmin) {
    var namediv;
    var button = $("<button>").text("-")
    var listItem = $("<div>")
    var returnlist = $("<div>")

    if (!isAdmin) {
        namediv = $("<span>").html("" + name)
        button.attr("type", "button")
        button.attr("onclick", "RemoveUser(" + id + ")")
        button.addClass("btn")
        button.addClass("btn-danger")
        button.addClass("w-auto")
        button.onclick = "RemoveUser(" + id + ")"
        listItem.addClass("p-1")

        listItem.append(button)
    } else {
        namediv = $("<span>").html("(Admin) " + name)
        listItem.addClass("p-2")
    }

    namediv.addClass("ml-2")

    listItem.append(namediv)
    listItem.attr("id", "AddedUser" + id)
    listItem.addClass("mt-1")
    listItem.addClass("list-group-item")
    returnlist.append(listItem)
    return returnlist.html()
}

function RemoveUser(id) {
    let userIds = $("#AddedUserIds").val()?.split(",");
    if (userIds == null) return;

    $("#AddedUser" + id)?.remove();
    if (userIds.find(x => x == id) != null) {
        $("#AddedUserIds").val(userIds.filter(x => x != id))
    }
}

async function getTeamInfo(id) {
    const response = await fetch(window.location.origin + `/Team/Info/${id}`);
    const object = await response.json();

    return object;
}

function FloorSelectChange() {
    loadWorkzones();
    LoadImage();
}


function loadWorkzones() {
    let floorId = $('#FloorSelectorSelect').val();
    let teamonly = $("#TeamCheckBox").prop('checked');

    $(".workzone-list-item").each(function () {
        workzone = $(this);

        if (workzone.data("floor") == floorId) {
            workzone.removeClass("d-none")
        } else {
            if (!workzone.hasClass("d-none")) workzone.addClass("d-none")
        }

        if (this.dataset.full == 'false' || '') {
            $(workzone).attr('data-toggle', 'modal');
        }

        if (!teamonly) {
            if (workzone.data("teamOnly") == "True" && !workzone.hasClass("d-none")) {
                workzone.addClass("d-none")
            }
            this.dataset.target = "#WorkzoneDefaultSelectedModal"
        } else {
            this.dataset.target = "#WorkzoneTeamOnlySelectedModal"
        }
    })
}


//Image overlay
function LoadImage() {
    var input = $('#DateSelectorInput').val();
    if (!StringIsEmpty(input)) {
        console.log(input)
    }

    GenerateNewImage()
    fetch(window.location.origin + "/Workzone/GetWorkzonePositions/" + $("#FloorSelectorSelect").val())
        .then(resp => resp.json())
        .then(data => {

            GenerateImagePoints(data)
            $('#LoadingVisualWorkspots').alert('close')
        })
}

function GenerateNewImage() {
    const image = document.querySelector('#FloorImage');
    image.src = "/images/Verdieping-" + $("#FloorSelectorSelect").val() + ".jpg"
    $("#ImageOverlay").html("")
}

let floorImages = null;

function GenerateImagePoints(data) {
    floorImages = data;
    const overlay = document.querySelector('.image-overlay');
    const image = document.querySelector('#FloorImage');

    data.forEach((point) => {
        let teamonly = $("#TeamCheckBox").prop('checked')
        if (point.ypos == "" || point.xpos == "") { return };
        if ((!teamonly && point.teamOnly == teamonly) || teamonly) {
            let scale = (image.height / 350)
            let maxMinScale = 1 - scale
            let properYPos = 1 - maxMinScale / 2
            let y = ((image.height * (point.ypos / 100)) * properYPos)
            let img = document.createElement('img');
            img.style.left = (point.xpos + "%");
            img.style.top = y + "px";
            img.title = point.name;
            img.id = "DataPoint" + point.id
            img.dataset.toggle = "modal"
            if (teamonly) img.dataset.target = "#WorkzoneTeamOnlySelectedModal"
            else img.dataset.target = "#WorkzoneDefaultSelectedModal"
            img.dataset.workzoneId = point.id
            img.dataset.workzoneName = point.name

            let percentage = (point.availableWorkspaces / point.workspaces) * 100
            let color = "green"
            if (percentage <= 50) color = "orange"
            if (percentage <= 0) color = "red"

            img.className = 'overlay-image ' + color;
            img.style.scale = scale;

            let imgSrc = "Single";
            if (point.teamOnly) imgSrc = "Group"
            else if (point.name.includes("ST")) imgSrc = "ST"

            img.src = "/images/Workspace" + imgSrc + ".svg"
            img.alt = point.name
            overlay.appendChild(img);
        }
    });
}

function LoadXYImage() {
    let xValue = $("#XPosistionInput").val()
    let yValue = $("#YPosistionInput").val()
    let name = $("#Name").val()
    let teamonly = $("#TeamOnly").prop("checked")
    const overlay = document.querySelector('.image-overlay');
    const image = document.querySelector('#FloorImage');

    let scale = (image.height / 350)
    let maxMinScale = 1 - scale
    let properYPos = 1 - maxMinScale / 2
    let y = ((image.height * (yValue / 100)) * properYPos)
    let img = document.createElement('img');
    img.style.left = (xValue + "%");
    img.style.top = y + "px";
    img.className = 'green overlay-image';
    img.style.scale = scale;

    let imgSrc = "Single";
    if (teamonly) imgSrc = "Group"
    else if (name.includes("ST")) imgSrc = "ST"

    img.src = "/images/Workspace" + imgSrc + ".svg"
    overlay.innerHTML = "";
    overlay.appendChild(img);
}

function SwitchTeamWorkzonesImages() {
    let teamonly = $("#TeamCheckBox").prop('checked')
    const overlay = document.querySelector('.image-overlay');
    const image = document.querySelector('#FloorImage');
    let data = floorImages;
    $("#ImageOverlay").html("")
    data.forEach((point) => {
        if (point.ypos == "" || point.xpos == "") { return };
        if ((!teamonly && point.teamOnly == teamonly) || teamonly) {
            let scale = (image.height / 350)
            let maxMinScale = 1 - scale
            let properYPos = 1 - maxMinScale / 2
            let y = ((image.height * (point.ypos / 100)) * properYPos)
            let img = document.createElement('img');

            let percentage = (point.availableWorkspaces / point.workspaces) * 100
            let color = "green"
            if (percentage <= 50) color = "orange"
            if (percentage <= 0) color = "red"
            if (point.teamOnly && percentage < 100) color = "red"
            if (teamonly) img.dataset.target = "#WorkzoneTeamOnlySelectedModal"
            else img.dataset.target = "#WorkzoneDefaultSelectedModal"

            img.style.left = (point.xpos + "%");
            img.style.top = y + "px";
            img.title = point.name;
            img.id = "DataPoint" + point.id
            img.dataset.toggle = "modal"
            img.dataset.workzoneId = point.id
            img.dataset.workzoneName = point.name
            img.className = 'overlay-image ' + color;
            img.style.scale = scale;

            let imgSrc = "Single";
            if (point.teamOnly) imgSrc = "Group"
            else if (point.name.includes("ST")) imgSrc = "ST"

            img.src = "/images/Workspace" + imgSrc + ".svg"
            img.alt = point.name
            overlay.appendChild(img);
        }
    });
}

function SearchForBhv() {
    let datetime = $("#BhvRegisterDateTimeInput").val()
    window.location.search = "?datetime=" + datetime
    console.log(window.location)
}