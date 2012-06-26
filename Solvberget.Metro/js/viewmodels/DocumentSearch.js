function DocumentListViewModel() {
    var self = this;
    self.documents = ko.observableArray([]);
    self.searchString = ko.observable("");
    self.suggestion = ko.observable("");
    self.suggestionLink = ko.observable("");

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
        console.log("Return: "+allData);
        self.suggestion(allData);
    };

    this.suggestionLink = ko.computed(function () {
        // Knockout tracks dependencies automatically. It knows that fullName depends on firstName and lastName, because these get called when evaluating fullName.
        return "Mente du " + this.suggestion() + "?";
    }, this);
}

ko.applyBindings(new DocumentListViewModel());
