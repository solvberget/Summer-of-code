using System.Collections.Generic;

namespace Solvberget.Nancy.Modules.V2
{
    public class DocumentDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Type { get; set; }
        public DocumentAvailabilityDto Availability { get; set; }
    }

    public class DocumentAvailabilityDto
    {
        public string Branch { get; set; }
        public int AvailableCount { get; set; }
        public int TotalCount { get; set; }
        public string Department { get; set; }
        public string Collection { get; set; }
        public string Location { get; set; }
        public string EstimatedAvailableDate { get; set; }
    }
}