(function () {
    "use strict";

    var foo = function (document) {
        var results = [];
        var i = 0;

        var stringVerdi = "";
        for (var property in document) {
            var propertyValue = null;
            var propertyName = null;

            propertyValue = eval("document." + property);
            propertyName = Solvberget.Localization.getString(property);

            var temp = "<div class=fact-name data-win-res=\"{textContent: '" + property + "'}\"></div>\n" + "<div class=fact-value" + " data-win-bind=\"innerText:" + property + "></div>";
            temp = "  <div class=\"fact\">" + "\n"+ temp ;
            temp = temp +"\n"+ "</div>";
            stringVerdi += temp;
            console.log(temp);


        }
        return stringVerdi;
    };

    WinJS.Namespace.define("CodeGenerator", {

        documentToFactsHTML: foo



    });
})();
