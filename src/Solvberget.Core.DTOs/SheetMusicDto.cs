namespace Solvberget.Core.DTOs
{
    public class SheetMusicDto : DocumentDto
    {
        public string ComposerName { get; set; }
        public string CompositionType { get; set; }
        public string NumberOfPagesAndParts { get; set; }
        public string[] MusicalLineup { get; set; }
    }
}