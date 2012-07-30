(function () {
    "use strict";

    var search = "/images/home/Search.png";
    var events = "/images/home/Events.png";
    var home = "/images/home/MyPage.png";
    var contact = "/images/home/Contact.png";
    var openingHours = "/images/home/OpeningHours.png";
    var tasks = "/images/home/Lists.png";
    var activePage = "home";

    var serverBaseUrl = "http://localhost:7089";

    var menuItems = [
        { key: "lists", title: "Nyheter og anbefalinger", subtitle: "Nyheter, anbefalinger og topplister", backgroundImage: tasks, navigateTo: function () { activePage = "lists"; WinJS.Navigation.navigate("/pages/lists-v2/libraryLists.html"); } },
        { key: "mypage", title: "Min Side", subtitle: "", backgroundImage: home, navigateTo: function () { activePage = "mypage"; loginThenNavigateTo("/pages/mypage/mypage.html"); } },
        { key: "events", title: "Arrangementer", subtitle: "Hva skjer på Sølvberget", backgroundImage: events, navigateTo: function () { activePage = "events"; WinJS.Navigation.navigate("/pages/events/events.html"); } },
        { key: "openingHours", title: "Åpningstider", subtitle: "Velkommen", backgroundImage: openingHours, navigateTo: function () { activePage = "openingHours"; WinJS.Navigation.navigate("/pages/openingHours/openingHours.html"); } },
         { key: "contact", title: "Kontaktinformasjon", subtitle: "For spørsmål", backgroundImage: contact, navigateTo: function () { activePage = "contact"; WinJS.Navigation.navigate("/pages/contact/contact.html"); } },
        { key: "search", title: "Søk", subtitle: "Søk etter bøker, filmer eller lydbøker", backgroundImage: search, navigateTo: function () { Windows.ApplicationModel.Search.SearchPane.getForCurrentView().show(); } },
    ];

    function goHome() {
        return WinJS.Navigation.navigate("/pages/home/home.html");
    };

    var loginThenNavigateTo = function (page) {


        var loginDiv = document.getElementById("loginDiv");

        WinJS.UI.Fragments.renderCopy("/fragments/login/login.html", loginDiv).done(function () {

            var loginAnchor = document.querySelector(".win-container:nth-child(1)");
            LoginFlyout.showLogin(loginAnchor, page);

        });

    }

    var itemByKey = function (key) {

        for (var i = 0; i < menuItems.length; i++) {
            if (key === menuItems[i].key)
                return menuItems[i];
        }
    }

    var list = new WinJS.Binding.List(menuItems);

    WinJS.Namespace.define("Data", {

        items: list,
        itemByKey: itemByKey,
        menuItems: menuItems,
        serverBaseUrl: serverBaseUrl,
        goHome: goHome,
        activePage: activePage,

    });
})();
