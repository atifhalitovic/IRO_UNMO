"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

//connection.on("ReceiveMessage", function (user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});

connection.on("updatestatus", function (obj) {
    console.log(obj);
    var ul = document.getElementById("notiContent");
    var li = document.createElement("li");
    li.appendChild(document.createTextNode(obj.statusOfApplication));
    ul.appendChild(li);
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

