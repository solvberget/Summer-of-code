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

    //var serverBaseUrl = "http://localhost:7089";
    var serverBaseUrl = "http://31.24.130.26";

    var colorPoolRgba = ["rgba(255, 153, 0, ", "rgba(204, 51, 0, ", "rgba(136, 187, 0, ", "rgba(0, 85, 34, ", "rgba(0, 153, 204, ", "rgba(0, 51, 102, ", "rgba(102, 0, 102, ", "rgba(51, 0, 51, "];
    var colorPoolSubsetSorted = ["rgba(0, 51, 102, ", "rgba(0, 153, 204, ", "rgba(0, 85, 34, ", "rgba(136, 187, 0, ", "rgba(255, 153, 0, ", "rgba(204, 51, 0, " ];
    var colorPoolBlogs = ["rgba(0, 51, 102, ", "rgba(0, 85, 34, ", "rgba(204, 51, 0, "];

    var menuItems = [
        { key: "lists", title: "Anbefalinger", subtitle: "Anbefalinger og topplister", backgroundImage: tasks, icon: "icon-book", navigateTo: navigateToLists },
        { key: "openingHours", title: "Åpningstider", subtitle: "", backgroundImage: openingHours, icon: "icon-info-sign", navigateTo: navigateToOpeningHours },
        { key: "contact", title: "Kontakt oss", subtitle: "", backgroundImage: contact, icon: "icon-phone", navigateTo: navigateToContact },
        { key: "search", title: "Søk", subtitle: "Søk etter bøker, filmer eller lydbøker", icon: "icon-search", backgroundImage: search, navigateTo: searchHandler },
        { key: "blogs", title: "Blogger", subtitle: "", backgroundImage: blogs, icon: "icon-edit", navigateTo: navigateToBlogs },
        { key: "news", title: "Nyheter", subtitle: "", backgroundImage: news, icon: "icon-rss", navigateTo: navigateToNews },
        { key: "mypage", title: "Min Side", subtitle: "", backgroundImage: home, icon: "icon-user", navigateTo: navigateToMypage },
        { key: "events", title: "Arrangementer", subtitle: "Hva skjer på Sølvberget", icon: "icon-calendar", backgroundImage: events, navigateTo: navigateToEvents }
    ];

    var menuItemsPortrait = [
        menuItems[0], menuItems[3], menuItems[6], menuItems[1], menuItems[2], menuItems[4], menuItems[5],
        { key: "events", title: "Arrangementer", subtitle: "", icon: "icon-calendar", backgroundImage: events, navigateTo: navigateToEvents }
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

    function getColorFromSubsetPool(index, alpha) {
        if (index >= 0 && index < colorPoolSubsetSorted.length) {
            var color = colorPoolSubsetSorted[index];
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

    function getColorFromBlogsPool(index, alpha) {
        if (index >= 0 && index < colorPoolBlogs.length) {
            var color = colorPoolBlogs[index];
            if (alpha)
                color = color + alpha + ")";
            else
                color = color + "0.6)";
            return color;

        }
        else {
            return colorPoolBlogs[0] + "0.6)";
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
    var listPortrait = new WinJS.Binding.List(menuItemsPortrait);

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
        itemsPortrait: listPortrait,
        itemByKey: itemByKey,
        menuItems: menuItems,
        menuItemsPortrait: menuItemsPortrait,
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
        getColorFromPool: getColorFromPool,
        getColorFromSubsetPool: getColorFromSubsetPool,
        getColorFromBlogsPool: getColorFromBlogsPool
    });
})();
