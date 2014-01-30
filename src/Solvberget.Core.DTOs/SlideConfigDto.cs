using System.Collections.Generic;

namespace Solvberget.Core.DTOs
{
    public class SlideConfigDto
    {
        public string Template { get; set; }
        public int Duration { get; set; }

        public Dictionary<string, string> SlideOptions { get; set; }
    }
}