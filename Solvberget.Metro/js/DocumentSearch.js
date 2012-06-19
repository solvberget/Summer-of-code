function Document(data) {
    this.title = ko.observable(data.Title);
    this.DocumentNumber = ko.observable(data.DocumentNumber);
    //this.test = ko.observable("Test");
}

function DocumentListViewModel() {
    var self = this;
    self.documents = ko.observableArray([]);
    self.searchString = ko.observable("Input search");

    self.search = function () {
        var url = "http://localhost:7089/Document/Search/" + self.searchString();
        $.getJSON(url, self.populate);
    };

    self.populate = function (allData) {
        var mappedDocuments = $.map(allData, function (item) { return new Document(item) });
        self.documents(mappedDocuments);
    };

}

ko.applyBindings(new DocumentListViewModel());
