namespace Proiect.DataModels
{
    public class FriendRequest
    {
        public int SenderId { get; set; }
        public int ReciverId { get; set; }
        public DateTime Date { get; set; }

        public User? Sender { get; set; }
        public User? Reciver { get; set; }
    }
}
