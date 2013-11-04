namespace Solvberget.Core.DTOs
{
    public class BookDto : DocumentDto
    {
        public string Classification { get; set; }

        public string AuthorName { get; set; }
        
        public BookSeriesDto Series { get; set; }
    }

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