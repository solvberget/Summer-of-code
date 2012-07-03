﻿// For an introduction to the Search Contract template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232512

// TODO: Add the following script tag to the start page's head to
// subscribe to search contract events.
//  
// <script src="/js/viewmodels/searchResults.js"></script>

(function () {
    "use strict";

    var appModel = Windows.ApplicationModel;
    var appViewState = Windows.UI.ViewManagement.ApplicationViewState
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;
    var searchPageURI = "/pages/searchResults/searchResults.html";
    var suggestionMethods = {
        suggestionList: [],
        url : "http://localhost:7089/Document/SuggestionList/",
        populateSuggestionList : function (allData) {
            suggestionMethods.suggestionList = allData;
        },
        getSuggestionListFromServer: function () {
            $.getJSON(suggestionMethods.url, suggestionMethods.populateSuggestionList);
        },
        didYouMean: "",
        suggestionQuery : "",

    };

    var spellingMethods = {

    }

    var loadingWheel = {
        opts: {
            lines: 17, // The number of lines to draw
            length: 23, // The length of each line
            width: 5, // The line thickness
            radius: 40, // The radius of the inner circle
            rotate: 13, // The rotation offset
            color: '#FFF', // #rgb or #rrggbb
            speed: 1.1, // Rounds per second
            trail: 86, // Afterglow percentage
            shadow: true, // Whether to render a shadow
            hwaccel: false, // Whether to use hardware acceleration
            className: 'spinner', // The CSS class to assign to the spinner
            zIndex: 2e9, // The z-index (defaults to 2000000000)
            top: 'auto', // Top position relative to parent in px
            left: 'auto' // Left position relative to parent in px
        },
        spinner: null,
        spin: function () {

                var target = document.getElementById('search-loading-wheel');
                loadingWheel.spinner = new Spinner(loadingWheel.opts).spin();
                target.appendChild(loadingWheel.spinner.el);
                loadingWheel.initialized = true;

        },
        stop: function () {

            loadingWheel.spinner.stop();

        },
    };


    var ajaxSearchDocuments = function (query) {
        return $.getJSON("http://localhost:7089/Document/Search/" + query); 
    }
    var lookupDict = function (query) {
        // Does not work, do not return the json promise
        return $.getJSON("http://localhost:7089/Document/SpellingDictionaryLookup", { value: query });
    }

    ui.Pages.define(searchPageURI, {
        /// <field elementType="Object" />
        filters: [],
        lastSearch: "",

        generateFilters: function () {
            this.filters = [];
            this.filters.push({ results: null, text: "Alle", predicate: function (item) { return true; } });

            // TODO: Replace or remove example filters.
            this.filters.push({ results: null, text: "Bok", predicate: function (item) { return item.DocType == "Book"; } });
            this.filters.push({ results: null, text: "Film", predicate: function (item) { return item.DocType == "Film" } });
            this.filters.push({ results: null, text: "Lydbok", predicate: function (item) { return item.DocType == "AudioBook" } });
            this.filters.push({ results: null, text: "Annet", predicate: function (item) { return item.DocType == "Document" } });
        },

        itemInvoked: function (args) {
            args.detail.itemPromise.done(function itemInvoked(item) {
                // TODO: Navigate to the item that was invoked.
                var itemObject = args.detail.itemPromise._value.data;
                     nav.navigate("/pages/itemDetail/itemDetail.html", { item: itemObject, key: itemObject.DocumentNumber });
            });
        },

        // This function populates a WinJS.Binding.List with search results for the
        // provided query.
        searchData: function (queryText) {

            var originalResults;
            var regex;
            
        },

        // This function filters the search data using the specified filter.
        applyFilter: function (filter, originalResults) {
            if (filter.results === null) {
                filter.results = originalResults.createFiltered(filter.predicate);
            }
            return filter.results;
        },

        // This function responds to a user selecting a new filter. It updates the
        // selection list and the displayed results.
        filterChanged: function (element, filterIndex) {
            var filterBar = element.querySelector(".filterbar");
            var listView = element.querySelector(".resultslist").winControl;

            utils.removeClass(filterBar.querySelector(".highlight"), "highlight");
            utils.addClass(filterBar.childNodes[filterIndex], "highlight");

            element.querySelector(".filterselect").selectedIndex = filterIndex;

            listView.itemDataSource = this.filters[filterIndex].results.dataSource;
        },
        updateSuggestions : function( query ) {

            // Reset suggestion
            suggestionMethods.didYouMean = "";
            suggestionMethods.suggestionQuery = "";


            // Get a new search-suggestion
            $.getJSON("http://localhost:7089/Document/SpellingDictionaryLookup", { value: query }, function (allData) {

                // Check to see if we have a suggestion
                if (query != allData && allData != "") {

                    suggestionMethods.didYouMean = allData;
                    suggestionMethods.suggestionQuery = allData;

                    var bindingSource = WinJS.Binding.as(suggestionMethods);
                    bindingSource.didYouMean = "Mente du " + suggestionMethods.didYouMean + "?";

                    var spanDidYouMean = document.getElementById("spanDidYouMean");

                    $(spanDidYouMean).click(function () {

                        var searchPane = Windows.ApplicationModel.Search.SearchPane.getForCurrentView();
                        searchPane.show(suggestionMethods.suggestionQuery);

                    });

                    WinJS.Binding.processAll(spanDidYouMean, suggestionMethods);

                }
            });

        },
        

        // This function executes each step required to perform a search.
        handleQuery: function (element, args) {
            var originalResults;
            this.lastSearch = args.queryText;
            WinJS.Namespace.define("searchResults", { markText: this.markText.bind(this) });
            utils.markSupportedForProcessing(searchResults.markText);
            this.initializeLayout(element.querySelector(".resultslist").winControl, Windows.UI.ViewManagement.ApplicationView.value);
            this.generateFilters();

            // Hide search pane ** Not implemented by Microsoft yet **
                //var searchPane = Windows.ApplicationModel.Search.SearchPane.getForCurrentView();
                //searchPane.hide(); ** Not implemented by Microsoft yet **

            // Show loadingWheel
            loadingWheel.spin();

            this.updateSuggestions(args.queryText);
            
            $.when(ajaxSearchDocuments(args.queryText))
               .then($.proxy(function (response) {

                   var originalResults = new WinJS.Binding.List();

                   for (var x in response) {
                       originalResults.push(response[x]);
                   }

                   this.populateFilterBar(element, originalResults);
                   this.applyFilter(this.filters[0], originalResults);
                   loadingWheel.stop();

               }, this)
            );
            
        },

        // This function updates the ListView with new layouts
        initializeLayout: function (listView, viewState) {
            /// <param name="listView" value="WinJS.UI.ListView.prototype" />

            var modernQuotationMark = "&#148;";
            if (viewState === appViewState.snapped) {
                listView.layout = new ui.ListLayout();
                document.querySelector(".titlearea .pagetitle").innerHTML = modernQuotationMark + toStaticHTML(this.lastSearch) + modernQuotationMark;
                document.querySelector(".titlearea .pagesubtitle").innerHTML = "";
            } else {
                listView.layout = new ui.GridLayout();
                document.querySelector(".titlearea .pagetitle").innerHTML = "Søk";
                document.querySelector(".titlearea .pagesubtitle").innerHTML = "Resultater for " + modernQuotationMark + toStaticHTML(this.lastSearch) + modernQuotationMark;
            }
        },

        // This function colors the search term. Referenced in /js/viewmodels/searchResults.html
        // as part of the ListView item templates.
        markText: function (source, sourceProperties, dest, destProperties) {

            if (source.AgeLimit != undefined && sourceProperties[0] == "AgeLimit") {

                var text = source[sourceProperties[0]];
                var regex = new RegExp(this.lastSearch, "gi");
                dest[destProperties[0]] = text.replace(regex, "<mark>$&</mark>");

            }
            else if (sourceProperties[0] != "AgeLimit") {
                var text = source[sourceProperties[0]];
                var regex = new RegExp(this.lastSearch, "gi");
                dest[destProperties[0]] = text.replace(regex, "<mark>$&</mark>");

            }
        },

        // This function generates the filter selection list.
        populateFilterBar: function (element, originalResults) {
            var filterBar = element.querySelector(".filterbar");
            if (element.querySelector(".resultslist") != undefined) {
                var listView = element.querySelector(".resultslist").winControl;

                var li, option, filterIndex;

                filterBar.innerHTML = "";
                for (filterIndex = 0; filterIndex < this.filters.length; filterIndex++) {
                    this.applyFilter(this.filters[filterIndex], originalResults);

                    li = document.createElement("li");
                    li.filterIndex = filterIndex;
                    li.tabIndex = 0;
                    li.textContent = this.filters[filterIndex].text + " (" + this.filters[filterIndex].results.length + ")";
                    li.onclick = function (args) { this.filterChanged(element, args.target.filterIndex); }.bind(this);
                    li.onkeyup = function (args) {
                        if (args.key === "Enter" || args.key === "Spacebar")
                            this.filterChanged(element, args.target.filterIndex);
                    }.bind(this);
                    filterBar.appendChild(li);

                    if (filterIndex === 0) {
                        utils.addClass(li, "highlight");
                        listView.itemDataSource = this.filters[filterIndex].results.dataSource;
                    }

                    option = document.createElement("option");
                    option.value = filterIndex;
                    option.textContent = this.filters[filterIndex].text + " (" + this.filters[filterIndex].results.length + ")";
                    element.querySelector(".filterselect").appendChild(option);
                }

                element.querySelector(".filterselect").onchange = function (args) { this.filterChanged(element, args.currentTarget.value); }.bind(this);
            }
        },

        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {

            var listView = element.querySelector(".resultslist").winControl;
            listView.itemTemplate = element.querySelector(".itemtemplate")
            listView.oniteminvoked = this.itemInvoked;
            this.handleQuery(element, options);
            listView.element.focus();

            var spanDidYouMean = document.getElementById("spanDidYouMean");
            WinJS.Binding.processAll(spanDidYouMean, suggestionMethods);


        },

        // This function updates the page layout in response to viewState changes.
        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />
            /// <param name="viewState" value="Windows.UI.ViewManagement.ApplicationViewState" />
            /// <param name="lastViewState" value="Windows.UI.ViewManagement.ApplicationViewState" />

            var listView = element.querySelector(".resultslist").winControl;
            if (lastViewState !== viewState) {
                if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                    var handler = function (e) {
                        listView.removeEventListener("contentanimating", handler, false);
                        e.preventDefault();
                    }
                    listView.addEventListener("contentanimating", handler, false);
                    var firstVisible = listView.indexOfFirstVisible;
                    this.initializeLayout(listView, viewState);
                    listView.indexOfFirstVisible = firstVisible;
                }
            }
        }
    });

    WinJS.Application.addEventListener("activated", function (args) {
        if (args.detail.kind === appModel.Activation.ActivationKind.search) {
            args.setPromise(ui.processAll().then(function () {
                if (!nav.location) {
                   //nav.history.current = { location: Application.navigator.home, initialState: {} };
                }

                return nav.navigate(searchPageURI, { queryText: args.detail.queryText });
            }));
        }
    });

    appModel.Search.SearchPane.getForCurrentView().onquerysubmitted = function (args) { nav.navigate(searchPageURI, args); };
    
    // Populate suggestionList from server
    suggestionMethods.getSuggestionListFromServer();




    Windows.ApplicationModel.Search.SearchPane.getForCurrentView().onsuggestionsrequested = function (eventObject) {
        var queryText = eventObject.queryText, suggestionRequest = eventObject.request;
        var query = queryText.toLowerCase();
        var maxNumberOfSuggestions = 5;
                    
        // Suggestion based on content

        for (var i = 0, len = suggestionMethods.suggestionList.length; i < len; i++) {
            if (suggestionMethods.suggestionList[i].substr(0, query.length).toLowerCase() === query) {
                suggestionRequest.searchSuggestionCollection.appendQuerySuggestion(suggestionMethods.suggestionList[i]);
                if (suggestionRequest.searchSuggestionCollection.size === maxNumberOfSuggestions) {
                    break;
                }
            }
        }

        if (suggestionRequest.searchSuggestionCollection.size > 0) {
            WinJS.log && WinJS.log("Suggestions provided for query: " + queryText, "sample", "status");
        } else {
            WinJS.log && WinJS.log("No suggestions provided for query: " + queryText, "sample", "status");
        }
    };
    

})();
