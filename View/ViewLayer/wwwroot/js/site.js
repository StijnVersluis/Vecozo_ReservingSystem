$(document).ready(function () {
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
});

$(document).ready(function () {
    var date = new Date();

    var currentDateFormat = date.toISOString().substring(0, 10) + " " + date.toLocaleString().substring(12, 17);
    $("#DateSelectorInput").val(currentDateFormat)
});

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
    loadWorkzones(new Date(Date.parse(event.target.value)).toLocaleString());
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
    console.log(html);
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

$('#WorkzoneSelectedModal').on('shown.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var workzoneId = button.data('workzone-id') // Extract info from data-teamid attributes
    var workzoneName = button.data('workzone-name') // Extract info from data-teamid attributes
    let datetimeArrive = $("#DateSelectorInput").val()
    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.

    var modal = $(this)
    modal.find('.modal-title').text('Werkblok: ' + workzoneName)
    modal.find('#DateAndTimeReservation').text($("#DateSelectorInput").val().replace(/T/g, " "))
    modal.find('#WorkzoneArrivingForm').val(datetimeArrive)
    modal.find('#WorkzoneIdForm').val(workzoneId)

    let time = datetimeArrive.replace(/([0-9]{1,4}-[0-9]{1,2}-[0-9]{1,2})T/g, "")
    modal.find('#WorkzoneLeavingForm').attr({
        "min": time
    })
})

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

function loadWorkzones(date) {
    if (date == null) {
        date = new Date().toLocaleString();
    }

    let floorId = $('#FloorSelectorSelect').val();

    fetch(window.location.origin + `/Workzone/Floor/${floorId}?date=${date}`, {
        method: "GET"
    })
        .then(resp => resp.text())
        .then(data => {
            $("#WorkSpotSelectList").html(data)
        })
        .catch(err => {
            console.log(err);
        })
}


//Image overlay
function LoadImage() {
    GenerateNewImage()
    var something = fetch(window.location.origin + "/Workzone/GetWorkzonePositions/" + $("#FloorSelectorSelect").val())
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

function GenerateImagePoints(data) {
    console.log(data)
    const overlay = document.querySelector('.image-overlay'); 
    const image = document.querySelector('#FloorImage');

    data.forEach((point) => {
        if (point.ypos == "" || point.xpos == "") { return };
        let scale = (image.height / 300)
        let maxMinScale = 1 - scale
        let properYPos = 1 - maxMinScale / 2
        let y = ((image.height * (point.ypos / 100)) * properYPos)
        let img = document.createElement('img');
        img.style.left = (point.xpos + "%");
        img.style.top = y + "px";
        img.title = point.name;
        img.id = "DataPoint" + point.id
        img.dataset.toggle = "modal"
        img.dataset.target = "#WorkzoneSelectedModal"
        img.dataset.workzoneId = point.id
        img.dataset.workzoneName = point.name
        img.className = 'overlay-image';
        img.style.scale = scale;
        img.src = "/images/Workspace.svg"
        img.alt = point.name
        overlay.appendChild(img);
    });
}

function LoadXYImage() {
    let xValue = $("#XPosistionInput").val()
    let yValue = $("#YPosistionInput").val()
    const overlay = document.querySelector('.image-overlay');
    const image = document.querySelector('#FloorImage');

    let scale = (image.height / 300)
    let maxMinScale = 1 - scale
    let properYPos = 1 - maxMinScale / 2
    let y = ((image.height * (yValue / 100)) * properYPos)
    let img = document.createElement('img');
    img.style.left = (xValue + "%");
    img.style.top = y + "px";
    img.className = 'overlay-image';
    img.style.scale = scale;
    img.src = "/images/Workspace.svg"
    overlay.innerHTML = "";
    overlay.appendChild(img);
}