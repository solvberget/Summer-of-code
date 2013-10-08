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

        ready: function (element) {
            $(".pagetitle").html("Nyheter fra Sølvberget");
            var newsItemsListView = element.querySelector(".newsItemsListView").winControl;
            newsItemsListView.itemTemplate = this.itemTemplateFunction;
            newsItemsListView.oniteminvoked = this.newsItemInvoked.bind(this);
            this.getNewsItems(newsItemsListView);

            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());

        },

        getNewsItems: function (newsItemsListView) {
            var url = window.Data.serverBaseUrl + newsReqUrl;
            var context = { listView: newsItemsListView, that: this };
            Solvberget.Queue.QueueDownload("news", { url: url }, this.getNewsItemsCallback, context, true);
        },

        getNewsItemsCallback: function (request, context) {
            var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
            if (response != undefined && response !== "") {
                context.that.newsItems = new WinJS.Binding.List(response);
                context.that.initializeLayout(context.listView, appView.value);
                $("#newsItemsLoading").hide();
                $(".newsItemsListView").fadeIn();
                context.listView.element.focus();
            }
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
            var newsItemslistView = element.querySelector(".newsItemsListView");
            if (newsItemslistView) {
                var newsItemslistViewWinControl = newsItemslistView.winControl;
                if (newsItemslistViewWinControl)
                    if (lastViewState !== viewState) {
                        if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                            var handler = function (e) {
                                newsItemslistViewWinControl.removeEventListener("contentanimating", handler, false);
                                e.preventDefault();
                            }
                            newsItemslistViewWinControl.addEventListener("contentanimating", handler, false);
                            this.initializeLayout(newsItemslistViewWinControl, viewState, element);
                        }
                    }
            }
        },

        initializeLayout: function (newsItemsListView, viewState) {
            if (this.newsItems) {
                if (viewState === appViewState.snapped) {
                    $(".pagetitle").html("Nyheter");
                    newsItemsListView.itemDataSource = this.newsItems.dataSource;
                    newsItemsListView.layout = new ui.ListLayout();
                } else {
                    newsItemsListView.itemDataSource = this.newsItems.dataSource;
                    newsItemsListView.layout = new ui.GridLayout();
                }
            }
        },

        itemTemplateFunction: function (itemPromise) {
            return itemPromise.then(function (item) {
                var colorIndex = Math.floor(Math.random() * Data.colorPoolRgba.length);
                var newsItemTemplate = document.getElementById("newsItemTemplate");
                var container = document.createElement("div");
                container.style.backgroundColor = Data.getColorFromPool(colorIndex, 1.0);
                newsItemTemplate.winControl.render(item.data, container);
                return container;
            });
        },

        unload: function () {
            Solvberget.Queue.CancelQueue('news');
        },

    });

})();