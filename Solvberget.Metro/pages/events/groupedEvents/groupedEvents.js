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

        initializeLayout: function (listView, viewState) {
            if (viewState === appViewState.snapped) {
                listView.itemTemplate = this.groupTemplateFunction;
                listView.itemDataSource = EventData.groups.dataSource;
                listView.groupDataSource = null;
                listView.layout = new ui.ListLayout();
            } else {
                listView.itemTemplate = this.eventTemplateFunction;
                listView.itemDataSource = EventData.items.dataSource;
                listView.groupDataSource = EventData.groups.dataSource;
                listView.layout = new ui.GridLayout({ groupHeaderPosition: "top" });
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
                                listView.removeEventListener("contentanimating", handler, false);
                                e.preventDefault();
                            };
                            listView.addEventListener("contentanimating", handler, false);
                            this.initializeLayout(listViewWinControl, viewState, element);
                        }
                    }
            }
        },

        eventTemplateFunction: function (itemPromise) {
            return itemPromise.then(function (item) {
                var eventItemTemplate = document.getElementById("event-item-template");
                var container = document.createElement("div");
                container.style.backgroundColor = window.Data.getColorFromPool(item.groupKey % window.Data.colorPoolRgba.length, 0.45);
                if (eventItemTemplate)
                    eventItemTemplate.winControl.render(item.data, container);
                return container;
            });
        },

        groupTemplateFunction: function (itemPromise) {
            return itemPromise.then(function (item) {
                var groupItemTemplate = document.getElementById("group-item-template");
                var container = document.createElement("div");
                container.style.backgroundColor = window.Data.getColorFromPool(item.key % window.Data.colorPoolRgba.length, 0.6);
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