angular.module('Solvberget.WebApp')
    .filter('translate', function() {

        var dictionary = {
            'AudioBook' : 'Lydbok',
            'Blog' : 'Blogg',
            'Book' : 'Bok',
            'Game' : 'Spill',
            'Journal' : 'Journal',
            'LanguageCourse' : 'Språkkurs',
            'Other' : 'Annen media',
            'SheetMusic' : 'Notehefte'
        };

    return function(input) {

        var output = dictionary[input];
        if(!output) output = input;

        return output;
    }
}).filter('trustAsHtml', function($sce) {
    return function(val) {
        return $sce.trustAsHtml(val);
    };
});
