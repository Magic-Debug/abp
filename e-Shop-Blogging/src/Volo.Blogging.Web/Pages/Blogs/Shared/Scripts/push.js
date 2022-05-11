$(function() {
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalr-hubs/blogging").build();

    connection.on("ReceiveMessage", function (message) {
        $('#MessageList').append('<li><strong><i class="fas fa-long-arrow-alt-right"></i> ' + message + '</strong></li>');
    });

    connection.start().then(function (e) {
        console.info("connection");

    }).catch(function (err) {
        return console.error(err.toString());
    });

    $(document).ready(function (e) {
        var targetUserName = "lucy"; //$('#TargetUser').val();
        var message = "hello"; //$('#Message').val();
        $('#Message').val('');

        connection.invoke("SendMessage", targetUserName, message)
            .then(function () {
                console.log('<li><i class="fas fa-long-arrow-alt-left"></i> ' + abp.currentUser.userName + ': ' + message + '</li>');
            })
            .catch(function(err) {
                return console.error(err.toString());
            });
    });
});