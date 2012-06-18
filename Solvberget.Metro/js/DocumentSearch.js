function Document(data) {
    this.title = ko.observable(data.Title);
    //this.test = ko.observable("Test");
}


function DocumentListViewModel() {
    var self = this;
    self.documents = ko.observableArray([]);
    self.test = ko.observable("Test");

    $.getJSON("http://localhost:7089/Document/Search/Naiv super", function(allData) {
        var mappedDocuments = $.map(allData, function(item) { return new Document(item) });
        self.documents(mappedDocuments);
    });
}

ko.applyBindings(new DocumentListViewModel());