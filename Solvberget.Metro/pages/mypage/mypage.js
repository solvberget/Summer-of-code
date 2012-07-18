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

            $(".box").draggable({ revert: "valid", containment: "body" });
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

var addFinesToDom = function (fines) {

    var fineTemplate = new WinJS.Binding.Template(document.getElementById("fineTemplate"));
    var fineTemplateContainer = document.getElementById("fineTemplateHolder");

    fineTemplateContainer.innerHTML = "";

    var i, fine;
    for (i = 0; i < fines.length; i++) {
        fine = fines[i];
        fineTemplate.render(fine, fineTemplateContainer);
    }

}

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
                var titleNameDiv = document.getElementById("pageSubtitleName");
                var balanceDiv = document.getElementById("balance");


                // avoid processing null (if user navigates to fast away from page etc)
                if (contentDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(contentDiv, response);

                if (titleNameDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(titleNameDiv, response);

                if (balanceDiv != undefined && response != undefined)
                    WinJS.Binding.processAll(balanceDiv, response);

                this.addFinesToDom(fines);

            }

            // Hide progress-ring, show content
            $("#mypageLoading").css("display", "none").css("visibility", "none");
            $("#mypageData").css("display", "block").css("visibility", "visible").hide().fadeIn(500);

        }, this)
    );

    WinJS.Namespace.define("MyPageConverters", {

        balanceConverter: WinJS.Binding.converter(function (balance) {
            if (balance == undefined) return "";

            return balance == "" ? "" : "Balanse: " + balance + ",-";
        }),
        sumConverter: WinJS.Binding.converter(function (sum) {
            if (sum == undefined) return "";
            return sum == "" ? "" : "Gebyr: " + sum + ",-";
        }),
        statusConverter: WinJS.Binding.converter(function (status) {
            if (status == undefined) return "";
            return status == "" ? "" : "Status: " + status;
        }),
        dateConverter: WinJS.Binding.converter(function (date) {
            if (date == undefined) return "";
            return date == "" ? "" : "Dato: " + date;
        })

    });




};