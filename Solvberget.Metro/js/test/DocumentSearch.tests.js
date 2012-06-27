describe('SuggestionListTest', function () {
    it('Should return an array containing at least Harry Potter', function () {
        window.suggestionMethods.getSuggestionListFromServer();

        var suggestionList = window.suggestionMethods.suggestionList;
        var book = 'harry potter';
        suggestionList.should.contain(book);
    });
});