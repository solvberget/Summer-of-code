function Document(data) {
    this.title = ko.observable(data.Title);
    this.DocumentType = ko.observable(data.DocumentType);
}