(function () {
    "use strict";

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var nav = WinJS.Navigation;
    WinJS.strictProcessing();

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {

            var applicationData = Windows.Storage.ApplicationData.current;
            applicationData.addEventListener("datachanged", roamingDataChangeHandler);

            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }

            if (args.detail.arguments !== "") {

                if (app.sessionState.history) {
                    nav.history = app.sessionState.history;
                } else {
                    nav.history.backStack.push({ location: "/pages/home/home.html" });
                }

                args.setPromise(WinJS.UI.processAll().done(function () {
                    nav.navigate(args.detail.arguments);
                }));

            } else {

                if (app.sessionState.history) {
                    nav.history = app.sessionState.history;
                }
                args.setPromise(WinJS.UI.processAll().done(function () {
                    if (nav.location) {
                        nav.history.current.initialPlaceholder = true;
                        nav.navigate(nav.location, nav.state);
                    } else {
                        nav.navigate(Application.navigator.home);
                    }
                }));
            }
            setAppbarButton();

            //Add functionality to the appbar buttons
            document.getElementById("cmdLoginFlyout").addEventListener("click", doLogin);
            document.getElementById("cmdPin").addEventListener("click", pinToStart);
            document.getElementById("toNewsButton").addEventListener("click", Data.navigateToNews);
            document.getElementById("toListsButton").addEventListener("click", Data.navigateToLists);
            document.getElementById("toMyPageButton").addEventListener("click", Data.navigateToMypage);
            document.getElementById("toBlogsPageButton").addEventListener("click", Data.navigateToBlogs);
            document.getElementById("toEventsButton").addEventListener("click", Data.navigateToEvents);
            document.getElementById("toOpeningHoursButton").addEventListener("click", Data.navigateToOpeningHours);
            document.getElementById("toContactButton").addEventListener("click", Data.navigateToContact);
            document.getElementById("toSearchButton").addEventListener("click", Data.navigateToSearch);
            document.getElementById("toHomeButton").addEventListener("click", Data.navigateToHome);
        }
    };

    function roamingDataChangeHandler(eventArgs) {
        // TODO: Refresh your data
    }

    app.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. You might use the
        // WinJS.Application.sessionState object, which is automatically
        // saved and restored across suspension. If you need to complete an
        // asynchronous operation before your application is suspended, call
        // args.setPromise().
        app.sessionState.history = nav.history;
    };

    app.start();
})();

function doLogin() {

    // Get active user
    var user = LoginFlyout.getLoggedInBorrowerId();
    LoginFlyout.logout();

    // If user was not logging out, user was logging in, so show login
    if (!user || user == "") {

        // TODO: ROAMING
        var loginDiv = document.getElementById("loginFragmentHolder");
        loginDiv.innerHTML = "";
        WinJS.UI.Fragments.renderCopy("/fragments/login/login.html", loginDiv).done(function () {

            var loginAnchor = document.querySelector("div");

            LoginFlyout.showLogin(loginAnchor);
        });
    }
}

function pinByElementAsync(element, newTileID, newTileShortName, newTileDisplayName) {

    var uriLogo = new Windows.Foundation.Uri("ms-appx:///images/solvberget150.png");
    var uriSmallLogo = new Windows.Foundation.Uri("ms-appx:///images/solvberget30.png");
    var currentTime = new Date();
    var TileActivationArguments = WinJS.Navigation.location;
    var tile = new Windows.UI.StartScreen.SecondaryTile(newTileID, newTileShortName, newTileDisplayName, TileActivationArguments, Windows.UI.StartScreen.TileOptions.showNameOnLogo, uriLogo);
    tile.foregroundText = Windows.UI.StartScreen.ForegroundText.light;
    tile.smallLogo = uriSmallLogo;

    var selectionRect = element.getBoundingClientRect();

    return new WinJS.Promise(function (complete, error, progress) {
        tile.requestCreateAsync({ x: selectionRect.left, y: selectionRect.top }).done(function (isCreated) {
            if (isCreated) {
                complete(true);
            } else {
                complete(false);
            }
        });
    });
}

function unpinByElementAsync(element, unwantedTileID) {

    var selectionRect = element.getBoundingClientRect();

    var tileToGetDeleted = new Windows.UI.StartScreen.SecondaryTile(unwantedTileID);

    return new WinJS.Promise(function (complete, error, progress) {
        tileToGetDeleted.requestDeleteForSelectionAsync({ height: selectionRect.height, width: selectionRect.width, x: selectionRect.left, y: selectionRect.top }).done(function (isDeleted) {
            if (isDeleted) {
                complete(true);
            } else {
                complete(false);
            }
        });
    });
}

function pinToStart() {
    document.getElementById("appBar").winControl.sticky = true;

    if (WinJS.UI.AppBarIcon.unpin === document.getElementById("cmdPin").winControl.icon) {
        unpinByElementAsync(document.getElementById("cmdPin"), Data.activePage).then(function (isDeleted) {
            if (isDeleted) {
                setAppbarButton();
            } else {
            }
        });

    } else {

        pinByElementAsync(document.getElementById("cmdPin"), Data.activePage, "Appbar pinned secondary tile", "A secondary tile that was pinned by the user from the Appbar").then(function (isCreated) {
            if (isCreated) {
                setAppbarButton();
            } else {
            }
        });
    }
}

function setAppbarButton() {

    LoginFlyout.updateAppBarButton();

    if (Windows.UI.StartScreen.SecondaryTile.exists(Data.activePage)) {
        document.getElementById("cmdPin").winControl.label = "Fjern fra start";
        document.getElementById("cmdPin").winControl.icon = "unpin";
        document.getElementById("cmdPin").winControl.tooltip = "Fjern fra start";
    } else {
        document.getElementById("cmdPin").winControl.label = "Pin til start";
        document.getElementById("cmdPin").winControl.icon = "pin";
        document.getElementById("cmdPin").winControl.tooltip = "Pin til start";
    }
}
