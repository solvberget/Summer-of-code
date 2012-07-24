(function () {
    "use strict";

    var search = "/images/home/Search.png";
    var events = "/images/home/Events.png";
    var info = "/images/icon/Icon Pack 1/png/info.png";
    var music = "/images/icon/Icon Pack 1/png/music.png";
    var home = "/images/home/MyPage.png";
    var tasks = "/images/home/Lists.png";
    var activePage = "home";

    var serverBaseUrl = "http://localhost:7089";

    var menuItems = [
<<<<<<< HEAD
        { key: "lists", title: "Lister fra Biblioteket", subtitle: "Mest lest, nyheter etc.", backgroundImage: tasks, navigateTo: function () { activePage = "lists"; WinJS.Navigation.navigate("/pages/lists/libraryLists.html"); } },
        { key: "mypage", title: "Min Side", subtitle: "", backgroundImage: home, navigateTo: function () { activePage = "mypage"; loginThenNavigateTo("/pages/mypage/mypage.html"); } },
        {
            key: "events", title: "Arrangementer", subtitle: "Hva skjer på Sølvberget", backgroundImage: events,
            navigateTo: function () {
                activePage = "events"; WinJS.Navigation.navigate("/pages/events/events.html"
                    );
            }
        },
=======
        { key: "lists", title: "Nyheter og anbefalinger", subtitle: "Topplister, nyheter og anbefalinger", backgroundImage: tasks, navigateTo: function () { activePage = "/pages/lists/libraryLists.html"; WinJS.Navigation.navigate("/pages/lists/libraryLists.html"); } },
        { key: "mypage", title: "Min Side", subtitle: "", backgroundImage: home, navigateTo: function () { activePage = "/pages/mypage/mypage.html"; loginThenNavigateTo("/pages/mypage/mypage.html"); } },
        { key: "events", title: "Arrangementer", subtitle: "Hva skjer på Sølvberget", backgroundImage: events, navigateTo: function () { activePage = "/pages/events/events.html"; WinJS.Navigation.navigate("/pages/events/events.html"); } },
>>>>>>> 06b1342409d51ff02847b6d9782bc260918157fe
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
