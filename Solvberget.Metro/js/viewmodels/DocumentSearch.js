function DocumentListViewModel() {
    var self = this;
    self.documents = ko.observableArray([]);
    self.searchString = ko.observable("");
    self.suggestion = ko.observable("");
    self.suggestionLink = ko.observable("");
    self.displaySearchSuggestion = ko.observable(false);

    self.search = function () {
        var url = "http://localhost:7089/Document/Search/" + self.searchString();
        self.lookup();
        $.getJSON(url, self.populate);
    };

    self.searchSuggested = function () {

        self.searchString(self.suggestion());
        self.search();

    }

    self.populate = function (allData) {

        var mappedDocuments = $.map(allData, function (item) {
            return new Document(item);
        });
        self.documents(mappedDocuments);
        
    };

    self.lookup = function () {

        var url = "http://localhost:7089/Document/SpellingDictionaryLookup/";
        $.getJSON(url, { value : self.searchString() }, self.suggest);

    };


    self.suggest = function (allData) {

        self.suggestion(allData);

        if ( self.searchString() == allData ) 
            self.displaySearchSuggestion(false);
        else 
            self.displaySearchSuggestion(true);

    };

    this.suggestionLink = ko.computed(function () {
        // Will recompute when suggestion is changed
        return "Mente du " + self.suggestion() + "?";
    }, this);

    Windows.ApplicationModel.Search.SearchPane.getForCurrentView().onquerysubmitted = function (eventObject) {
        self.searchString(eventObject.queryText);
        self.search();
    };

}

ko.applyBindings(new DocumentListViewModel());
