using System;

namespace Solvberget.Core.DTOs
{
    public class FineDto
    {
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public double Sum { get; set; }
        public string Description { get; set; }
        
        public DocumentDto Document { get; set; }
    }
}
