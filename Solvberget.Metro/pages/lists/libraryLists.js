(function () {
    "use strict";

    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var binding = WinJS.Binding;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;

    ui.Pages.define("/pages/lists/libraryLists.html", {

        group: null,
        itemSelectionIndex: -1,

        // This function checks if the list and details columns should be displayed
        // on separate pages instead of side-by-side.
        isSingleColumn: function () {
            var viewState = Windows.UI.ViewManagement.ApplicationView.value;
            return (viewState === appViewState.snapped || viewState === appViewState.fullScreenPortrait);
        },

        listOfContentItemInvoked: function (eventInfo) {
            var listViewForListContent = document.body.querySelector(".listOfListContent").winControl;
            var details;
            var that = this;
            // By default, the selection is restriced to a single item.
            listViewForListContent.selection.getItems().done(function updateDetails(items) {
                if (items.length > 0) {
                    that.itemSelectionIndex = items[0].index;
                    nav.navigate("/pages/itemDetail/itemDetail.html", { itemModel: items[0].data, key: items[0].data.DocumentNumber });
                }
            });

        },

        itemTemplateFunction: function (itemPromise) {
            return itemPromise.then(function (item) {

                // Select either normal product template or on sale template
                var itemTemplate = document.getElementById("documentTemplate");

                if (item.data.DocType === "Book") {
                    itemTemplate = document.getElementById("bookTemplate");
                }
                else if (item.data.DocType === "Film") {
                    itemTemplate = document.getElementById("filmTemplate");
                }
                else if (item.data.DocType === "AudioBook") {
                    itemTemplate = document.getElementById("audioBookTemplate");
                }

                // Render selected template to DIV container
                var container = document.createElement("div");
                itemTemplate.winControl.render(item.data, container);
                return container;
            });
        },

        updateListViewForContentDataSoruce: function (itemData) {
            var listViewForListContent = document.body.querySelector(".listOfListContent").winControl;
            var docs = new WinJS.Binding.List(itemData.docs);
            listViewForListContent.itemDataSource = docs.dataSource;
        },

        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {

            var listViewForLists = element.querySelector(".listOfLists").winControl;
            var listViewForListContent = element.querySelector(".listOfListContent").winControl;

            //Setup the ListDataSource
            var listDataSource = new DataSources.List.ListDataSource();

            // Store information about the group and selection that this page will
            // display.
            this.group = (options && options.groupKey) ? Data.resolveGroupReference(options.groupKey) : Data.groups.getAt(0);
            //this.items = null;
            this.itemSelectionIndex = (options && "selectedIndex" in options) ? options.selectedIndex : -1;

            //Set page header
            element.querySelector("header[role=banner] .pagetitle").textContent = this.group.title;


            // Set up the listViewForLists.
            listViewForLists.itemDataSource = listDataSource;
            listViewForLists.itemTemplate = element.querySelector(".listListTemplate");
            listViewForLists.onselectionchanged = this.listOfListsSelectionChanged.bind(this);
            listViewForLists.layout = new ui.ListLayout();
            
            // Set up the listViewForLists.
            listViewForListContent.itemTemplate = this.itemTemplateFunction;
            listViewForListContent.oniteminvoked = this.listOfContentItemInvoked.bind(this);
            listViewForListContent.layout = new ui.ListLayout();

            this.updateVisibility();
            if (this.isSingleColumn()) {
                if (this.itemSelectionIndex >= 0) {
                    // For single-column detail view, load title and list content.
                    binding.processAll(element.querySelector(".articlesection"), options.item);
                    setImmediate(this.updateListViewForContentDataSoruce(options.item));
                }
                else {
                    listViewForLists.selection.set(Math.max(this.itemSelectionIndex, 0));
                }
            } else {
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/lists/libraryLists.html") {
                    // Clean up the backstack to handle a user snapping, navigating
                    // away, unsnapping, and then returning to this page.
                    nav.history.backStack.pop();
                }
                // If this page has a selectionIndex, make that selection
                // appear in the listViewForLists.
                listViewForLists.selection.set(Math.max(this.itemSelectionIndex, 0));
            }

        },

        listOfListsSelectionChanged: function (args) {
            var listViewForLists = document.body.querySelector(".listOfLists").winControl;
            if (listViewForLists != null) {
                var details;
                var that = this;
                // By default, the selection is restriced to a single item.
                listViewForLists.selection.getItems().done(function updateDetails(items) {
                    if (items.length > 0) {
                        that.itemSelectionIndex = items[0].index;
                        if (that.isSingleColumn()) {

                            // If snapped or portrait, navigate to a new page containing the
                            // selected item's details.
                            nav.navigate("/pages/lists/libraryLists.html", { groupKey: that.group.key, selectedIndex: that.itemSelectionIndex, item: items[0].data });

                        } else {

                            // If fullscreen or filled, update the details column with new data.
                            details = document.querySelector(".article-title");
                            binding.processAll(details, items[0].data);
                            //details.scrollTop = 0;

                            //Update list content
                            setImmediate(that.updateListViewForContentDataSoruce(items[0].data));

                        }
                    }
                });
            }
        },

        unload: function () {
            //this.items.dispose();
        },

        // This function updates the page layout in response to viewState changes.
        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />
            /// <param name="viewState" value="Windows.UI.ViewManagement.ApplicationViewState" />
            /// <param name="lastViewState" value="Windows.UI.ViewManagement.ApplicationViewState" />

            var listViewForLists = element.querySelector(".listOfLists").winControl;
            var firstVisible = listViewForLists.indexOfFirstVisible;
            this.updateVisibility();

            var handler = function (e) {
                listViewForLists.removeEventListener("contentanimating", handler, false);
                e.preventDefault();
            }

            if (this.isSingleColumn()) {
                listViewForLists.selection.clear();
                if (this.itemSelectionIndex >= 0) {
                    // If the app has snapped into a single-column detail view,
                    // add the single-column list view to the backstack.
                    nav.history.current.state = {
                        groupKey: this.group.key,
                        selectedIndex: this.itemSelectionIndex
                    };
                    nav.history.backStack.push({
                        location: "/pages/lists/libraryLists.html",
                        state: { groupKey: this.group.key }
                    });
                    element.querySelector(".articlesection").focus();


                } else {
                    listViewForLists.addEventListener("contentanimating", handler, false);
                    listViewForLists.indexOfFirstVisible = firstVisible;
                    listViewForLists.forceLayout();
                }
            } else {
                // If the app has unsnapped into the two-column view, remove any
                // splitPage instances that got added to the backstack.
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/lists/libraryLists.html") {
                    nav.history.backStack.pop();
                }
                if (viewState !== lastViewState) {
                    listViewForLists.addEventListener("contentanimating", handler, false);
                    listViewForLists.indexOfFirstVisible = firstVisible;
                    listViewForLists.forceLayout();
                }

                listViewForLists.selection.set(this.itemSelectionIndex >= 0 ? this.itemSelectionIndex : Math.max(firstVisible, 0));

            }
        },

        // This function toggles visibility of the two columns based on the current
        // view state and item selection.
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
                  utils.addClass(document.querySelector(".listOfListsSection"), "primarycolumn");
                    document.querySelector(".listOfLists").focus();
                }
            } else {
                document.querySelector(".listOfLists").focus();
            }
        }

    });
})();