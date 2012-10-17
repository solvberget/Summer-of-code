(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;

    ui.Pages.define("/pages/events/eventGroupDetail/eventGroupDetail.html", {

        items: null,

        initializeLayout: function (listViewWinControl, viewState) {
            if (viewState === appViewState.snapped) {
                listViewWinControl.layout = new ui.ListLayout();
            } else {
                listViewWinControl.layout = new ui.GridLayout();
            }
            listViewWinControl.forceLayout();
        },

        itemInvoked: function (args) {
            if (this.items) {
                var item = this.items.getAt(args.detail.itemIndex);
                nav.navigate("/pages/events/eventDetail/eventDetail.html", { item: EventData.getItemReference(item) });
            }
        },

        ready: function (element, options) {
            var listView = element.querySelector(".eventItemsListView");
            if (listView) {
                var listViewWinControl = listView.winControl;
                if (listViewWinControl) {
                    var group = (options && options.groupKey) ? EventData.resolveGroupReference(options.groupKey) : EventData.groups.getAt(0);
                    this.items = EventData.getItemsFromGroup(group);
                    var pageList = this.items.createGrouped(
                        function groupKeySelector(item) { return group.key; },
                        function groupDataSelector(item) { return group; }
                    );
                    element.querySelector(".titlearea .pagetitle").textContent = group.Name;
                    listViewWinControl.itemDataSource = pageList.dataSource;
                    listViewWinControl.itemTemplate = element.querySelector(".event-item-template");
                    listViewWinControl.oniteminvoked = this.itemInvoked.bind(this);
                    this.initializeLayout(listViewWinControl, Windows.UI.ViewManagement.ApplicationView.value);
                    listViewWinControl.element.focus();
                }
            }
        },

        unload: function () {
            if (this.items) this.items.dispose();
        },

        updateLayout: function (element, viewState, lastViewState) {
            var listView = element.querySelector(".eventItemsListView");
            if (listView) {
                var listViewWinControl = listView.winControl;
                if (listViewWinControl) {
                    if (lastViewState !== viewState) {
                        if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                            var handler = function (e) {
                                listViewWinControl.removeEventListener("contentanimating", handler, false);
                                e.preventDefault();
                            };
                            listViewWinControl.addEventListener("contentanimating", handler, false);
                            var firstVisible = listViewWinControl.indexOfFirstVisible;
                            this.initializeLayout(listViewWinControl, viewState);
                            listViewWinControl.indexOfFirstVisible = firstVisible;
                        }
                    }
                }
            }
        }
    });

})();
