(function () {
    "use strict";
    WinJS.Namespace.define("ViewModel", {
        propertiesList: {

            documentToPropertiesList: function (document) {
                var results = []
                var i = 0;
                for (var property in document) {
                    var propertyValue = null;
                    var propertyName = null;

                    propertyValue = eval("document." + property);
                    propertyName = Solvberget.Localization.getString(property);

                    if (propertyValue !== null) {
                        if (propertyValue[0] != undefined) {
                            if (propertyValue[0].Name != undefined)
                                propertyValue = propertyValue[0].Name + ",";
                        } else if (propertyValue.Name != undefined) {
                            propertyValue = propertyValue.Name;
                        }
                    }

                    if ((propertyValue !== null) && (propertyValue !== []) && (propertyValue != "")) {
                        var type = typeof (propertyValue);

                        while (type !== "string" && type !== "integer") {
                            propertyValue = propertyValue.toString();
                            var propertyValueTemp = "";
                            for (var j = 0; j < propertyValue.length; j++) {
                                if (propertyValue[j] == ",") {

                                    propertyValueTemp += ", ";
                                } else {
                                    propertyValueTemp += propertyValue[j];
                                }
                            }
                            propertyValue = propertyValueTemp;

                            type = typeof (propertyValue);
                        }
                      
                        results[i] =
                        {
                                propertyName: propertyName,
                                propertyValue: propertyValue,
                         
                        };
                        i++

                    }
                }

            

                return results;

            },
        },


    });
})();
