(function () {

    // Define the IListDataAdapter.
    var listDataAdapter = WinJS.Class.define(
        function () {

            // Constructor
            this._minPageSize = 1;  // default
            this._maxPageSize = 10;  // max
            this._maxCount = 20;   // limit
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

                // Create the request string for the lists (no query parameters) 
                var requestStr = "http://localhost:7089/List/GetLists";

                // Return the promise from making an XMLHttpRequest to the server.
                return WinJS.xhr({ url: requestStr }).then(

                    // The callback for a successful operation. 
                    function (request) {
                        var self = that;
                        var results = [], count;

                        // Use the JSON parser on the results (it's safer than using eval).
                        var obj = JSON.parse(request.responseText);

                        if (obj !== undefined) {
                            var items = obj

                            // Create an array of IItem objects:
                            // results =[{ key: key1, data : { field1: value, field2: value, ... }}, { key: key2, data : {...}}, ...];
                            for (var i = 0, itemsLength = items.length; i < itemsLength; i++) {
                                var dataItem = items[i];
                                results.push({
                                    key: (fetchIndex + i).toString(),
                                    data: {
                                        listtitle: dataItem.Name,
                                        docnrs: dataItem.DocumentNumbers,
                                        docs: dataItem.Documents
                                    }
                                });
                            }

                            // Get the count
                            count = items.length;
                            self._count = count;

                            return {
                                items: results, // The array of items.
                                offset: requestIndex - fetchIndex, // The index of the requested item in the items array.
                                totalCount: Math.min(count, that._maxCount), // The total number of records. Bing will only return 1000, so we cap the value.
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
                var self = this
                return WinJS.Promise.timeout(100)
                    .then(function () {
                        return self._count;
                    });
            }
        }
    );

    var listDataSource = WinJS.Class.derive(WinJS.UI.VirtualizedDataSource, function () {
        this._baseDataSourceConstructor(new listDataAdapter());
    });

    WinJS.Namespace.define("DataSources.List", { ListDataSource: listDataSource });

})();