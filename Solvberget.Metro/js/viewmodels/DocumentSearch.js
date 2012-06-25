function DocumentListViewModel() {
    var self = this;
    self.documents = ko.observableArray([]);
    self.searchString = ko.observable("Input search");
    self.suggestions = ko.observableArray([]);
    self.suggestion = ko.observable("Ingen forslag");

    self.search = function () {
        var url = "http://localhost:7089/Document/Search/" + self.searchString();
        $.getJSON(url, self.populate);
    };

    self.populate = function (allData) {
        var mappedDocuments = $.map(allData, function (item) {
            return new Document(item);
        });
        self.documents(mappedDocuments);
        
    };

    self.lookup = function() {
        var url = "http://localhost:7089/Document/Lookup/" + self.searchString();
        $.getJSON(url, self.suggest);
    };

    self.suggest = function(allData) {
        var mappedLinks = $.map(allData, function(item) {
            return new Link(item);
        });
        self.suggestions(mappedLinks);
        self.suggestion = suggestion[0];
    };

}

ko.applyBindings(new DocumentListViewModel());
