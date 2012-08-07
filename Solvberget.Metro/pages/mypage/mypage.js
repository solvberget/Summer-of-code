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

            getUserInformation();

            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());


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



var cancelReservation = function (reservations, index, element) {

    var cancelReservationDiv = document.getElementById("requestReservationFragmentHolder");
    cancelReservationDiv.innerHTML = "";

    WinJS.UI.Fragments.renderCopy("/fragments/cancelReservation/cancelReservation.html", cancelReservationDiv).done(function () {

        var reservationAnchor = document.getElementById("reservationTemplateHolder");

        CancelReservation.showFlyout(reservationAnchor, reservations, index, element);
    });


};

var renewLoan = function (loan) {

    var renewalDiv = document.getElementById("loanRenewalFragmentHolder");
    renewalDiv.innerHTML = "";

    WinJS.UI.Fragments.renderCopy("/fragments/loanRenewal/loanRenewal.html", renewalDiv).done(function () {

        var renewalAnchor = document.getElementById("loanTemplateHolder");

        LoanRenewal.showFlyout(renewalAnchor, null, loan);
    });


};
var addFinesToDom = function (fines) {

    if (fines == undefined)
        return;

    var fineTemplate = new WinJS.Binding.Template(document.getElementById("fineTemplate"));
    var fineTemplateContainer = document.getElementById("fineTemplateHolder");
    if (fineTemplateContainer) {
        fineTemplateContainer.innerHTML = "";

        var i, fine;
        for (i = 0; i < fines.length; i++) {
            fine = fines[i];
            fineTemplate.render(fine, fineTemplateContainer);
        }
    }
};
var addLoansToDom = function (loans) {

    if (loans == undefined) {
        $(".renewAllButton").css("display", "none");
        $("#loanTemplateHolder").text("Du har ingen lån");
        return;
    }

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

    $(".renewAllButton").click(function () {
        var index = $(this).attr("index");

        for (i = 0; i < loans.length; i++) {
            renewLoan(loans[i]);
        }
    });

};
var addReservationsToDom = function (reservations) {

    if (reservations == undefined) {
        $("#reservationTemplateHolder").text("Du har ingen reserveringer");
        return;
    }
    var reservationTemplate = new WinJS.Binding.Template(document.getElementById("reservationTemplate"));
    var reservationsTemplateContainer = document.getElementById("reservationTemplateHolder");

    reservationsTemplateContainer.innerHTML = "";

    var i, reservation;
    for (i = 0; i < reservations.length; i++) {
        reservation = reservations[i];
        if (!reservation) continue;
        reservationTemplate.render(reservation, reservationsTemplateContainer).done(function (element) {
            $(element).find(".cancelReservationButton:last").attr("index", i);
            $(element).find(".cancelReservationButton:last").click(function () {
                var index = $(this).attr("index");
                cancelReservation(reservations, index, $(this));
            });
        });
    }
};

var addNotificationsToDom = function (notifications) {
    if (notifications == undefined) {
        $("#notificationTemplateHolder").text("Du har ingen meldinger");
        return;
    }

    var notificationTemplate = new WinJS.Binding.Template(document.getElementById("notificationTemplate"));
    var notificationTemplateContainer = document.getElementById("notificationTemplateHolder");

    notificationTemplateContainer.innerHTML = "";

    var i, notification;
    for (i = 0; i < notifications.length; i++) {
        notification = notifications[i];
        if (!notification) continue;
        notificationTemplate.render(notification, notificationTemplateContainer);
    }
};

var addColors = function () {

    $("#fines").css("background-color", Data.colorPoolRgba[1]);
    $("#loans").css("background-color", Data.colorPoolRgba[3]);
    $("#reservations").css("background-color", Data.colorPoolRgba[6]);
    $("#notifications").css("background-color", Data.colorPoolRgba[4]);

    $("#myPagePersonalInformation").children().each(function () {
        $(this).css("background-color", Data.colorPoolRgba[0]);
    });

    /**
    $(".box").each(function () {
        $(this).css("background-color", Data.getRandomColor())
    });*/
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

                var notifications = response.Notifications;
                delete response.Notifications;

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
                this.addNotificationsToDom(notifications);
                this.addColors();

            }

            // Hide progress-ring, show content
            $("#mypageLoading").css("display", "none").css("visibility", "none");
            $("#mypageData").css("display", "-ms-flexbox").css("visibility", "visible").hide().fadeIn(500);

        }, this)
    );

    WinJS.Namespace.define("MyPage", {
        addReservationsToDom: addReservationsToDom,
    });

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
            return holdRequestFrom == "" ? "" : "Reservert: " + holdRequestFrom;
        }),
        holdRequestEndConverter: WinJS.Binding.converter(function (holdRequestEnd) {
            if (holdRequestEnd == undefined) return "";
            return holdRequestEnd == "" ? "" : "Hentefrist: " + holdRequestEnd;
        }),
        pickupLibraryConverter: WinJS.Binding.converter(function (pickupLibrary) {
            if (pickupLibrary == undefined) return "";
            return pickupLibrary == "" ? "" : "Hentes hos: " + pickupLibrary;
        }),
        holdRequestReadyTextConverter: WinJS.Binding.converter(function (holdRequestEnd) {
            return holdRequestEnd == undefined || holdRequestEnd == "" ? "Klar til henting: Nei" : "Klar til henting: Ja";
        }),


    });

};