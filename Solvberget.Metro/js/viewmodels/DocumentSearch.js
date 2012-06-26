function DocumentListViewModel() {
    var self = this;
    self.documents = ko.observableArray([]);
    self.searchString = ko.observable("Input search");
    self.suggestion = ko.observable("Ingen forslag");

    self.search = function () {
        var url = "http://localhost:7089/Document/Search/" + self.searchString();
        self.lookup();
        $.getJSON(url, self.populate);
    };

    self.searchSuggested = function () {
        console.log("Search suggested with: " + self.suggestion());
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

}

ko.applyBindings(new DocumentListViewModel());
