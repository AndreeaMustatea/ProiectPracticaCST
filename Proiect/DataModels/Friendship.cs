namespace Proiect.DataModels
{
    public class Friendship
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public DateTime Date { get; set; }
        public User? User1 { get; set; }
        public User? User2 { get; set; }
    }
}
