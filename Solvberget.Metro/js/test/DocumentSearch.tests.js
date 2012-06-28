describe('SuggestionListTest', function () {
    it('Should return an array containing at least Harry Potter', function () {

        suggestionMethods.getSuggestionListFromServer(done);
        var suggestionList = suggestionMethods.suggestionList;
        var book = 'harry potter';
        ($.inArray(book, suggestionList)).should.be.true;

    });
});