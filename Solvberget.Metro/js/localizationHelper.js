(function () {

    var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
    var getString = function (name) {
        if (!name)
            return "";
        var stringName1 = resourceLoader.getString(name);
        if (!stringName1) {
            return name;
        }
        return stringName1;
    };
    WinJS.Namespace.define("Solvberget.Localization", {
        getString: getString
    });
})();

