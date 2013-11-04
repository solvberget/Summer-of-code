using System.Collections.Generic;

namespace Solvberget.Core.DTOs
{
    public class CdDto : DocumentDto
    {
        public string ArtistOrComposerName { get; set; }
        public string[] CompositionTypesOrGenres { get; set; }
    }
}