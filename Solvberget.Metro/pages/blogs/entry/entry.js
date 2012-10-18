(function () {
    "use strict";
    
    var ui = WinJS.UI;

    ui.Pages.define("/pages/blogs/entry/entry.html", {

        ready: function (element, options) {
            var model = options.model;
            // Sanitize HTML (remove dynamic HTML like iframes and script)
            model.Content = window.toStaticHTML(model.Content);
            
            // Float images inside the text 
            WinJS.Binding.processAll(element, model).done(function () {
                $("a img").each(function () {
                    if(!$(this).parent().parent().hasClass("separator")) // Bugfix with blogger.com
                         $(this).parent().css("float", "left").css("padding", "10px");
                });

            });
            
            if (model.Title) {
                if (model.Title.length > 40) {
                    $(".pagetitle").css("font-size", "15pt");
                }
            }

        },
    });
})();




