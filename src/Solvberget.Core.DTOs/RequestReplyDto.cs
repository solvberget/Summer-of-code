namespace Solvberget.Core.DTOs
{
    public class RequestReplyDto
    {
        public RequestReplyDto()
        {
            Success = true;
        }

        public bool Success { get; set; }
        public string Reply { get; set; }
    }
}
