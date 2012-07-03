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
            fragmentPath: null,
            fillProperties: function (movieItem) {
                this.title = movieItem.Title;
                this.subtitle = movieItem.PublishedYear;
                this.image = "/images/dummydata/hp1.jpeg";
                this.content = "Lorem ipsum dolor sit amet";
                this.agelimit = movieItem.AgeLimit;
                this.actors = movieItem.Actors;
                this.genre = movieItem.Genre.toString();
                
            },
        }),
    });
})();