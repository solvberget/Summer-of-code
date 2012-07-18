(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var utils = WinJS.Utilities;
    var ui = WinJS.UI;

    var self;
    ui.Pages.define("/pages/mypage/mypage.html", {
        
        ready: function (element, options) {

            self = this;
            element.querySelector(".titlearea").addEventListener("click", this.showHeaderMenu, false);
            document.getElementById("headerMenuLists").addEventListener("click", function () { window.Data.itemByKey("lists").navigateTo(); }, false);
            document.getElementById("headerMenuEvents").addEventListener("click", function () { window.Data.itemByKey("events").navigateTo(); }, false);
            document.getElementById("headerMenuSearch").addEventListener("click", function () { window.Data.itemByKey("search").navigateTo(); }, false);
            document.getElementById("headerMenuHomeMenuItem").addEventListener("click", function () { self.goHome(); }, false);

            var theMenu = document.getElementById("HeaderMenu");
            WinJS.UI.processAll(theMenu);

        },
        

        showHeaderMenu: function () {

            var title = document.querySelector("header .titlearea");
            var menu = document.getElementById("HeaderMenu").winControl;
            menu.anchor = title;
            menu.placement = "bottom";
            menu.alignment = "left";

            menu.show();

        },

        goHome: function () {
            WinJS.Navigation.navigate("/pages/home/home.html");
            WinJS.log && WinJS.log("You are home.", "sample", "status");

        },
    });
    
})();
