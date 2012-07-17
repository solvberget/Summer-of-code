(function () {
    "use strict";
    WinJS.Namespace.define("ViewModel", {
        propertiesList: {

            documentToPropertiesList: function (document) {
                var results = [], count;
                var i = 0;
                for (var property in document) {
                    var documentValue = null;
                    var documentName = null;

                    documentValue = eval("document." + property);
                    documentName = Solvberget.Localization.getString(property);

                    if (documentValue !== null) {
                        if (documentValue[0] != undefined) {
                            if (documentValue[0].Name != undefined)
                                documentValue = documentValue[0].Name + ",";
                        } else if (documentValue.Name != undefined) {
                            documentValue = documentValue.Name;
                        }
                    }

                    if ((documentValue !== null) && (documentValue !== []) && (documentValue != "")) {
                        var type = typeof (documentValue);

                        while (type !== "string" && type !== "integer") {
                            documentValue = documentValue.toString();
                            var documentValueTemp = "";
                            for (var j = 0; j < documentValue.length; j++) {
                                if (documentValue[j] == ",") {

                                    documentValueTemp += ", ";
                                } else {
                                    documentValueTemp += documentValue[j];
                                }
                            }
                            documentValue = documentValueTemp;

                            type = typeof (documentValue);
                        }
                      
                        results.push(
                        {
                            key: (i).toString(),
                            data:
                            {
                                propertyName: documentName,
                                propertyValue: documentValue,
                            }
                        });

                        i++;
                    }
                }

                // Get the count.
                count = results.length;

                return {
                   items: results, // The array of documents.      
                    offset: 0,
                    totalCount: count, // The total number of records.
                };

            },
        },


    });
})();
