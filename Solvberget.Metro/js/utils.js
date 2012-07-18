(function () {

    var delayImageLoader = WinJS.Class.define(
            function (element, options) {
                this._element = element || document.createElement("div");
                this.element.winControl = this;
                WinJS.Utilities.addClass(this.element, "imageLoader");
                WinJS.Utilities.query("img", element).forEach(function (img) {
                    img.addEventListener("load", function () {
                        WinJS.Utilities.addClass(img, "loaded");
                    });
                });
            },
            {


                element: {
                    get: function () { return this._element; }
                },
            });

    WinJS.Namespace.define("Solvberget.Utils", {
        DelayImageLoader: delayImageLoader
    });
})();

(function () {

    var styleNullToHiddenConverter = WinJS.Binding.converter(function (val) {
        var returnvalue;
        if (val != null) {
            returnvalue = val.toString() == [] ? "none" : "block";
            ;
        } else {
            returnvalue = "none";
        }
        return returnvalue;
    });
    WinJS.Namespace.define("Solvberget.Converters", {
        styleNullToHiddenConverter: styleNullToHiddenConverter
    });
})();