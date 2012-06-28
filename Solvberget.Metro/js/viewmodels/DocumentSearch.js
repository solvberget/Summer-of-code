/**
function DocumentListViewModel() {
    var self = this;
    self.documents = ko.observableArray([]);
    self.searchString = ko.observable("");
    self.suggestion = ko.observable("");
    self.suggestionLink = ko.observable("");
    self.displaySearchSuggestion = ko.observable(false);

    self.search = function () {
        var url = "http://localhost:7089/Document/Search/" + self.searchString();
        self.lookup();
        self.showLoadingWheel(true);
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
        self.showLoadingWheel(false);
       
    };

    self.lookup = function () {

        var url = "http://localhost:7089/Document/SpellingDictionaryLookup/";
        $.getJSON(url, { value : self.searchString() }, self.suggest);

    };


    self.suggest = function (allData) {

        self.suggestion(allData);

        if ( self.searchString() == allData ) 
            self.displaySearchSuggestion(false);
        else 
            self.displaySearchSuggestion(true);

    };

    self.showLoadingWheel = function ( showIt ) {

        if (showIt == undefined || showIt == true) {
            console.log("Loading wheel showing");
            var opts = {
                lines: 15, // The number of lines to draw
                length: 12, // The length of each line
                width: 5, // The line thickness
                radius: 14, // The radius of the inner circle
                rotate: 30, // The rotation offset
                color: '#FFF', // #rgb or #rrggbb
                speed: 1.1, // Rounds per second
                trail: 45, // Afterglow percentage
                shadow: false, // Whether to render a shadow
                hwaccel: false, // Whether to use hardware acceleration
                className: 'spinner', // The CSS class to assign to the spinner
                zIndex: 2e9, // The z-index (defaults to 2000000000)
                top: 'auto', // Top position relative to parent in px
                left: 'auto' // Left position relative to parent in px
            };

            var target = jQuery('#searchField');
            var spinner = new window.Spinner(opts).spin();
            //var spinner = new window.Spinner().spin();
            jQuery("#searchField").css("margin-left","200px");
            jQuery("#searchField").css("margin-top","100px");


            target.append(spinner.el);
        }
        else {
            console.log("Loading wheel removed");
            jQuery('#searchField').html("");
            jQuery("#searchField").css("margin-left", "0px");
            jQuery("#searchField").css("margin-top", "0px");

        }

    }

    self.suggestionLink = ko.computed(function () {
        // Will recompute when suggestion is changed
        return "Mente du " + self.suggestion() + "?";
    }, this);



    Windows.ApplicationModel.Search.SearchPane.getForCurrentView().onquerysubmitted = function (eventObject) {
        self.searchString(eventObject.queryText);
        self.search();
    };

}

ko.applyBindings(new DocumentListViewModel());
*/