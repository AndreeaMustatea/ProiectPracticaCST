using System.ComponentModel.DataAnnotations;

namespace Proiect.DataModels
{
    public class User
    {
        [Key]

        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDay { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt{ get; set; }

        public string? ProfilePicturePath { get; set; }

        public ICollection<FriendRequest>? FriendRequestsSend { get; set; }
        public ICollection<FriendRequest>? FriendRequestsRecived { get; set; }

        public ICollection<Friendship>? FriendsOf { get; set; }
        public ICollection<Friendship>? Friends { get; set; }

    }
}
