(function () {
    "use strict";

    var appView = Windows.UI.ViewManagement.ApplicationView;
    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var utils = WinJS.Utilities;
    var ui = WinJS.UI;

    var newsReqUrl = "/News/GetNewsItems";

    ui.Pages.define("/pages/news/news.html", {

        newsItems: null,
        onPage: false,

        ready: function (element, options) {
            this.onPage = true;
            $(".pagetitle").html("Nyheter fra Sølvberget");
            var newsItemsListView = element.querySelector(".newsItemsListView").winControl;
            newsItemsListView.itemTemplate = this.itemTemplateFunction;
            newsItemsListView.oniteminvoked = this.newsItemInvoked.bind(this);
            this.getNewsItems(newsItemsListView);
        },

        getNewsItems: function (newsItemsListView) {

            $.when(this.ajaxGetNews()).then($.proxy(function (response) {
                if (this.onPage) {
                    if (response != undefined && response !== "") {
                        this.newsItems = new WinJS.Binding.List(response);
                        this.initializeLayout(newsItemsListView, appView);
                        $("#newsItemsLoading").hide();
                        $(".newsItemsListView").fadeIn();
                        newsItemsListView.element.focus();
                    }
                    else {
                        $("#newsItemsLoading").hide();
                        $("#newsLoadError").fadeIn();
                    }
                }
            }, this));

        },

        ajaxGetNews: function () {
            return $.getJSON(window.Data.serverBaseUrl + newsReqUrl).error(function (x, t, m) {
                $("#newsItemsLoading").hide();
                $("#newsLoadError").fadeIn();
            });
        },

        newsItemInvoked: function (args) {
            var uriRaw = this.newsItems.getItem(args.detail.itemIndex).data.Link;
            var uri = new Windows.Foundation.Uri(uriRaw);
            Windows.System.Launcher.launchUriAsync(uri).then(
            function (success) {
                if (success) {
                    //No-op
                } else {
                    //Error
                }
            });
        },

        updateLayout: function (element, viewState, lastViewState) {
            var newsItemslistView = element.querySelector(".newsItemsListView")
            if (newsItemslistView) {
                var newsItemslistViewWinControl = newsItemslistView.winControl;
                if (newsItemslistViewWinControl)
                    if (lastViewState !== viewState) {
                        if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                            this.initializeLayout(newsItemslistViewWinControl, viewState, element);
                        }
                    }
            }
        },

        initializeLayout: function (newsItemsListView, viewState) {
            if (viewState === appViewState.snapped) {
                $(".pagetitle").html("Nyheter");
                newsItemsListView.itemDataSource = this.newsItems.dataSource;
                newsItemsListView.layout = new ui.ListLayout();
            } else {
                newsItemsListView.itemDataSource = this.newsItems.dataSource;
                newsItemsListView.layout = new ui.GridLayout();
            }
        },

        itemTemplateFunction: function (itemPromise) {
            return itemPromise.then(function (item) {
                var colorIndex = Math.floor(Math.random() * Data.colorPoolRgba.length);
                var newsItemTemplate = document.getElementById("newsItemTemplate");
                var container = document.createElement("div");
                container.style.backgroundColor = Data.colorPoolRgba[colorIndex];
                newsItemTemplate.winControl.render(item.data, container);
                return container;
            });
        },

        unload: function () {
            this.onPage = false;
        },

    });

})();

WinJS.Namespace.define("NewsConverters", {

});