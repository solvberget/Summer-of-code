(function () {

    var delayImageLoader = WinJS.Class.define(
            function (element, options) {
                this._element = element || document.createElement("div");
                this.element.winControl = this;
                WinJS.Utilities.addClass(this.element, "imageLoader");
                WinJS.Utilities.query("img", element).forEach(function (img) {
                    img.addEventListener("load", function () {
                        WinJS.Utilities.addClass(img, "loaded");
                    });
                });
            },
            {
                element: {
                    get: function () { return this._element; }
                },
            });

    WinJS.Namespace.define("Solvberget.Utils", {
        DelayImageLoader: delayImageLoader
    });
})();

(function () {

    var queueDownload = WinJS.Class.define(function (queue, options, completed, context, priority) {
        var maxNumInProgress = 5;
        if (!document.numInProgress) document.numInProgress = 0;
        if (!document.queue) document.queue = { q: {}, r: [] };

        if (!document.queue.q[queue]) document.queue.q[queue] = [];
        if (!document.queue.r) document.queue.r = [];

        if (options) {

            var callback = function (req, callBacks) {
                if (callBacks.completed) callBacks.completed(req, callBacks.context);
                document.numInProgress--;
                if (document.queue.q[queue].length > 0 && document.numInProgress < maxNumInProgress) {
                    for (var i = document.numInProgress; i <= maxNumInProgress && i < document.queue.q[queue].length; i++) {
                        var next = document.queue.q[queue].shift();
                        (function processNext(next) {
                            document.queue.r.push(WinJS.xhr(next.options));
                            document.queue.r[document.queue.r.length - 1].done(function (request, req) { callback(request, next) });
                        })(next);
                        document.numInProgress++;
                    }
                }
                if (document.queue.r) {
                    for (var i = 0; i < document.queue.r.length; i++) {
                        r = document.queue.r[i];
                        if (r._value)
                            document.queue.r.splice(i, 1);
                    }
                }
            }

            var saveValues = { options: options, completed: completed, context: context }
            if (priority)
                document.queue.q[queue].unshift(saveValues);
            else
                document.queue.q[queue].push(saveValues);
            if (document.queue.q[queue]. length >= 1 && document.numInProgress < maxNumInProgress) {
                document.numInProgress++;
                var next = document.queue.q[queue].shift();
                document.queue.r = WinJS.xhr(next.options).done(function (request) { callback(request, next) });
            }
        }
    });

    var cancelQueue = WinJS.Class.define(function (queue) {
        if (document.queue) {
            if (document.queue.r) {
                for (var i = 0; i < document.queue.r.length; i++) {
                    document.queue.r[i].cancel();
                }
            }
            if (document.queue.q)
                document.queue.q[queue] = [];
        }
        if (document.numInProgress)
            document.numInProgress = 0;

    });

    var prioritizeUrls = WinJS.Class.define(function (queue, urls) {
        if (!urls) return;
        if (!document.queue) return;
        var length = document.queue.q[queue].length;
        
        if (length <= document.numInProgress) return;

        for(var i = length -1; i > document.numInProgress; i--) {
            var q = document.queue.q[queue][i];
            if(urls.indexOf(q.options.url) != -1) {
                document.queue.q[queue].splice(i, 1);
                document.queue.q[queue].unshift(q);
            }
        }

    });

    WinJS.Namespace.define("Solvberget.Queue", {
        QueueDownload: queueDownload,
        CancelQueue: cancelQueue,
        PrioritizeUrls: prioritizeUrls
    });

})();

(function () {

    var styleNullToHiddenConverter = WinJS.Binding.converter(function (val) {
        var returnvalue;
        if (val != null) {
            returnvalue = val.toString() == [] ? "none" : "block";
            ;
        } else {
            returnvalue = "none";
        }
        return returnvalue;
    });
    WinJS.Namespace.define("Solvberget.Converters", {
        styleNullToHiddenConverter: styleNullToHiddenConverter
    });
})();

(function () {


    function getLoggedInBorrowerId() {


        var applicationData = Windows.Storage.ApplicationData.current;
        if (applicationData)
            var roamingSettings = applicationData.roamingSettings;

        var borrowerId = undefined;
        if (roamingSettings) {
            borrowerId = roamingSettings.values["BorrowerId"];
        }
        if (borrowerId == undefined || borrowerId == "")
            borrowerId = window.localStorage.getItem("BorrowerId");
        
        return borrowerId != undefined ? borrowerId : "";
    }

    function getLoggedInLibraryUserId() {

        var applicationData = Windows.Storage.ApplicationData.current;
        if (applicationData)
            var roamingSettings = applicationData.roamingSettings;

        var libraryUserId = undefined;
        if (roamingSettings) {
            libraryUserId = roamingSettings.values["LibraryUserId"];
        }
        
        if (libraryUserId == undefined || libraryUserId == "")
            libraryUserId = window.localStorage.getItem("LibraryUserId");

        return libraryUserId != undefined ? libraryUserId : "";
    }

    function getUserNotifications() {

        var applicationData = Windows.Storage.ApplicationData.current;
        if (applicationData)
            var roamingSettings = applicationData.roamingSettings;

        var notifications = new Array();

        if (roamingSettings) {
            notifications = roamingSettings.values["Notifications"];
        }

        if (notifications == undefined || notifications == "")
            notifications = window.localStorage.getItem("Notifications");

        return notifications != undefined ? notifications : "";
    }

    function setUserNotifications(notifications) {

        var applicationData = Windows.Storage.ApplicationData.current;
        if (applicationData)
            var roamingSettings = applicationData.roamingSettings;

        var notificationsAsString = "";

        for (i = 0; i < notifications.length; i++ ){
            notificationsAsString = notificationsAsString.concat(notifications[i].Title, ",", notifications[i].Content, ";");
        }

        window.localStorage.setItem("Notifications", notificationsAsString);

        if (roamingSettings)
            roamingSettings.values["Notifications"] = notificationsAsString;

    }

    function updateAppBarButton() {

        if (document.getElementById("cmdLoginFlyout")) {
            var user = LoginFlyout.getLoggedInBorrowerId();
            if (user && user != "") {
                document.getElementById("cmdLoginFlyout").winControl.label = "Logg ut";
                document.getElementById("cmdLoginFlyout").winControl.tooltip = "Logg ut";
            }
            else {
                document.getElementById("cmdLoginFlyout").winControl.label = "Logg inn";
                document.getElementById("cmdLoginFlyout").winControl.tooltip = "Logg inn";
            }
        }

    }
    function logout() {

        var activePage = WinJS.Navigation.location;

        window.localStorage.setItem("BorrowerId", "");
        window.localStorage.setItem("LibraryUserId", "");
        window.localStorage.setItem("Notifications", "");

        var applicationData = Windows.Storage.ApplicationData.current;
        var roamingSettings = applicationData.roamingSettings;

        roamingSettings.values["BorrowerId"] = "";
        roamingSettings.values["LibraryUserId"] = "";
        roamingSettings.values["Notifications"] = "";

        document.getElementById("logoutConfimationMsg").innerHTML = "Du blir nå logget ut";
               
        setTimeout(function () {
            var flyout = document.getElementById("loginFlyout");
            if (flyout != undefined)
                flyout.winControl.hide();
            if (WinJS.Navigation.location == "/pages/mypage/mypage.html")
                Data.navigateToHome();
            updateAppBarButton();
        }, 1200);

        LiveTile.liveTile();

        setTimeout(function () {        
            $("#logoutConfimationMsg").css("display", "none").css("visibility", "hidden");
            $("#confirmLogoutButton").css("display", "none").css("visibility", "hidden");
            $("#cancelLogoutButton").css("display", "none").css("visibility", "hidden");

            $("#labelForUserId").css("display", "block").css("visibility", "visible");
            $("#userId").css("display", "block").css("visibility", "visible");
            $("#laberForPin").css("display", "block").css("visibility", "visible");
            $("#pin").css("display", "block").css("visibility", "visible");
            $("#submitLoginButton").css("display", "block").css("visibility", "visible");
            $("#outputMsg").css("display", "block").css("visibility", "visible");
        }, 1300);
        
    }
    var interval;

    function liveTile() {      

        Windows.UI.Notifications.TileUpdateManager.createTileUpdaterForApplication().clear();

        Windows.UI.Notifications.TileUpdateManager.createTileUpdaterForApplication().enableNotificationQueue(true);

        var type = Windows.UI.Notifications.TileTemplateType.tileWideText09;

        var tileXml = Windows.UI.Notifications.TileUpdateManager.getTemplateContent(type);

        var notificationsAsString = Notifications.getUserNotifications();

        var notifications = notificationsAsString.split(";");

        

        var liveTileInterval = function () {
            title = notifications[number % (notifications.length - 1)].split(",")[0];
            content = notifications[number % (notifications.length - 1)].split(",")[1];

            tileNotification = new Windows.UI.Notifications.TileNotification(tileXml);

            tileImageAttributes[0].innerText = title + number;
            tileImageAttributes[1].innerText = content;

            Windows.UI.Notifications.TileUpdateManager.createTileUpdaterForApplication().update(tileNotification);

            number++;
        };

        if (notifications[0] != "") {
            var tileNotification = new Windows.UI.Notifications.TileNotification(tileXml);


            var scheduledTileNotifications = new Array();

            var title = "";
            var content = "";

            var number = 0;

            var tileImageAttributes = tileXml.getElementsByTagName("text");


            interval = setInterval(liveTileInterval, 4000);
        }
        else {
            if(interval)
                clearInterval(interval);
            Windows.UI.Notifications.TileUpdateManager.createTileUpdaterForApplication().clear();
        }
    };

    WinJS.Namespace.define("LoginFlyout", {
        getLoggedInBorrowerId: getLoggedInBorrowerId,
        getLoggedInLibraryUserId: getLoggedInLibraryUserId,
        updateAppBarButton: updateAppBarButton,
        logout: logout,
    });

    WinJS.Namespace.define("Notifications", {
        getUserNotifications: getUserNotifications,
        setUserNotifications: setUserNotifications,
    });

    WinJS.Namespace.define("LiveTile", {
        liveTile: liveTile,
    });

})();