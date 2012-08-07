(function () {
    "use strict";

    var appModel = Windows.ApplicationModel;
    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;
    var utils = WinJS.Utilities;
    var searchPageURI = "/pages/searchResults/searchResults.html";

    var suggestionMethods = {
        suggestionList: [],
        url: window.Data.serverBaseUrl + "/Document/SuggestionList/",
        populateSuggestionList: function (allData) {
            suggestionMethods.suggestionList = allData;
        },
        getSuggestionListFromServer: function () {
            $.getJSON(suggestionMethods.url, suggestionMethods.populateSuggestionList);
        },
        updateSuggestions: function (query) {

            // Reset suggestion
            suggestionMethods.didYouMean = "";
            suggestionMethods.suggestionQuery = "";
            var suggestionText = document.getElementById("spanDidYouMean");

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

                    // Fade in the suggestion
                    $(spanDidYouMean).hide();
                    if (spanDidYouMean != null)
                        WinJS.Binding.processAll(spanDidYouMean, suggestionMethods);

                    setTimeout(function () {
                        if ($(spanDidYouMean).text() !== "undefined")
                            $(spanDidYouMean).fadeIn(250);
                    }, 1600);
                }
            });

        },
        didYouMean: "",
        suggestionQuery: "",

    };

    var loadingWheel = {

        spin: function () {

            $("#search-loading-wheel").css("visibility", "visible");


        },
        stop: function () {

            $("#search-loading-wheel").css("display", "none").css("visibility", "none");
            $("#resultslist").css("display", "block").css("visibility", "visible").hide().fadeIn(500);


        },
    };


    var ajaxSearchDocuments = function (query) {
        return $.getJSON(window.Data.serverBaseUrl + "/Document/Search/" + query);
    };

    var ajaxGetThumbnailDocumentImage = function (query, size) {
        var url = window.Data.serverBaseUrl + "/Document/GetDocumentThumbnailImage/";
        return $.getJSON(size == undefined ? url + query : url + query + "/" + size);
    };

    var lookupDict = function (query) {
        return $.getJSON(window.Data.serverBaseUrl + "/Document/SpellingDictionaryLookup", { value: query });
    };

    var getImageQueue = {
        queue: [],
        working: false,
        inSearchPage: false,
        numInProgress: 0,
        fireFinished: function () {

            this.working = false;
            this.numInProgress = this.numInProgress - 1;
                this.startWorking();

        },
        addToQueue: function (item, index) {

            this.queue.push({ item: item, index: index });
                this.startWorking();

        },
        startWorking: function () {
            if (!this.working && this.inSearchPage && this.queue[0] !== undefined) {
                if (this.numInProgress == 5) {
                    this.working = true;
                }

                this.numInProgress = this.numInProgress + 1;


                var itemIndexObj = this.queue[0];
                this.queue.shift();
                if (itemIndexObj != undefined)
                    self.getAndSetThumbImage(itemIndexObj.item, itemIndexObj.index);
            }
            else {
                if (this.numInProgress < 1) {
                    this.working = false;
                    //if (this.inSearchPage)
                    //    this.startWorking();
                }
            }
        }
    };

    var self;

    ui.Pages.define(searchPageURI, {
        filters: [],
        lastSearch: "",

        generateFilters: function () {

            this.filters = [];

            this.filters.push({ results: null, text: "Alle", predicate: function (item) { return true; } });
            this.filters.push({ results: null, text: "Bøker", predicate: function (item) { return item.DocType == "Book"; } });
            this.filters.push({ results: null, text: "Filmer", predicate: function (item) { return item.DocType == "Film"; } });
            this.filters.push({ results: null, text: "Lydbøker", predicate: function (item) { return item.DocType == "AudioBook"; } });
            this.filters.push({ results: null, text: "CDer", predicate: function (item) { return item.DocType == "Cd"; } });
            this.filters.push({ results: null, text: "Språkkurs", predicate: function (item) { return item.DocType == "LanguageCourse"; } });
            this.filters.push({ results: null, text: "Tidsskrift", predicate: function (item) { return item.DocType == "Journal"; } });
            this.filters.push({ results: null, text: "Noter", predicate: function (item) { return item.DocType == "SheetMusic"; } });
            this.filters.push({ results: null, text: "Spill", predicate: function (item) { return item.DocType == "Game"; } });
            this.filters.push({ results: null, text: "Annet", predicate: function (item) { return item.DocType == "Document"; } });

        },

        itemInvoked: function (args) {

            var itemIndex = args.detail.itemIndex;
            var listView = document.body.querySelector(".resultslist").winControl;
            if (listView) {
                args.detail.itemPromise.done(function itemInvoked(item) {
                    var model = item.data;
                    nav.navigate("/pages/documentDetail/documentDetail.html", { documentModel: model });

                });

            }
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

        // This function executes each step required to perform a search.
        handleQuery: function (element, args) {
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

            suggestionMethods.updateSuggestions(args.queryText);

            // Perform the search
            $.when(ajaxSearchDocuments(args.queryText))
               .then($.proxy(function (response) {

                   var originalResults = new WinJS.Binding.List();

                   for (x in response) {

                       if (response[x].ThumbnailUrl !== "") {
                           response[x].BackgroundImage = response[x].ThumbnailUrl;
                       }
                       else {
                           if (response[x].DocType == "Film" && response[x].TypeOfMedia == "Blu-ray") {
                               response[x].BackgroundImage = "images/placeholders/Blu-ray.png";
                           }
                           else if (response[x].DocType == "Film" && response[x].TypeOfMedia == "3D") {
                               response[x].BackgroundImage = "images/placeholders/3D.png";
                            }
                           else {
                               response[x].BackgroundImage = "images/placeholders/" + response[x].DocType + ".png";
                           }
                       }

                       originalResults.push(response[x]);
                   }

                   this.populateFilterBar(element, originalResults);
                   this.applyFilter(this.filters[0], originalResults);
                   loadingWheel.stop();

                   for (var x in response) {
                       if (!response[x].ThumbnailUrl || response[x].ThumbnailUrl == "")
                           getImageQueue.addToQueue(originalResults.getItem(x), x);
                   }

               }, this)
            );
        },
        getAndSetThumbImage: function (item, index) {


            $.when(ajaxGetThumbnailDocumentImage(item.data.DocumentNumber))
            .then($.proxy(function (response) {

                if (response && response != "") {
                    // Set the new value in the model of this item                   
                    item.data.BackgroundImage = response;

                    // Get the live DOM-object of this item
                    var section = document.getElementById("searchResultSection");
                    if (section) {
                        var listView = section.querySelector(".resultslist").winControl;
                        var htmlItem = listView.elementFromIndex(index);
                        if (htmlItem != null)
                            WinJS.Binding.processAll(htmlItem, item.data);
                    }
                }
                if (getImageQueue.inSearchPage)
                    getImageQueue.fireFinished();


            }, this),
            function () {
                if (getImageQueue.inSearchPage)
                    getImageQueue.fireFinished();
            })


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

            if (source.DocType != undefined && sourceProperties[0] == "MainResponsible") {
                var text = source[sourceProperties[0]];

                if (text != undefined) {
                    if (text.Name != undefined) {
                        var regex = new RegExp(this.lastSearch, "gi");
                        dest[destProperties[0]] = text.Name.replace(regex, "<mark>$&</mark>");
                    } else if (text.Name == undefined && text.Role == undefined && text.LivingYears == undefined && text.Nationality == undefined && text.ReferredWork == undefined) {
                        var regex = new RegExp(this.lastSearch, "gi");
                        dest[destProperties[0]] = text.replace(regex, "<mark>$&</mark>");
                    }
                }
            } else {
                var text = source[sourceProperties[0]];
                if (text != undefined) {
                    var regex = new RegExp(this.lastSearch, "gi");
                    dest[destProperties[0]] = text.replace(regex, "<mark>$&</mark>");
                }
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

            self = this;
            getImageQueue.inSearchPage = true;

            var listView = element.querySelector(".resultslist").winControl;
            listView.itemTemplate = element.querySelector(".itemtemplate");
            listView.oniteminvoked = this.itemInvoked;
            this.handleQuery(element, options);
            listView.element.focus();

        },

        unload: function () {

            getImageQueue.inSearchPage = false;
            getImageQueue.queue = [];
            getImageQueue.numInProgress = 0;
            getImageQueue.working = false;

        },

        // This function updates the page layout in response to viewState changes.
        updateLayout: function (element, viewState, lastViewState) {

            var listView = element.querySelector(".resultslist").winControl;
            if (lastViewState !== viewState) {
                if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                    var handler = function (e) {
                        listView.removeEventListener("contentanimating", handler, false);
                        e.preventDefault();
                    };
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
                    nav.history.current = { location: Application.navigator.home, initialState: {} };
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
