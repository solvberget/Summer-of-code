using System.Linq;
using Nancy;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Info;

namespace Solvberget.Nancy.Modules
{
    public class InformationModule : NancyModule
    {
        public InformationModule(IInformationRepository informationRepository) : base("/info")
        {
            Get["/contact"] = _ => informationRepository.GetContactInformation();

            Get["/opening-hours"] = _ =>
            {
                var results = informationRepository.GetOpeningHoursInformation();
                return results.Select(oh => new OpeningHoursDto
                {
                    Title = oh.Title,
                    Hours = oh.LocationOrDayOfWeekToTime,
                    Phone = oh.Phone,
                    Location = oh.Location,
                    SubTitle = oh.SubTitle,
                    Url = oh.Url,
                    UrlText = oh.UrlText
                });
            };
        }
    }
}