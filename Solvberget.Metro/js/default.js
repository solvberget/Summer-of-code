// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var nav = WinJS.Navigation;
    WinJS.strictProcessing();

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }

            if (app.sessionState.history) {
                nav.history = app.sessionState.history;
            }
            args.setPromise(WinJS.UI.processAll().then(function () {
                if (nav.location) {
                    nav.history.current.initialPlaceholder = true;
                    return nav.navigate(nav.location, nav.state);
                } else {
                    return nav.navigate(Application.navigator.home);
                }
            }));

            setAppbarButton()

            document.getElementById("cmdLoginFlyout").addEventListener("click", doLogin);
            document.getElementById("cmdPin").addEventListener("click", pinToStart);

        }
    };

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

    window.localStorage.setItem("BorrowerId", "");

    var loginDiv = document.getElementById("loginFragmentHolder");
    loginDiv.innerHTML = "";
    WinJS.UI.Fragments.renderCopy("/fragments/login/login.html", loginDiv).done(function () {

        var loginAnchor = document.querySelector("div"); 

        LoginFlyout.showLogin(loginAnchor);

    });

}

function pinByElementAsync(element, newTileID, newTileShortName, newTileDisplayName) {

    // Sometimes it's convenient to create our tile and pin it, all in a single asynchronous call.

    // We're pinning a secondary tile, so let's build up the tile just like we did in pinByElement
    var uriLogo = new Windows.Foundation.Uri("ms-appx:///images/solvberget150.png");
    var uriSmallLogo = new Windows.Foundation.Uri("ms-appx:///images/solvberget30.png");
    var currentTime = new Date();
    var TileActivationArguments = "timeTileWasPinned=" + currentTime;
    var tile = new Windows.UI.StartScreen.SecondaryTile(newTileID, newTileShortName, newTileDisplayName, TileActivationArguments, Windows.UI.StartScreen.TileOptions.showNameOnLogo, uriLogo);
    tile.foregroundText = Windows.UI.StartScreen.ForegroundText.light;
    tile.smallLogo = uriSmallLogo;


    // Let's place the focus rectangle near the button, just like we did in pinByElement
    var selectionRect = element.getBoundingClientRect();

    // Now let's try to pin the tile.
    // We'll make the same fundamental call as we did in pinByElement, but this time we'll return a promise.
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

    // Sometimes it's convenient to get ready to delete our tile and then try to unpin it, all in a single asynchronous call.

    // element is the html element on the page where the unpin request was initiated.
    // We want to grab the coordinates of that element in order to properly position the confirmation flyout.
    // We'll use the bounding client rectangle of the element to get those coordinates.
    var selectionRect = element.getBoundingClientRect();

    // In this example, we're going to delete a tile named SecondaryTile.01. In order to tell the Unpin
    // API which tile we want to delete, we need to construct a SecondaryTile with that name.
    var tileToGetDeleted = new Windows.UI.StartScreen.SecondaryTile(unwantedTileID);

    // Now we make the delete request, passing a point built from the bounding client rectangle.
    // Note that this is an async call, and we'll return a promise so we can do additional work when it's complete.

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

    // Check whether you've received a pin or unpin command.
    if (WinJS.UI.AppBarIcon.unpin === document.getElementById("cmdPin").winControl.icon) {
        // unpinByElementAsync is an asynchronous, app-defined, helper function used 
        // in the Secondary Tiles sample.
        unpinByElementAsync(document.getElementById("cmdPin"), "SecondaryTile.appBarPinnedTile", "Appbar pinned secondary tile", "A secondary tile that was pinned by the user from the Appbar").then(function (isDeleted) {
            if (isDeleted) {
                // The unpin operation was successful. The app bar should show the pin glyph.
                setAppbarButton();
                // Do any other necessary cleanup.
            } else {
                // The unpin operation was either canceled or failed.
            }
        });
         
    } else {
        // pinByElementAsync is an asynchronous, app-defined, helper function used
        // in the Secondary Tiles sample.
        pinByElementAsync(document.getElementById("cmdPin"), "SecondaryTile.appBarPinnedTile", "Appbar pinned secondary tile", "A secondary tile that was pinned by the user from the Appbar").then(function (isCreated) {
            if (isCreated) {
                // The pin operation was successful. The app bar should show the unpin glyph.
                setAppbarButton();
                // Send notifications and do other necessary work.
            } else {
                // The pin operation was either canceled or failed.
            }
        });
    }
}


function setAppbarButton() {

    if (Windows.UI.StartScreen.SecondaryTile.exists("SecondaryTile.appBarPinnedTile")) {
        document.getElementById("cmdPin").winControl.label = "Fjern fra start";
        document.getElementById("cmdPin").winControl.icon = "unpin";
        document.getElementById("cmdPin").winControl.tooltip = "Fjern fra start";
    } else {
        document.getElementById("cmdPin").winControl.label = "Pin til start";
        document.getElementById("cmdPin").winControl.icon = "pin";
        document.getElementById("cmdPin").winControl.tooltip = "Pin til start";
    }

    document.getElementById("appBar").winControl.sticky = false;
}
