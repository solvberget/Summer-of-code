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

    var queueDownload = WinJS.Class.define(function (queue, options, completed, context, priority, error) {
        /** NOTICE! maxNumInProgress
        *
        *   IE limits number of concurrent connections to 6. By setting it to 5 there will always be one connection
        *    ready to serve image loads through <img src> and similare ajax calls which we do not always think about. 
        *
        *    Setting maxNumInProgress to 6 will clog up the number of concurrent connections resulting in image loading
        *    not bein executed before our custom ajax calls are complete. 
        */
        var maxNumInProgress = 5;

        function isConnectedToInternet() {
            var profile = Windows.Networking.Connectivity.NetworkInformation.getInternetConnectionProfile();
            if (profile) {
                return (profile.getNetworkConnectivityLevel() != Windows.Networking.Connectivity.NetworkConnectivityLevel.none);
            }
            else {
                return false;
            }
        }



        function processQueue() {
            if (isConnectedToInternet()) {
                document.numInProgress++;
                var next = document.queue.q[queue].shift();
                document.queue.r.push(WinJS.xhr(next.options));
                document.queue.r[document.queue.r.length - 1].done(function (request) {
                    callback(request, next, true);
                }, function (request) {
                    callback(request, next, false);
                })
                ;
            }
            else {
                showNoInternetDialog();
            }
        };



        function showNoInternetDialog() {

            setTimeout(function () {

                // Create the message dialog and set its content
                var internetDialog = new Windows.UI.Popups.MessageDialog(
                    "Ingen internettforbindelse ble funnet.\n\n" +
                        "Denne applikasjonen trenger tilgang til internett for å fungere korrekt.", "Ooops!");

                // Add commands and set their command handlers
                internetDialog.commands.append(new Windows.UI.Popups.UICommand(
                    "Prøv igjen",
                    commandInvokedHandler, 0));

                // Set the command that will be invoked by default
                internetDialog.defaultCommandIndex = 0;

                try {
                    // Show the message dialog
                    internetDialog.showAsync();
                } catch (exception) {
                    // No access exception
                    console.log(new Date().toString() + ": No access exception(cant display dialog)");
                }
            }, 500);

        }



        function commandInvokedHandler(command) {
            if (command.id == 0) {
                // Try again
                processQueue();
            }
        }



        if (!document.numInProgress) document.numInProgress = 0;
        if (!document.queue) document.queue = { q: {}, r: [] };

        if (!document.queue.q[queue]) document.queue.q[queue] = [];
        if (!document.queue.r) document.queue.r = [];

        if (options) {

            var callback = function (req, callBacks, success) {
                if (success) {
                    if (callBacks.completed) { callBacks.completed(req, callBacks.context); }
                }
                else {
                    if (callBacks.error) { callBacks.error(req, callBacks.context); }
                }

                    document.numInProgress--;
                if (document.queue.q[queue].length > 0 && document.numInProgress < maxNumInProgress) {
                    for (var i = document.numInProgress; i <= maxNumInProgress && i < document.queue.q[queue].length; i++) {
                        processQueue();
                    }
                }
                if (document.queue.r) {
                    for (var j = 0; j < document.queue.r.length; j++) {
                        r = document.queue.r[j];
                        if (r._value)
                            document.queue.r.splice(j, 1);
                    }
                }
            };

            var saveValues = { options: options, completed: completed, context: context };
            if (priority)
                document.queue.q[queue].unshift(saveValues);
            else
                document.queue.q[queue].push(saveValues);
            if (document.queue.q[queue].length >= 1 && document.numInProgress < maxNumInProgress) {
                processQueue();
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
            if (document.queue.q && queue) {
                document.queue.q[queue] = [];
            }
            else {
                document.queue = { q: {}, r: [] };
            }
        }
        if (document.numInProgress)
            document.numInProgress = 0;

    });

    var prioritizeUrls = WinJS.Class.define(function (queue, urls) {
        if (!urls) return;
        if (!document.queue) return;
        var length = document.queue.q[queue].length;

        if (length <= document.numInProgress) return;

        for (var i = length - 1; i > document.numInProgress; i--) {
            var q = document.queue.q[queue][i];
            if (urls.indexOf(q.options.url) != -1) {
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

    var nullConverter = WinJS.Binding.converter(function (value) {
        return value == undefined ? "" : value;
    });

    var emailToMailtoConverter = WinJS.Binding.converter(function (value) {
        return value == undefined ? "" : "mailto:" + value;
    });

    WinJS.Namespace.define("Solvberget.Converters", {
        styleNullToHiddenConverter: styleNullToHiddenConverter,
        nullConverter: nullConverter,
        emailToMailtoConverter: emailToMailtoConverter
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

        for (var i = 0; i < notifications.length; i++) {
            notificationsAsString = notificationsAsString.concat(notifications[i].Title, ",", notifications[i].Content, ";");
        }

        window.localStorage.setItem("Notifications", notificationsAsString);

        if (roamingSettings)
            roamingSettings.values["Notifications"] = notificationsAsString;

    }

    function setAreNotificationsSeen(isSeen) {
        var applicationData = Windows.Storage.ApplicationData.current;
        if (applicationData)
            var roamingSettings = applicationData.roamingSettings;

        if (isSeen) {

            window.localStorage.setItem("NotificationsSeen", "true");

            if (roamingSettings)
                roamingSettings.values["NotificationsSeen"] = "true";

        }
        else {
            window.localStorage.setItem("NotificationsSeen", "false");

            if (roamingSettings)
                roamingSettings.values["NotificationsSeen"] = "false";
        }

    }

    function areNotificationsSeen() {
        var applicationData = Windows.Storage.ApplicationData.current;
        if (applicationData)
            var roamingSettings = applicationData.roamingSettings;

        var notifications = new Array();

        if (roamingSettings) {
            notifications = roamingSettings.values["NotificationsSeen"];
        }

        if (notifications == undefined || notifications == "")
            notifications = window.localStorage.getItem("NotificationsSeen");

        if (notifications == "true") return true;

        return false;
    }

    var showToast = function (heading, body) {

        var template = Windows.UI.Notifications.ToastTemplateType.toastText02;

        var toastXml = Windows.UI.Notifications.ToastNotificationManager.getTemplateContent(template);

        var toastTextElements = toastXml.getElementsByTagName("text");
        toastTextElements[0].appendChild(toastXml.createTextNode(heading));
        toastTextElements[1].appendChild(toastXml.createTextNode(body));

        var toast = new Windows.UI.Notifications.ToastNotification(toastXml);

        var toastNotifier = Windows.UI.Notifications.ToastNotificationManager.createToastNotifier();
        toastNotifier.show(toast);

        Notifications.setAreNotificationsSeen(true);
    };

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
        if (applicationData) {
            var roamingSettings = applicationData.roamingSettings;
            if (roamingSettings) {
                roamingSettings.values["BorrowerId"] = "";
                roamingSettings.values["LibraryUserId"] = "";
                roamingSettings.values["Notifications"] = "";
            }
        }
        
        Notifications.setAreNotificationsSeen(false);

        document.getElementById("logoutConfimationMsg").innerHTML = "Du blir nå logget ut";

        setTimeout(function () {
            var flyout = document.getElementById("loginFlyout");
            if (flyout != undefined)
                flyout.winControl.hide();
            if (WinJS.Navigation.location == "/pages/mypage/mypage.html")
                Data.navigateToHome();
            updateAppBarButton();
        }, 1200);

        if (interval)
            clearInterval(interval);
        Windows.UI.Notifications.TileUpdateManager.createTileUpdaterForApplication().clear();

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

            tileImageAttributes[0].innerText = title;
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


            interval = setInterval(liveTileInterval, 5000);
        }
        else {
            if (interval)
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
        setAreNotificationsSeen: setAreNotificationsSeen,
        areNotificationsSeen: areNotificationsSeen,
    });

    WinJS.Namespace.define("LiveTile", {
        liveTile: liveTile,
        interval: interval
    });

    WinJS.Namespace.define("Toast", {
        showToast: showToast,
    });

})();