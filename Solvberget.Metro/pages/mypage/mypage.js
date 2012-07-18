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

            getUserInformation();

            $(".box").draggable({ revert: "valid" });
            $("#mypageData").droppable();
            $(".box").droppable();


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

        },
    });

})();


var ajaxGetUserInformation = function () {
    var borrowerId = window.localStorage.getItem("BorrowerId");
    if (borrowerId != undefined && borrowerId !== "")
        return $.getJSON(window.Data.serverBaseUrl + "/User/GetUserInformation/" + borrowerId);
};

var getUserInformation = function () {

    // Show progress-ring, hide content
    $("#mypageData").css("display", "none").css("visibility", "none");
    $("#mypageLoading").css("display", "block").css("visibility", "visible");

    // Get the user information from server
    $.when(ajaxGetUserInformation())
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {
                // Extract fines from object
                var fines = response.Fines;
                // Delete fines from main object
                delete response.Fines;

                if (response.Name === response.PrefixAddress)
                    response.PrefixAddress = "";

                // Select HTML-section to process with the new binding lists
                var contentDiv = document.getElementById("mypageData");
                var finesDiv = document.getElementById("fines");
                var titleNameDiv = document.getElementById("pageSubtitleName");
                var balanceDiv = document.getElementById("balance");

                // avoid processing null (if user navigates to fast away from page etc)
                if (contentDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(contentDiv, response);
                if (finesDiv != undefined && fines != undefined)
                    WinJS.Binding.processAll(finesDiv, fines[0]);
                if (titleNameDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(titleNameDiv, response);
                if (balanceDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(balanceDiv, response);

            }

            // Hide progress-ring, show content
            $("#mypageLoading").css("display", "none").css("visibility", "none");
            $("#mypageData").css("display", "block").css("visibility", "visible").hide().fadeIn(500);

        }, this)
    );

};