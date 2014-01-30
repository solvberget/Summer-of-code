(function () {
    "use strict";

    var ui = WinJS.UI;

    ui.Pages.define("/pages/openingHours/openingHours.html", {

        ready: function (element, options) {
            this.ajaxGetOpeningHoursInformation();
            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());
        },
        
        ajaxGetOpeningHoursInformation: function () {
            var url = window.Data.serverBaseUrl + "/Information/GetOpeningHoursInformation";
            Solvberget.Queue.QueueDownload("opening", { url: url }, this.ajaxGetOpeningHoursInformationCallback, this, true);
        },

        ajaxGetOpeningHoursInformationCallback: function (request, context) {
            var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
            if (response != undefined && response !== "")
                context.populateOpeningHours(response);
            $(".opening-hours .content").fadeIn("slow");
            $("#opening-hours-loading").hide();
        },
            
        populateOpeningHours: function (response) {

            var openingHoursTemplateDiv = document.getElementById("opening-hours-template");
            var openingHoursTemplateHolder = document.getElementById("opening-hours-information-template-holder");

            var openingHoursTemplate = undefined;
            if (openingHoursTemplateDiv)
                openingHoursTemplate = new WinJS.Binding.Template(openingHoursTemplateDiv);

            var model;

            if (response) {

                for (var i = 0; i < response.length; i++) {
                    model = response[i];

                    if (openingHoursTemplate && openingHoursTemplateHolder && model)
                        openingHoursTemplate.render(model, openingHoursTemplateHolder).done($.proxy(function () {

                            if (this.LocationOrDayOfWeekToTime) {
                                var openingHourListHtml = "";
                                for (var key in this.LocationOrDayOfWeekToTime) {
                                    openingHourListHtml += "<div class=\"opening-time-list-item\">";
                                    openingHourListHtml += "<span>" + key + "</span>";
                                    openingHourListHtml += "<div class=\"opening-time\">" + this.LocationOrDayOfWeekToTime[key] + "</div>";
                                    openingHourListHtml += "</div>";
                                }

                                $(".opening-hours-list:last").html(openingHourListHtml);
                            }

                            var colorIndex;
                            if (i % 2) colorIndex = 3;
                            else colorIndex = 5;
                            $(".opening-hour-tile:last").css("background-color", Data.getColorFromPool( colorIndex, "1.0"));

                        }, model));
                }
            }
        },

        unload: function () {
            Solvberget.Queue.CancelQueue('opening');
        }

    });

})();