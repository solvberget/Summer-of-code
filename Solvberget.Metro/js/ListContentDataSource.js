(function () {

    // Define the IListDataAdapter.
    var listContentDataAdapter = WinJS.Class.define(
        function (docnumbers) {

            // Constructor
            this._minPageSize = 10;  // based on the default of 10
            this._maxPageSize = 50;  // max request size for bing images
            this._maxCount = 1000;   // limit on the bing API
            this._docnumbers = docnumbers;
            this._count = 0;

        },

        // IListDataDapter methods
        {
            
            //   This funtion must return an object that implements IFetchResult. 
            itemsFromIndex: function (requestIndex, countBefore, countAfter) {
                var that = this;
                if (requestIndex >= that._maxCount) {
                    return Promise.wrapError(new WinJS.ErrorFromName(UI.FetchError.doesNotExist));
                }

                var fetchSize, fetchIndex;

                // See which side of the requestIndex is the overlap.
                if (countBefore > countAfter) {
                    // Limit the overlap
                    countAfter = Math.min(countAfter, 10);

                    // Bound the request size based on the minimum and maximum sizes.
                    var fetchBefore = Math.max(
                        Math.min(countBefore, that._maxPageSize - (countAfter + 1)),
                        that._minPageSize - (countAfter + 1)
                        );
                    fetchSize = fetchBefore + countAfter + 1;
                    fetchIndex = requestIndex - fetchBefore;
                } else {
                    countBefore = Math.min(countBefore, 10);
                    var fetchAfter = Math.max(Math.min(countAfter, that._maxPageSize - (countBefore + 1)), that._minPageSize - (countBefore + 1));
                    fetchSize = countBefore + fetchAfter + 1;
                    fetchIndex = requestIndex - countBefore;
                }

                //Make request URL for the (light) documents we want
                var requestStr = "http://localhost:7089/Document/GetDocumentsLight/?ids=";
                for (var i = 0, docnumbersLength = this._docnumbers.length; i < docnumbersLength; i++) {
                    requestStr += this._docnumbers[i] + "-";
                }

                // Return the promise from making an XMLHttpRequest to the server.
                return WinJS.xhr({ url: requestStr }).then(

                    // The callback for a successful operation. 
                    function (request) {
                        var self = that;
                        var results = [], count;

                        // Use the JSON parser on the results (it's safer than using eval).
                        var obj = JSON.parse(request.responseText);

                        // Verify that the service returned images.
                        if (obj !== undefined) {
                            var items = obj;

                            // Create an array of IItem objects:
                            // results =[{ key: key1, data : { field1: value, field2: value, ... }}, { key: key2, data : {...}}, ...];
                            for (var i = 0, itemsLength = items.length; i < itemsLength; i++) {
                                var dataItem = items[i];
                                results.push({
                                    key: (fetchIndex + i).toString(),
                                    data: {
                                        rank: (i+1).toString(),
                                        doc: dataItem
                                    }
                                });
                            }

                            // Get the count
                            count = items.length;
                            self._count = count;

                            return {
                                items: results, // The array of items.
                                offset: requestIndex - fetchIndex, // The index of the requested item in the items array.
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

                //Make request URL for the (light) documents we want
                var requestStr = "http://localhost:7089/Document/GetDocumentsLight/?ids=";
                for (var i = 0, docnumbersLength = this._docnumbers.length; i < docnumbersLength; i++) {
                    requestStr += this._docnumbers[i] + "-";
                }

                // Return the promise from making an XMLHttpRequest to the server.
                return WinJS.xhr({ url: requestStr }).then(

                    // The callback for a successful operation. 
                    function (request) {

                        // Use the JSON parser on the results (it's safer than using eval).
                        var obj = JSON.parse(request.responseText);

                        // Verify that the service returned images.
                        if (obj !== undefined) {
                            return obj.length;
                        } else {
                            return WinJS.UI.FetchError.doesNotExist;
                        }
                    },

                    // Called if the WinJS.xhr funtion returned an error. 
                    function (request) {
                        return WinJS.UI.FetchError.noResponse;
                    });
            }
        }
        );

    var listContentDataSource = WinJS.Class.derive(WinJS.UI.VirtualizedDataSource, function (docnumbers) {
        this._baseDataSourceConstructor(new listContentDataAdapter(docnumbers));
    });

    WinJS.Namespace.define("DataSources.List", { ListContentDataSource: listContentDataSource });

})();