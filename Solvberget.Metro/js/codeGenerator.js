(function () {
    "use strict";

    var foo = function (document) {
        var results = [];
        var i = 0;
        var stopwords = [];
        stopwords["Isbn"] = 1;
        stopwords["Ean"] = 1;
        stopwords["Issn"] = 1;
        stopwords["Organization"] = 1;
        stopwords["DocType"] = 1;
        stopwords["Title"] = 1;
        stopwords["DocumentNumber"] = 1;
        stopwords["DocumentType"] = 1;
        stopwords["ThumbnailUrl"] = 1;
        stopwords["ThumbnailUrl"] = 1;
        stopwords["ImageUrl"] = 1;
        
        var stringVerdi = "";
        for (var property in document) {
            var propertyValue = null;
            var propertyName = null;


            propertyValue = eval("document." + property);
            propertyName = Solvberget.Localization.getString(property);
            var temp;
            if (stopwords[property] != 1) {
                temp = "<div class=fact-name data-win-res=\"{textContent: '" + property + "'}\"></div>\n" + "<div class=fact-value" + " data-win-bind=\"innerText:" + property + "></div>";
                temp = "  <div class=\"fact\" data-win-bind=\"style.display: " + property + " DocumentDetailConverters.hideNullOrEmptyConverter\">" + "\n" + temp;
                temp = temp + "\n" + "</div>";
                stringVerdi += temp;
                console.log(temp);
            }else {
                temp = "<div class=fact-name data-win-res=\"{textContent: '" + property + "'}\"></div>\n" + "<div class=fact-value" + " data-win-bind=\"innerText:" + property + "></div>";
                temp = "  <div class=\"more-fact\" style=\"display:none\">" + "\n" + temp;
                temp = temp + "\n" + "</div>";
                stringVerdi += temp;
                console.log(temp);
            }

        }
        return stringVerdi;
    };

    WinJS.Namespace.define("CodeGenerator", {

        documentToFactsHTML: foo



    });
})();
