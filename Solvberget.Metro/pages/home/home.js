(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var ui = WinJS.UI;

    ui.Pages.define("/pages/home/home.html", {

        ready: function (element, options) {
            var listView = element.querySelector(".home-list").winControl;
            listView.oniteminvoked = this.itemInvoked.bind(this);
            this.initializeLayout(listView, Windows.UI.ViewManagement.ApplicationView.value, element);
            listView.element.focus();
            MyPage.ajaxGetUserInformation(false);
        },

        updateLayout: function (element, viewState, lastViewState) {
            var listView = element.querySelector(".home-list").winControl;
            if (lastViewState !== viewState) {
                if (lastViewState !== appViewState.fullScreenLandscape || viewState !== appViewState.full) {
                    var handler = function (e) {
                        listView.removeEventListener("contentanimating", handler, false);
                        e.preventDefault();
                    }
                    listView.addEventListener("contentanimating", handler, false);
                    var firstVisible = listView.indexOfFirstVisible;
                    this.initializeLayout(listView, viewState, element);
                    listView.indexOfFirstVisible = firstVisible;
                }
            }
        },

        initializeLayout: function (listView, viewState, element) {
            if (viewState === appViewState.snapped) {
                listView.itemDataSource = Data.itemsSnapped.dataSource;
                listView.itemTemplate = snappedTemplateRenderer;
                listView.layout = new ui.ListLayout();
            } else if (viewState === appViewState.fullScreenPortrait) {
                listView.itemDataSource = Data.itemsPortrait.dataSource;
                listView.itemTemplate = multisizeItemTemplateRendererPortrait;
                listView.layout = new ui.GridLayout({ groupInfo: groupInfo, groupHeaderPosition: "top" });
            }
            else {
                listView.itemDataSource = Data.items.dataSource;
                listView.itemTemplate = multisizeItemTemplateRendererLandscape;
                listView.layout = new ui.GridLayout({ groupInfo: groupInfo, groupHeaderPosition: "top" });
            }
        },

        itemInvoked: function (args) {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            if (viewState === appViewState.fullScreenLandscape) {
                Data.menuItems[args.detail.itemIndex].navigateTo();
            } else if (viewState === appViewState.fullScreenPortrait) {
                Data.menuItemsPortrait[args.detail.itemIndex].navigateTo();
            } else if (viewState === appViewState.snapped) {
                Data.menuItemsSnapped[args.detail.itemIndex].navigateTo();
            }
            else {
                Data.menuItems[args.detail.itemIndex].navigateTo();
            }
        },

    });

    function groupInfo() {
        return {
            enableCellSpanning: true,
            cellWidth: 125,
            cellHeight: 125
        };
    }

    function multisizeItemTemplateRendererLandscape(itemPromise) {
        return itemPromise.then(function (currentItem) {
            var content;
            content = document.getElementsByClassName("home-template")[0];
            var result = content.cloneNode(true);
            switch (currentItem.data.key) {
                case "news":
                    {
                        result.className = "home-small-template color4";
                    }
                    break;
                case "mypage":
                    {
                        result.className = "home-large-template color5";
                    }
                    break;
                case "events":
                    {
                        result.className = "home-medium-template color6";
                    }
                    break;
                case "openingHours":
                    {
                        result.className = "home-small-template color2";
                    }
                    break;
                case "lists":
                    {
                        result.className = "home-large-template color1";
                    }
                    break;
                case "contact":
                    {
                        result.className = "home-small-template color2";
                    }
                    break;
                case "blogs":
                    {
                        result.className = "home-small-template color4";
                    }
                    break;
                case "search":
                    {
                        result.className = "home-large-template color3";
                    }
                    break;
                default:
                    {
                        result.className = "home-large-template color1";
                    }
            }

            result.attributes.removeNamedItem("data-win-control");
            result.attributes.removeNamedItem("style");
            result.style.overflow = "hidden";

            result.getElementsByClassName("item-title")[0].textContent = currentItem.data.title;
            result.getElementsByClassName("item-subtitle")[0].textContent = currentItem.data.subtitle;
            result.getElementsByClassName("item-vector-icon")[0].innerHTML = "<i class=\"" + currentItem.data.icon + "\"></i>";
            return result;

        });
    }

    function multisizeItemTemplateRendererPortrait(itemPromise) {
        return itemPromise.then(function (currentItem) {
            var content;
            content = document.getElementsByClassName("home-template")[0];
            var result = content.cloneNode(true);
            switch (currentItem.data.key) {
                case "news":
                    {
                        result.className = "home-small-template color4";
                    }
                    break;
                case "mypage":
                    {
                        result.className = "home-large-template color5";
                    }
                    break;
                case "events":
                    {
                        result.className = "home-medium-rotate-template color6";
                    }
                    break;
                case "openingHours":
                    {
                        result.className = "home-small-template color2";
                    }
                    break;
                case "lists":
                    {
                        result.className = "home-large-template color1";
                    }
                    break;
                case "contact":
                    {
                        result.className = "home-small-template color2";
                    }
                    break;
                case "blogs":
                    {
                        result.className = "home-small-template color4";
                    }
                    break;
                case "search":
                    {
                        result.className = "home-large-template color3";
                    }
                    break;
                default:
                    {
                        result.className = "home-medium-template color1";
                    }
            }

            result.attributes.removeNamedItem("data-win-control");
            result.attributes.removeNamedItem("style");
            result.style.overflow = "hidden";

            result.getElementsByClassName("item-title")[0].textContent = currentItem.data.title;
            result.getElementsByClassName("item-subtitle")[0].textContent = currentItem.data.subtitle;
            result.getElementsByClassName("item-vector-icon")[0].innerHTML = "<i class=\"" + currentItem.data.icon + "\"></i>";
            return result;

        });
    }

    function snappedTemplateRenderer(itemPromise) {
        return itemPromise.then(function (currentItem) {
            var content;
            content = document.getElementsByClassName("home-template")[0];
            var result = content.cloneNode(true);
            switch (currentItem.data.key) {
                case "news":
                    {
                        result.className = "home-snapped-template color4";
                    }
                    break;
                case "mypage":
                    {
                        result.className = "home-snapped-template color5";
                    }
                    break;
                case "events":
                    {
                        result.className = "home-snapped-template color6";
                    }
                    break;
                case "openingHours":
                    {
                        result.className = "home-snapped-template color2";
                    }
                    break;
                case "lists":
                    {
                        result.className = "home-snapped-template color1";
                    }
                    break;
                case "contact":
                    {
                        result.className = "home-snapped-template color2";
                    }
                    break;
                case "blogs":
                    {
                        result.className = "home-snapped-template color4";
                    }
                    break;
                case "search":
                    {
                        result.className = "home-snapped-template color3";
                    }
                    break;
                default:
                    {
                        result.className = "home-snapped-template color1";
                    }
            }

            result.attributes.removeNamedItem("data-win-control");
            result.attributes.removeNamedItem("style");
            result.style.overflow = "hidden";

            result.getElementsByClassName("item-title")[0].textContent = currentItem.data.title;
            result.getElementsByClassName("item-subtitle")[0].textContent = "";
            result.getElementsByClassName("item-vector-icon")[0].innerHTML = "<i class=\"" + currentItem.data.icon + "\"></i>";
            return result;

        });
    }

})();
