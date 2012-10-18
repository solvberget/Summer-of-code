(function () {
    "use strict";

    var appView = Windows.UI.ViewManagement.ApplicationView;
    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;

    var eventsReqUrl = "/Event/GetEvents";

    ui.Pages.define("/pages/events/groupedEvents/groupedEvents.html", {

        ready: function (element) {
            var listView = element.querySelector(".eventItemsListView");
            var listViewWinControl = listView.winControl;
            listViewWinControl.groupHeaderTemplate = element.querySelector(".headertemplate");
            listViewWinControl.oniteminvoked = this.itemInvoked.bind(this);
            this.getEvents(listViewWinControl);
        },

        getEvents: function (listViewWinControl) {
            var url = window.Data.serverBaseUrl + eventsReqUrl;
            var context = { listViewWinControl: listViewWinControl, that: this };
            window.Solvberget.Queue.QueueDownload("events", { url: url }, this.getEventsCallback, context, true);
        },

        getEventsCallback: function (request, context) {
            var response = request.responseText === "" ? "" : JSON.parse(request.responseText);
            if (response != undefined && response !== "") {
                EventData.setData(response);
                context.that.initializeLayout(context.listViewWinControl, appView.value);
                $("#eventItemsLoading").hide();
                $(".eventItemsListView").fadeIn();
                context.listViewWinControl.element.focus();
            }
        },

        initializeLayout: function (listViewWinControl, viewState) {
            if (viewState === appViewState.snapped) {
                listViewWinControl.itemTemplate = this.groupTemplateFunction;
                listViewWinControl.itemDataSource = EventData.groups.dataSource;
                listViewWinControl.groupDataSource = null;
                listViewWinControl.layout = new ui.ListLayout();
            } else {
                listViewWinControl.itemTemplate = this.eventTemplateFunction;
                listViewWinControl.itemDataSource = EventData.items.dataSource;
                listViewWinControl.groupDataSource = EventData.groups.dataSource;
                listViewWinControl.layout = new ui.GridLayout({ groupHeaderPosition: "top" });
            }
        },

        itemInvoked: function (args) {
            if (appView.value === appViewState.snapped) {
                //Snapped: User invoked a group.
                var group = EventData.groups.getAt(args.detail.itemIndex);
                nav.navigate("/pages/events/eventGroupDetail/eventGroupDetail.html", { groupKey: group.key });
            } else {
                //Not snapped: User invoked an item.
                var item = EventData.items.getAt(args.detail.itemIndex);
                nav.navigate("/pages/events/eventDetail/eventDetail.html", { item: EventData.getItemReference(item) });
            }
        },

        updateLayout: function (element, viewState, lastViewState) {
            var listView = element.querySelector(".eventItemsListView");
            if (listView) {
                var listViewWinControl = listView.winControl;
                if (listViewWinControl)
                    if (lastViewState !== viewState) {
                        if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                            var handler = function (e) {
                                listViewWinControl.removeEventListener("contentanimating", handler, false);
                                e.preventDefault();
                            };
                            listViewWinControl.addEventListener("contentanimating", handler, false);
                            this.initializeLayout(listViewWinControl, viewState, element);
                        }
                    }
            }
        },

        eventTemplateFunction: function (itemPromise) {
            return itemPromise.then(function (item) {
                var eventItemTemplate = document.getElementById("event-item-template");
                var container = document.createElement("div");
                container.style.backgroundColor = window.Data.getColorFromPool(item.groupKey % window.Data.colorPoolRgba.length, 1.0);
                if (eventItemTemplate)
                    eventItemTemplate.winControl.render(item.data, container);
                return container;
            });
        },

        groupTemplateFunction: function (itemPromise) {
            return itemPromise.then(function (item) {
                var groupItemTemplate = document.getElementById("group-item-template");
                var container = document.createElement("div");
                container.style.backgroundColor = window.Data.getColorFromPool(item.key % 8, 1.0);
                if (groupItemTemplate)
                    groupItemTemplate.winControl.render(item.data, container);
                return container;
            });
        },

        unload: function () {
            window.Solvberget.Queue.CancelQueue('events');
        }

    });
})();