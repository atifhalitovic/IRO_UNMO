"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/signalServer").build();

connection.on("GetNotification", function (obj) {
    var x = generateNotification(obj);
    $("#notifikacije").prepend(x);
    incrementNotificationNumber();
});

connection.on("getAll", function (obj) {
    obj.forEach(element => {
        var x = generateNotification(element);
        $("#notifikacije").prepend(x);
        if (!element.seen) {
        incrementNotificationNumber();
        }
    });
});

function incrementNotificationNumber() {
    var x = $("#counter").text();
    if (x != "") {
        var br = parseInt(x);
        var y = br + 1;
        $("#counter").text(y);
        return;
    }
    $("#counter").text("1");

}

function generateNotification(obj) {
    //var o = "";
    //if (!obj.seen) {
    //    o = "style='background-color:#d6e6ff'";
    //}
    var element1 =
        '<div onClick="deselectNotification(' + obj.notificationId + ')" class="item" href="' + obj.url + '">' +
        '<div class="content">' + '<a class="header">' + obj.user + '</a>' +
        '<span class="date"> ' + obj.time + '</span>' +
        '<div class="description">' + obj.message + '</div></div><div class="ui divider"></div></div>';

    var codeBlock = '<div class="content">' +
        '<h1>This is a heading</h1>' +
        '<p>This is a paragraph of text.</p>' +
        '<p><strong>Note:</strong> If you don\'t escape "quotes" properly, it will not work.</p>' +
        '</div>';

    return codeBlock;
}
function deselectNotification(id) {
    connection
        .invoke('deselectNotification', id);
}
connection.start().then(function () {
    console.log("connected");
    connection
        .invoke('getNotification', 20);
}).catch(function (err) {
    return console.error(err.toString());
});

