//Converter function
(function () {

   var localizeString= WinJS.Binding.converter(function(stringValue) {
        return Solvberget.Localization.getString(stringValue);
    });
    
    WinJS.Namespace.define("Solvberget.Converters", {
       localizestring: localizeString
        
    });
})();