(function () {

    var ajaxGetDocumentImage = function(query) {
        var url = window.Data.serverBaseUrl + "/Document/GetDocumentImage/";
        return $.getJSON(url + query);
    };
    var ajaxGetThumbnailDocumentImage = function(query, size) {
        var url = window.Data.serverBaseUrl + "Document/GetDocumentImage/";
        var thumbUrl = size == undefined ? url + query : url + query + "/" + size;
        return $.getJSON(thumbUrl);

    };
    WinJS.Namespace.define("Solvberget.DocumentImage", {
        get: ajaxGetDocumentImage,
        getThumbnail: ajaxGetThumbnailDocumentImage
    });
})();

