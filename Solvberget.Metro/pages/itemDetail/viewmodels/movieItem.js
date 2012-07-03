(function () {
    "use strict";
    WinJS.Namespace.define("ViewModel", {
        Movie: WinJS.Binding.as({
            title: null,
            subtitle: null,
            content: null,
            ///book specific
            actors: null,
            agelimit: null,
            genre: null,
            image: null,
            fillProperties: function (bookItem) {
                this.title = bookItem.Title;
                this.subtitle = bookItem.PublishedYear;
                this.image = "/images/dummydata/hp1.jpeg";
                this.content = "Lorem ipsum dolor sit amet";
                this.agelimit = bookItem.AgeLimit;
                this.actors = bookItem.Actors;
                this.genre = bookItem.Genre.toString();
                
            },
        }),

        MovieList: WinJS.Binding.as({
            movieItems: [],
            ///item specific

            addMovie: function (newMovie) {
                this.movieItems.push(newMovie);
            },
        })
    });
})();