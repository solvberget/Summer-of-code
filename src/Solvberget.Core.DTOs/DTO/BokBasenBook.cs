namespace Solvberget.Domain.DTO
{
    public sealed class BokBasenBook
    {
        // Same property names as XML-doc
        // ReSharper disable InconsistentNaming
        public string Id { get; set; }
        public string Fsreview { get; set; }
        public string Publisher_text { get; set; }
        public string Contents { get; set; }
        public string Thumb_Cover_Picture { get; set; }
        public string Small_Cover_Picture { get; set; }
        public string Large_Cover_Picture { get; set; }
        public string Sound { get; set; }
        public string Extract { get; set; }
        public string Marc { get; set; }
        public string Reviews { get; set; }
        // ReSharper restore InconsistentNaming
    }
}
