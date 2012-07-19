(function () {
    "use strict";

    var search = "/images/icon/Icon Pack 1/png/search.png";
    var events = "/images/icon/Icon Pack 1/png/calend.png";
    var info = "/images/icon/Icon Pack 1/png/info.png";
    var music = "/images/icon/Icon Pack 1/png/music.png";
    var home = "/images/icon/Icon Pack 1/png/home.png";
    var tasks = "/images/icon/Icon Pack 1/png/tasks.png";

    var serverBaseUrl = "http://localhost:7089";

    var menuItems = [
        { key: "lists", title: "Lister fra Biblioteket", subtitle: "Mest lest, nyheter etc.", backgroundImage: tasks, navigateTo : function () { WinJS.Navigation.navigate("/pages/lists/libraryLists.html"); } },
        { key: "mypage", title: "Min Side", subtitle: "", backgroundImage: home, navigateTo: function () { loginThenNavigateTo("/pages/mypage/mypage.html"); } },
        { key: "events", title: "Arrangementer", subtitle: "Hva skjer på Sølvberget", backgroundImage: events, navigateTo: function () { WinJS.Navigation.navigate("/pages/events/events.html"); } },
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
        itemByKey : itemByKey,
        menuItems: menuItems,
        serverBaseUrl: serverBaseUrl,
        goHome: goHome,
    });
})();
