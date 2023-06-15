namespace Proiect.DataModels.DTO
{
    public class RequestDTO
    {
        public int ReciverId { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderEmail { get; set; }
    }
}
