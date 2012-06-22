function DocumentListViewModel() {
    var self = this;
    self.documents = ko.observableArray([]);
    self.searchString = ko.observable("Input search");

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
}

ko.applyBindings(new DocumentListViewModel());
