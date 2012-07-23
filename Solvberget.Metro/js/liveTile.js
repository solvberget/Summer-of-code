(function () {
    "use strict";

    var notifications = Windows.UI.Notifications;
    
    var tileXml = notifications.TileUpdateManager.getTemplateContent(notifications.TileTemplateType.tileWideText03);

    setTimeout(function() {

        sendTileNotification();
    }, 1000);

    function sendTileNotification() {
        
        var tileTextAttributes = tileXml.getElementsByTagName("text");
        tileTextAttributes[0].appendChild(tileXml.createTextNode("Hello World! My very own tile notification"));

        var tileNotification = new notifications.TileNotification(tileXml);

        //var tileImageAttributes = tileXml.getElementsByTagName("image");
        //tileImageAttributes[0].setAttribute("src", "images\solvberget150.png");

        var currentTime = new Date();
        tileNotification.expirationTime = new Date(currentTime.getTime() + 600 * 100);

        notifications.TileUpdateManager.createTileUpdaterForApplication().update(tileNotification);


    };

})();