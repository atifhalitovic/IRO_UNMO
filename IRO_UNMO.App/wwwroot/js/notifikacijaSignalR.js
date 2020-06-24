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
    var o = "";
    if (!obj.seen) {
        o = "style='background-color:#d6e6ff'";
    }
    var element1 = 
        '<a onClick="deselectNotification(' + obj.notificationId + ')" class="item" href="' + obj.url + '">'+
        '<div class="content">' + '<a class="header">' + obj.user  + '</a>' + 
        '<span class="time"> ' +  obj.time + '</span>' + 
        '<div class="description">' + obj.message + '</div>' + 
        '<div class="extra">Thanks you for your support</div></div></div></a>';

    //var element = '<a ' + o + ' onClick="deselectNotifikaciju(' + obj.notificationId + ')" class="item" href="' + obj.url
    //    + '">' +
    //    ' <div class="font-weight-bold">' +
    //    '<div class="text-truncate">' + obj.message + '</div>' +
    //    '<div class="small text-gray-500">' + obj.user + ' · ' + obj.time + '</div>' +
    //    '</div></a>';

    return element1;
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

