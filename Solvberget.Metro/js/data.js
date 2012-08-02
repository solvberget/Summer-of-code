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
        { key: "lists", title: "Nyheter og anbefalinger", subtitle: "Nyheter, anbefalinger og topplister", backgroundImage: tasks, navigateTo: navigateToLists },
        { key: "mypage", title: "Min Side", subtitle: "", backgroundImage: home, navigateTo: navigateToMypage },
        { key: "events", title: "Arrangementer", subtitle: "Hva skjer på Sølvberget", backgroundImage: events, navigateTo: navigateToEvents },
        { key: "openingHours", title: "Åpningstider", subtitle: "Velkommen inn", backgroundImage: openingHours, navigateTo: navigateToOpeningHours },
        { key: "contact", title: "Kontakt oss", subtitle: "Kontaktinformasjon", backgroundImage: contact, navigateTo: navigateToContact },
        { key: "search", title: "Søk", subtitle: "Søk etter bøker, filmer eller lydbøker", backgroundImage: search, navigateTo: navigateToSearch },
    ];

    function navigateToHome() {
        return WinJS.Navigation.navigate("/pages/home/home.html");
    };

    var list = new WinJS.Binding.List(menuItems);

    function navigateToHome() {
        activePage = "home"; WinJS.Navigation.navigate("/pages/home/home.html");
    }


    function navigateToLists() 
    { 
        activePage = "lists"; WinJS.Navigation.navigate("/pages/lists-v2/libraryLists.html"); 
    }

    function navigateToMypage() 
    { 
        activePage = "mypage"; loginThenNavigateTo("/pages/mypage/mypage.html"); 
    }

    function navigateToEvents() 
    { 
        activePage = "events"; WinJS.Navigation.navigate("/pages/events/events.html"); 
    }

    function navigateToOpeningHours() 
    { 
        activePage = "openingHours"; WinJS.Navigation.navigate("/pages/openingHours/openingHours.html"); 
    }

    function navigateToContact() 
    { 
        activePage = "contact"; WinJS.Navigation.navigate("/pages/contact/contact.html");
    } 

    function navigateToSearch() 
    { 
        Windows.ApplicationModel.Search.SearchPane.getForCurrentView().show(); 
    }

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

    WinJS.Namespace.define("Data", {

        items: list,
        itemByKey: itemByKey,
        menuItems: menuItems,
        serverBaseUrl: serverBaseUrl,
        activePage: activePage,
        navigateToHome: navigateToHome,
        navigateToLists: navigateToLists,
        navigateToMypage: navigateToMypage,
        navigateToEvents: navigateToEvents,
        navigateToOpeningHours: navigateToOpeningHours,
        navigateToContact: navigateToContact,
        navigateToSearch: navigateToSearch,
    });
})();
