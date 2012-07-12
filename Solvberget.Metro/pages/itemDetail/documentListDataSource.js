(function () {

    // Define the IListDataAdapter.
    var documentDataAdapter = WinJS.Class.define(
        function (query) {
            this._query = query;


            // Constructor
            this._minPageSize = 10;  // default
            this._maxPageSize = 50;  // max
            this._maxCount = 1000;   // limit
        },

        // IListDataDapter methods
        // These methods define the contract between the IListDataSource and the IListDataAdapter.
        // These methods will be called by vIListDataSource to fetch items, 
        // get the number of items, and so on.
        {
            // This example only implements the itemsFromIndex and count methods

            // The itemsFromIndex method is called by the IListDataSource 
            // to retrieve items. 
            // It will request a specific item and hints for a number of items before and after the
            // requested item. 
            // The implementation should return the requested item. You can choose how many
            // additional items to send back. It can be more or less than those requested.
            //
            //   This funtion must return an object that implements IFetchResult.

            itemsFromIndex: function (requestIndex, countBefore, countAfter) {
                var that = this;
                if (requestIndex >= that._maxCount) {
                    return Promise.wrapError(new WinJS.ErrorFromName(UI.FetchError.doesNotExist));
                }

                

                // Create the request string. 
                var requestStr = "http://localhost:7089/Document/GetDocument/" + that._query;

                // Return the promise from making an XMLHttpRequest to the server.
                return WinJS.xhr({ url: requestStr }).then(

                    // The callback for a successful operation. 
                    function (request) {
                        var results = [], count;

                        // Use the JSON parser on the results (it's safer than using eval).
                        var obj = JSON.parse(request.responseText);

                        // Verify that the service returned images.
                        if (obj !== undefined) {
                            var item = obj;
                            //var items = [];
                            // Create an array of IItem objects:
                            // results =[{ key:i, data: propertyName1, propertyValue1}, {...}, ...];
                            var i = 0;
                            for (property in item) {
                                var itemValue = null;
                                var itemName = null;

                                itemValue = eval("item." + property);
                                itemName = Solvberget.Localization.getString(property);
                                if(itemName == "Forfatter") {
                                    console.log("debug");
                                }
                                if (itemValue !== null ) {
                                    if (itemValue[0] != undefined ) {
                                        if (itemValue[0].Name != undefined)
                                            itemValue = itemValue[0].Name+",";
                                    }else if(itemValue.Name != undefined) {
                                        itemValue = itemValue.Name;
                                    }
                                }
                                
                                if ((itemValue !== null) && (itemValue !== []) && (itemValue !="")) {
                                    var type = typeof (itemValue);
                                    
                                    while (type !== "string" && type !== "integer") {
                                        itemValue = itemValue.toString();
                                        var itemValueTemp="";
                                        for (var j = 0; j < itemValue.length; j++) {
                                            if (itemValue[j] == ",") {
                                                
                                                itemValueTemp += ", ";
                                            }else {
                                                itemValueTemp += itemValue[j];
                                            }
                                        }
                                        itemValue = itemValueTemp;
                                       
                                        type = typeof (itemValue);     
                                    }
                                    

                                    //items[i] = new Object();
                                    //var temp = items[i];
                                    //eval("temp." + itemName + "='" + itemValue + "'");

                                    results.push(
                                    {
                                        key: (i).toString(),
                                        data:
                                        {
                                            propertyName: itemName,
                                            propertyValue: itemValue,
                                        }
                                    });

                                    i++;
                                }
                            }

                            // Get the count.
                            count = results.length;

                            return {
                                items: results, // The array of items.      
                                offset: requestIndex,
                                totalCount: Math.min(count, that._maxCount), // The total number of records.
                            };
                        } else {
                            return WinJS.UI.FetchError.doesNotExist;
                        }
                    },

                    // Called if the WinJS.xhr funtion returned an error. 
                    function (request) {
                        return WinJS.UI.FetchError.noResponse;
                    });
            },
            // Gets the number of items in the result list. 
            // The count can be updated in itemsFromIndex.
            getCount: function () {
                var that = this;

                // Create up a request for 1 item so we can get the count
                var requestStr = "http://localhost:7089/Document/GetDocument/" + that._query;

                // Make an XMLHttpRequest to the server and use it to get the count.
                return WinJS.xhr({ url: requestStr }).then(

                    // The callback for a successful operation.
                    function (request) {
                        var data = JSON.parse(request.responseText);
                        var i = 0;
                        for (property in item) {
                            var itemValue = eval("item." + property);
                            if (itemValue !== null) {

                                i++;
                            }
                        }

                        // Get the count.
                        return i;
                    },
                    function (request) {
                        return WinJS.Promise.wrapError(new WinJS.ErrorFromName(WinJS.UI.FetchError.doesNotExist));
                    });
            }
            // setNotificationHandler: not implemented
            // itemsFromStart: not implemented
            // itemsFromEnd: not implemented
            // itemsFromKey: not implemented
            // itemsFromDescription: not implemented
        }
        );



    var documentDataSource = WinJS.Class.derive(WinJS.UI.VirtualizedDataSource, function (query) {
        this._baseDataSourceConstructor(new documentDataAdapter(query));
    });


    WinJS.Namespace.define("DataSources", { documentDataSource: documentDataSource });

})();