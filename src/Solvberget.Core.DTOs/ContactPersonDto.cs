namespace Solvberget.Core.DTOs
{
    public class ContactPersonDto : RequestReplyDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
