(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var utils = WinJS.Utilities;
    var ui = WinJS.UI;

    ui.Pages.define("/pages/mypage/mypage.html", {

        ready: function (element, options) {
            getUserInformation(true);
            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());
        },

        unload: function () {
            Solvberget.Queue.CancelQueue('mypage');
        }

    });

})();

var getUserInformation = function (loadingMypage) {

    // Show progress-ring, hide content
    $("#mypageData").hide();
    $("#mypageLoading").fadeIn();

    // Get the user information from server
    ajaxGetUserInformation(loadingMypage);

};

var ajaxGetUserInformation = function (isLoadingMyPage) {

    var borrowerId = LoginFlyout.getLoggedInBorrowerId();
    if (borrowerId != undefined && borrowerId !== "") {
        var url = window.Data.serverBaseUrl + "/User/GetUserInformation/" + borrowerId;
        var headers = { "If-Modified-Since": "Mon, 27 Mar 1972 00:00:00 GMT" };
        Solvberget.Queue.QueueDownload("mypage", { url: url, headers: headers }, ajaxGetUserInformationCallback, isLoadingMyPage, true);
    }
    else if (isLoadingMyPage) {
        Data.navigateToHome();
    }

};

var ajaxGetUserInformationCallback = function (request, context) {

    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

    var isLoadingMyPage = context;

    if (response != undefined && response !== "") {
        // Extract fines from object
        var fines = response.ActiveFines;
        // Delete fines from main object
        delete response.ActiveFines;

        // Extract loans from object
        var loans = response.Loans;
        // Delete loans from main objectareNotificationsSeen
        delete response.Loans;

        // Extract reservations from object
        var reservations = response.Reservations;
        // Delete reservations from main object
        delete response.Reservations;

        var notifications = response.Notifications;
        delete response.Notifications;

        if (response.Name === response.PrefixAddress)
            response.PrefixAddress = "";

        if (isLoadingMyPage) {

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

            addFinesToDom(fines);
            addLoansToDom(loans);
            addReservationsToDom(reservations);
            addNotificationsToDom(notifications);
            addFavoritesToDom();
            addColors();
        }

        if ((notifications) && (LoginFlyout.getLoggedInBorrowerId() != "" && LoginFlyout.getLoggedInBorrowerId() != undefined)) {
            if (!Notifications.areNotificationsSeen()) {
                for (i = 0; i < notifications.length; i++) {
                    Toast.showToast(notifications[i].Title, notifications[i].Content);
                }
            }
            Notifications.setUserNotifications(notifications);
        }

    }

    // Hide progress-ring, show content
    $("#mypageLoading").hide();
    $("#mypageData").fadeIn("slow");//css("display", "-ms-flexbox").css("visibility", "visible").hide().fadeIn(500);


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
            if (fines[i].DocumentNumber !== "") {
                var context = { DocumentNumber: fines[i].DocumentNumber };
                $(".clickableFinesArea:last").click($.proxy(function () {
                    var docModel = this;
                    WinJS.Navigation.navigate("/pages/documentDetail/documentDetail.html", { documentModel: docModel });
                }, context));
            }
            else {
                $(".clickableFinesArea:last").removeClass("clickableArea");
            }
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
    if (loansTemplateContainer)
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
            var context = { DocumentNumber: loans[i].DocumentNumber };
            $(".clickableLoanArea:last").click($.proxy(function () {
                var docModel = this;
                WinJS.Navigation.navigate("/pages/documentDetail/documentDetail.html", { documentModel: docModel });
            }, context));
        });
    }

    $(".renewAllButton").click(function () {
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

            var context = { DocumentNumber: reservations[i].DocumentNumber };
            $(".clickableReservationArea:last").click($.proxy(function () {
                var docModel = this;
                WinJS.Navigation.navigate("/pages/documentDetail/documentDetail.html", { documentModel: docModel });
            }, context));

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

var addFavoritesToDom = function (favoritesToUse) {

    var favorites;

    if (favoritesToUse)
        favorites = favoritesToUse;
    else
        favorites = getFavorites();

    if (!favorites || favorites === "") {
        $("#favoritesTemplateHolder").text("Du oppstod en feil ved henting av favoritter");
        return;
    }
    else if (favorites.length === 0) {
        $("#favoritesTemplateHolder").text("Du har ikke lagt til noen favoritter");
        return;
    }

    var favoriteTemplate = new WinJS.Binding.Template(document.getElementById("favoriteTemplate"));
    var favoritesTemplateContainer = document.getElementById("favoritesTemplateHolder");

    favoritesTemplateContainer.innerHTML = "";

    var i;
    var favorite;
    for (i = 0; i < favorites.length; i++) {
        favorite = favorites[i];
        if (!favorite) continue;
        favoriteTemplate.render(favorite, favoritesTemplateContainer).done(function (element) {
            $(element).find(".deleteFavoriteButton:last").attr("index", i);
            $(element).find(".deleteFavoriteButton:last").click(function () {
                var index = $(this).attr("index");
                deleteFavorite(favorites, index);
            });

            var context = { DocumentNumber: favorites[i].DocumentNumber };
            $(".clickableFavoriteArea:last").click($.proxy(function () {
                var docModel = this;
                WinJS.Navigation.navigate("/pages/documentDetail/documentDetail.html", { documentModel: docModel });
            }, context));

        });
    }

}

var getFavorites = function () {

    var applicationData = Windows.Storage.ApplicationData.current;
    var internalLibraryUserId = LoginFlyout.getLoggedInLibraryUserId();
    if (!applicationData || !internalLibraryUserId || internalLibraryUserId === "") {
        return "";
    }
    else {
        var roamingSettings = applicationData.roamingSettings;
        if (roamingSettings) {
            var key = "favorites-" + internalLibraryUserId;
            var favoritesUnparsed = roamingSettings.values[key];
            if (favoritesUnparsed && !jQuery.isEmptyObject(favoritesUnparsed)) {
                var favorites = JSON.parse(favoritesUnparsed);
                if (favorites) {
                    return favorites;
                }
            }
            return [];
        }
        else {
            return "";
        }
    }
}

var deleteFavorite = function (favorites, index) {
    if (favorites && index) {
        favorites.splice(index, 1);
        storeModifiedFavorites(favorites);
        addFavoritesToDom(favorites);
    }
}

var storeModifiedFavorites = function (favorites) {
    var applicationData = Windows.Storage.ApplicationData.current;
    if (applicationData) {
        var internalLibraryUserId = LoginFlyout.getLoggedInLibraryUserId();
        if (internalLibraryUserId && internalLibraryUserId !== "") {
            var roamingSettings = applicationData.roamingSettings;
            if (roamingSettings) {
                var key = "favorites-" + internalLibraryUserId;
                roamingSettings.values[key] = JSON.stringify(favorites);
            }
        }
    }
}

var addColors = function () {
    var alpha = "1.0";
    $("#notifications").css("background-color", Data.getColorFromSubsetPool(0, alpha));
    $("#myPagePersonalInformation").children().each(function () {
        $(this).css("background-color", Data.getColorFromSubsetPool(1, alpha));
    });
    $("#loans").css("background-color", Data.getColorFromSubsetPool(2, alpha));
    $("#reservations").css("background-color", Data.getColorFromSubsetPool(4, alpha));
    $("#fines").css("background-color", Data.getColorFromSubsetPool(5, alpha));
    $("#favorites").css("background-color", Data.getColorFromSubsetPool(6, alpha));
};

WinJS.Namespace.define("MyPage", {
    ajaxGetUserInformation: ajaxGetUserInformation,
    addReservationsToDom: addReservationsToDom
});

WinJS.Namespace.define("MyPageConverters", {

    balanceConverter: WinJS.Binding.converter(function (balance) {
        if (balance == undefined) return "Du har ingen gebyrer";
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
    nullConverter: WinJS.Binding.converter(function (value) {
        return value == undefined ? "" : value;
    })
    
});