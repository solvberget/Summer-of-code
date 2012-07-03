(function () {
    "use strict";
    WinJS.Namespace.define("ViewModel", {
        Book: WinJS.Binding.as({
            title: null,
            subtitle: null,
            content: null,
            ///book specific
            author: null,
            published: null,
            isbn: null,
            genre: null,
            image: null,
            fragmentPath: null,
            fillProperties: function (bookItem) {
                this.title = bookItem.Title;
                this.subtitle = bookItem.PublishedYear;
                this.image = "ms-appx:///images/dummydata/hp1.jpeg";
                this.content = "Lorem ipsum dolor sit amet";
                this.author = bookItem.Author.Name;
                this.published = bookItem.PublishedYear;
                this.isbn = bookItem.Isbn;
                this.genre = bookItem.Genre;
            },
        }),

 
    });
})();