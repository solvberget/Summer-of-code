namespace Solvberget.Core.DTOs
{
    public class FilmDto : DocumentDto
    {
        public string AgeLimit { get; set; }
        
        public string MediaInfo { get; set; }
        
        public string[] ActorNames { get; set; }
        
        public string[] SubtitleLanguages { get; set; }
        
        public string[] ReferredPeopleNames { get; set; }
        
        public string[] ReferencedPlaces { get; set; }
        
        public string[] Genres { get; set; }
        public string[] InvolvedPersonNames { get; set; }
        public string[] ResponsiblePersonNames { get; set; }
    }
}