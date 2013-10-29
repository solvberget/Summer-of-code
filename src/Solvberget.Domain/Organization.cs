namespace Solvberget.Domain.DTO
{
    public class Organization
    {
        public string Name { get; set; }
        public string UnderOrganization { get; set; }
        public string Role { get; set; }
        public string FurtherExplanation { get; set; }
        public string ReferencedPublication { get; set; }
    }
}
