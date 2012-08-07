(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;
    var continueToGetEvents = false;

    var requestUrl = Data.serverBaseUrl + "/Event/GetEvents";

    var eventBinding;

    ui.Pages.define("/pages/events/events.html", {

        itemSelectionIndex: -1,

        isSingleColumn: function () {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            return (viewState === appViewState.snapped || viewState === appViewState.fullScreenPortrait);
        },

        ready: function (element, options) {
            continueToGetEvents = true;
            element.querySelector("header[role=banner] .pagetitle").textContent = "Arrangementer";
            
            var listView = element.querySelector(".itemlist").winControl;
            if (listView) {
                listView.layout = new ui.ListLayout();
                listView.itemTemplate = element.querySelector(".itemtemplate");
                listView.onselectionchanged = this.selectionChanged.bind(this);
            }

            this.getEvents(listView);
        },

        getEvents: function (listView) {
            var that = this;
            return WinJS.xhr({ url: requestUrl }).then(

                function (request) {
                    if (!continueToGetEvents) return;
                    var obj = JSON.parse(request.responseText);

                    if (obj !== undefined) {
                        var items = obj;
                        var results = []
                        for (var i = 0, itemsLength = items.length; i < itemsLength; i++) {
                            var dataItem = items[i];
                            results.push({
                                title: dataItem.Name,
                                date: dataItem.Date,
                                start: "Starter kl. " + that.trimTime(dataItem.Start),
                                stop: "Slutter kl. " + that.trimTime(dataItem.Stop),
                                location: dataItem.Location,
                                description: dataItem.Description,
                                type: "Passer for: " + dataItem.TypeName,
                                url: { l: dataItem.Link, t: "Link til arrangement" },
                                address: dataItem.Address,
                                city: dataItem.City + " " + dataItem.PostalCode,
                                thumbImage: dataItem.ThumbUrl,
                                backgroundImage: dataItem.PictureUrl,
                                dateAndTime: dataItem.Date + " " + that.trimTime(dataItem.Start)
                            });
                        }
                        eventBinding = new WinJS.Binding.List(results);
                        listView.itemDataSource = eventBinding.dataSource;
                        if (that.isSingleColumn()) {
                            that.updateVisibility();
                        }
                        else {
                            listView.selection.set(Math.max(that.itemSelectionIndex, 0));
                        }
                        $(listView.id).fadeIn();
                    }
                });
        },

        trimTime: function (time) {
            if (time != undefined) {
                var t = time.trim();
                if (t === "" || t === "null")
                    return "Ukjent tidspunkt";
                else if (t.length > 7)
                    return t.substring(0, 5);
                else
                    return t;
            }
            else {
                return "Ukjent tidspunkt";
            }
        },

        goHome: function () {
            WinJS.Navigation.navigate("/pages/home/home.html");

        },

        selectionChanged: function (args) {
            if (!continueToGetEvents) return;
            var listViewDiv = document.body.querySelector(".itemlist");
            var listView = undefined;
            if (listViewDiv) {
                listView = listViewDiv.winControl;
            }

            if (listView) {
                var details;
                var that = this;
                listView.selection.getItems().done(function updateDetails(items) {
                    if (items.length > 0) {
                        that.itemSelectionIndex = items[0].index;

                        details = document.querySelector(".articlesection");
                        binding.processAll(details, items[0].data);
                        details.scrollTop = 0;
                        if (that.isSingleColumn()) {
                            document.body.querySelector(".articlesection").focus();
                            nav.history.current.state = {
                                selectedIndex: that.itemSelectionIndex
                            };
                            nav.history.backStack.push({
                                location: "/pages/events/events.html",
                                state: {}
                            });
                        }
                        that.updateVisibility();
                    }
                });
            }
        },

        unload: function () {
            continueToGetEvents = false;
        },

        updateLayout: function (element, viewState, lastViewState) {
            var listView = element.querySelector(".itemlist").winControl;
            var firstVisible = listView.indexOfFirstVisible;
            this.updateVisibility();

            if (this.isSingleColumn()) {
                listView.selection.clear();
                if (this.itemSelectionIndex >= 0) {
                    nav.history.current.state = {
                        selectedIndex: this.itemSelectionIndex
                    };
                    nav.history.backStack.push({
                        location: "/pages/events/events.html",
                        state: {}
                    });
                    element.querySelector(".articlesection").focus();
                } else {
                    listView.forceLayout();
                }
            } else {
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/events/events.html") {
                    nav.history.backStack.pop();
                }
                if (viewState !== lastViewState) {
                    listView.forceLayout();
                }
                listView.selection.set(this.itemSelectionIndex >= 0 ? this.itemSelectionIndex : firstVisible);
            }
        },

        updateVisibility: function () {
            var oldPrimary = document.querySelector(".primarycolumn");
            if (oldPrimary) {
                utils.removeClass(oldPrimary, "primarycolumn");
            }
            if (this.isSingleColumn()) {
                if (this.itemSelectionIndex >= 0) {
                    utils.addClass(document.querySelector(".articlesection"), "primarycolumn");
                    document.querySelector(".articlesection").focus();
                } else {
                    utils.addClass(document.querySelector(".itemlistsection"), "primarycolumn");
                    document.querySelector(".itemlist").focus();
                }
            } else {
                document.querySelector(".itemlist").focus();
            }
        }
    });
})();
