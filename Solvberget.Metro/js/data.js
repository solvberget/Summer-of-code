(function () {
    "use strict";

    var search = "/images/home/Search.png";
    var events = "/images/home/Events.png";
    var home = "/images/home/MyPage.png";
    var contact = "/images/home/Contact.png";
    var openingHours = "/images/home/OpeningHours.png";
    var tasks = "/images/home/Lists.png";
    var blogs = "/images/home/Blogs.png";
    var news = "/images/home/News.png";
    var activePage = "home";

    var serverBaseUrl = "http://localhost:7089";

    var colorPoolRgba = ["rgba(255, 153, 0, ", "rgba(204, 51, 0, ", "rgba(136, 187, 0, ", "rgba(0, 85, 34, ", "rgba(0, 153, 204, ", "rgba(0, 51, 102, ", "rgba(102, 0, 102, ", "rgba(51, 0, 51, "];

    var menuItems = [
        { key: "news", title: "Nyheter", subtitle: "Nyheter fra Sølvberget", backgroundImage: news, navigateTo: navigateToNews },
        { key: "mypage", title: "Min Side", subtitle: "", backgroundImage: home, navigateTo: navigateToMypage },
        { key: "events", title: "Arrangementer", subtitle: "Hva skjer på Sølvberget", backgroundImage: events, navigateTo: navigateToEvents },
        { key: "openingHours", title: "Åpningstider", subtitle: "Velkommen inn", backgroundImage: openingHours, navigateTo: navigateToOpeningHours },
        { key: "lists", title: "Anbefalinger", subtitle: "Anbefalinger og topplister", backgroundImage: tasks, navigateTo: navigateToLists },
        { key: "contact", title: "Kontakt oss", subtitle: "Kontaktinformasjon", backgroundImage: contact, navigateTo: navigateToContact },
        { key: "blogs", title: "Blogger", subtitle: "Utvalgte blogger", backgroundImage: blogs, navigateTo: navigateToBlogs },
        { key: "search", title: "Søk", subtitle: "Søk etter bøker, filmer eller lydbøker", backgroundImage: search, navigateTo: searchHandler },
    ];

    function searchHandler() {

        var currentViewState = Windows.UI.ViewManagement.ApplicationView.value;
        if (currentViewState === 2) {

            // Create the message dialog and set its content
            var snapDialog = new Windows.UI.Popups.MessageDialog(
                "Det er ikke mulig å søke i snapview\n\n" +
                    "Dra vinduet ut i vanlig visning for å søke", "Ooops!");

            snapDialog.commands.append(
                new Windows.UI.Popups.UICommand("Lukk", function () { }, 1));

            // Set the command that will be invoked by default
            snapDialog.defaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            snapDialog.cancelCommandIndex = 1;

            try {

                // Show the message dialog
                snapDialog.showAsync();

            } catch (exception) {
                // No access exception
                console.log(new Date().toString() + ": No access exception(cant display dialog)");
            }
        }

        else {
            navigateToSearch();
        }
    };


    function getColorFromPool(index, alpha) {
        if (index >= 0 && index < colorPoolRgba.length) {
            var color = colorPoolRgba[index];
            if (alpha) 
                color = color + alpha + ")";        
            else 
              color = color + "0.6)";
            return color;

        }
        else {
            return colorPoolRgba[0] + "0.6)";
        }
    };

    function getRandomColor(alpha) {

        var random = Math.random() * colorPoolRgba.length;
        random = Math.floor(random);
        var color = colorPoolRgba[random];
        if (alpha)
            color = color + alpha + ")";
        else
            color = color + "0.6)";
        return color;

    };


    var list = new WinJS.Binding.List(menuItems);

    function getActivePage() {
        return WinJS.Navigation.location;
    }


    function navigateToHome() {
        Data.activePage = "home"; WinJS.Navigation.navigate("/pages/home/home.html");
    }

    function navigateToNews() {
        Data.activePage = "news"; WinJS.Navigation.navigate("/pages/news/news.html");
    }

    function navigateToLists() {
        Data.activePage = "lists"; WinJS.Navigation.navigate("/pages/lists/libraryLists.html");
    }

    function navigateToMypage() {
        Data.activePage = "mypage"; loginThenNavigateTo("/pages/mypage/mypage.html");
    }

    function navigateToEvents() {
        Data.activePage = "events"; WinJS.Navigation.navigate("/pages/events/groupedEvents/groupedEvents.html");
    }

    function navigateToOpeningHours() {
        Data.activePage = "openingHours"; WinJS.Navigation.navigate("/pages/openingHours/openingHours.html");
    }

    function navigateToContact() {
        Data.activePage = "contact"; WinJS.Navigation.navigate("/pages/contact/contact.html");
    }

    function navigateToSearch() {
        Windows.ApplicationModel.Search.SearchPane.getForCurrentView().show();
    }
    function navigateToBlogs() {
        Data.activePage = "blogs"; WinJS.Navigation.navigate("/pages/blogs/main/blogs.html");
    }

    var loginThenNavigateTo = function (page) {


        var loginDiv = document.getElementById("loginDiv");


        WinJS.UI.Fragments.renderCopy("/fragments/login/login.html", loginDiv).done(function () {

            var loginAnchor = document.querySelector(".win-container:nth-child(1)");
            LoginFlyout.showLogin(loginAnchor, page);

        });

    };

    var itemByKey = function (key) {

        for (var i = 0; i < menuItems.length; i++) {
            if (key === menuItems[i].key)
                return menuItems[i];
        }
    };

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
        navigateToBlogs: navigateToBlogs,
        navigateToNews: navigateToNews,
        colorPoolRgba: colorPoolRgba,
        getRandomColor: getRandomColor,
        getColorFromPool: getColorFromPool
    });
})();
