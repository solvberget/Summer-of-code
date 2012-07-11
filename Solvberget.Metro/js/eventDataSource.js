(function () {

    // Define the IListDataAdapter.
    var eventsDataAdapter = WinJS.Class.define(
        function () {

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

                // Create the request string. 
                var requestStr = "http://localhost:7089/Event/GetEvents";
               
                // Return the promise from making an XMLHttpRequest to the server.
                return WinJS.xhr({ url: requestStr }).then(

                    // The callback for a successful operation. 
                    function (request) {
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
                                        title: dataItem.Name,
                                        date: dataItem.Date,
                                        start: "Starter kl. " + dataItem.Start,
                                        stop: "Slutter kl. " + dataItem.Stop,
                                        location: dataItem.Location,
                                        description: dataItem.Description,
                                        //NB: Teaser er foreløpig dummydata, skal bindes mot dataItem.Teaser, men dette er vanligvis tomt.
                                        teaser: "Dette blir veldig spennende, dere! (Dette er en teaser)",
                                        type: "Passer for: " + dataItem.TypeName,
                                        url: { l : dataItem.Link, t : "Link til arrangement" },
                                        address: dataItem.Address,
                                        city: dataItem.City + " " + dataItem.PostalCode,
                                        thumbImage: dataItem.ThumbUrl,
                                        backgroundImage: dataItem.PictureUrl,
                                        dateAndTime: dataItem.Date + " " + dataItem.Start
                                    }
                                });
                            }

                            // Get the count.
                            count = items.length;

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
                var that = this;

                // Create up a request for 1 item so we can get the count
                var requestStr = "http://localhost:7089/Event/GetEvents";
                               
                // Make an XMLHttpRequest to the server and use it to get the count.
                return WinJS.xhr({ url: requestStr }).then(

                    // The callback for a successful operation.
                    function (request) {
                        var data = JSON.parse(request.responseText);

                        // Bing may return a large count of items, 
                        /// but you can only fetch the first 1000.
                        return Math.min(data.length, that._maxCount);
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

    var eventsDataSource = WinJS.Class.derive(WinJS.UI.VirtualizedDataSource, function () {
        this._baseDataSourceConstructor(new eventsDataAdapter());
    });

    WinJS.Namespace.define("DataSources", { eventsDataSource: eventsDataSource }); 

})();