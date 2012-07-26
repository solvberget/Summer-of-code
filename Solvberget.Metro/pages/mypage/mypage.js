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
    var borrowerId = LoginFlyout.getLoggedInBorrowerId();
    if (borrowerId != undefined && borrowerId !== "")
        return $.getJSON(window.Data.serverBaseUrl + "/User/GetUserInformation/" + borrowerId);
};



var cancelReservation = function (reservation, element) {

    var cancelReservationDiv = document.getElementById("requestReservationFragmentHolder");
    cancelReservationDiv.innerHTML = "";

    WinJS.UI.Fragments.renderCopy("/fragments/cancelReservation/cancelReservation.html", cancelReservationDiv).done(function () {

        var reservationAnchor = document.getElementById("reservationTemplateHolder");

        CancelReservation.showFlyout(reservationAnchor, reservation, element);
    });


}


var renewLoan = function (loan) {

    var renewalDiv = document.getElementById("loanRenewalFragmentHolder");
    renewalDiv.innerHTML = "";

    WinJS.UI.Fragments.renderCopy("/fragments/loanRenewal/loanRenewal.html", renewalDiv).done(function () {

        var renewalAnchor = document.getElementById("loanTemplateHolder");

        LoanRenewal.showFlyout(renewalAnchor, null, loan);
    });

    
}

var addFinesToDom = function (fines) {

    if (fines == undefined)
        return;

    var fineTemplate = new WinJS.Binding.Template(document.getElementById("fineTemplate"));
    var fineTemplateContainer = document.getElementById("fineTemplateHolder");

    fineTemplateContainer.innerHTML = "";

    var i, fine;
    for (i = 0; i < fines.length; i++) {
        fine = fines[i];
        fineTemplate.render(fine, fineTemplateContainer);
    }
}

var addLoansToDom = function (loans) {

    if (loans == undefined) 
        return;
    
    var loanTemplate = new WinJS.Binding.Template(document.getElementById("loanTemplate"));
    var loansTemplateContainer = document.getElementById("loanTemplateHolder");

    loansTemplateContainer.innerHTML = "";

    var i, loan;
    for (i = 0; i < loans.length; i++) {
        loan = loans[i];

        loanTemplate.render(loan, loansTemplateContainer).done(function (element) {
            $(element).find(".renewLoanButton:last").attr("index", i);
            $(element).find(".renewLoanButton:last").click(function () {
                var index = $(this).attr("index");

                renewLoan(loans[index]);
            });
        });
    }
}

var addReservationsToDom = function (reservations) {

    if (reservations == undefined)
        return;

    var reservationTemplate = new WinJS.Binding.Template(document.getElementById("reservationTemplate"));
    var reservationsTemplateContainer = document.getElementById("reservationTemplateHolder");

    reservationsTemplateContainer.innerHTML = "";

    var i, reservation;
    for (i = 0; i < reservations.length; i++) {
        reservation = reservations[i];
        reservationTemplate.render(reservation, reservationsTemplateContainer).done(function (element) {
            $(element).find(".cancelReservationButton:last").attr("index", i);
            $(element).find(".cancelReservationButton:last").click(function () {
                var index = $(this).attr("index");
                cancelReservation(reservations[index], element);
            });
        });
    }
}

var getUserInformation = function () {

    // Show progress-ring, hide content
    $("#mypageData").css("display", "none").css("visibility", "none");
    $("#mypageLoading").css("display", "block").css("visibility", "visible");

    // Prevent caching of this request
    $.ajaxSetup({ cache: false });

    // Get the user information from server
    $.when(ajaxGetUserInformation())
        .then($.proxy(function (response) {
            if (response != undefined && response !== "") {
                // Extract fines from object
                var fines = response.ActiveFines;
                // Delete fines from main object
                delete response.ActiveFines;

                // Extract loans from object
                var loans = response.Loans;
                // Delete loans from main object
                delete response.Loans;

                // Extract reservations from object
                var reservations = response.Reservations;
                // Delete reservations from main object
                delete response.Reservations;

                if (response.Name === response.PrefixAddress)
                    response.PrefixAddress = "";

                // Select HTML-section to process with the new binding lists
                var contentDiv = document.getElementById("myPagePersonalInformation");
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
                this.addLoansToDom(loans);
                this.addReservationsToDom(reservations);


            }

            // Hide progress-ring, show content
            $("#mypageLoading").css("display", "none").css("visibility", "none");
            $("#mypageData").css("display", "block").css("visibility", "visible").hide().fadeIn(500);

        }, this)
    );

    WinJS.Namespace.define("MyPageConverters", {

        balanceConverter: WinJS.Binding.converter(function (balance) {
            if (balance == undefined) return "Du har ingen gebyrer!";
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
        }),
        loanMaterialConverter: WinJS.Binding.converter(function (loanMaterial) {
            if (loanMaterial == undefined) return "";
            return loanMaterial == "" ? "" : "Type: " + loanMaterial;
        }),
        loanCatalogerNameConverter: WinJS.Binding.converter(function (loanCataloger) {
            if (loanCataloger == undefined) return "";
            return loanCataloger == "" ? "" : "Utlåner: " + loanCataloger;
        }),
        loanDateConverter: WinJS.Binding.converter(function (loanDate) {
            if (loanDate == undefined) return "";
            return loanDate == "" ? "" : "Lånt: " + loanDate;
        }),
        dueDateConverter: WinJS.Binding.converter(function (dueDate) {
            if (dueDate == undefined) return "";
            return dueDate == "" ? "" : "Frist for innlevering: " + dueDate;
        }),
        originalDueDateConverter: WinJS.Binding.converter(function (originalDueDate) {
            if (originalDueDate == undefined) return "";
            return originalDueDate == "" ? "" : "Opprinnelig lånefrist: " + originalDueDate;
        }),
        holdRequestFromConverter: WinJS.Binding.converter(function (holdRequestFrom) {
            if (holdRequestFrom == undefined) return "";
            return holdRequestFrom == "" ? "" : "Reservert fra: " + holdRequestFrom;
        }),
        holdRequestToConverter: WinJS.Binding.converter(function (holdRequestTo) {
            if (holdRequestTo == undefined) return "";
            return holdRequestTo == "" ? "" : "Reservert til: " + holdRequestTo;
        }),
        pickupLibraryConverter: WinJS.Binding.converter(function (pickupLibrary) {
            if (pickupLibrary == undefined) return "";
            return pickupLibrary == "" ? "" : "Hentes hos: " + pickupLibrary;
        }),

    });






};