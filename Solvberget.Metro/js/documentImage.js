(function () {

    var ajaxGetDocumentImage = function(query) {
        var url = "http://localhost:7089/Document/GetDocumentImage/";
        return $.getJSON(url + query);
    };
    var ajaxGetThumbnailDocumentImage = function(query, size) {
        var url = "http://localhost:7089/Document/GetDocumentImage/";
        var thumbUrl = size == undefined ? url + query : url + query + "/" + size;
        return $.getJSON(thumbUrl);

    };
    WinJS.Namespace.define("Solvberget.DocumentImage", {
        get: ajaxGetDocumentImage,
        getThumbnail: ajaxGetThumbnailDocumentImage
    });
})();

