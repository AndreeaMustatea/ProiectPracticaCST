namespace Proiect.DataModels.DTO
{
    public class RegisterUserDTO
    {
        public string? Email { get; set; }
        public string? Password{ get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime BirthDay { get; set; }

        public string? ProfilePicturePath { get; set; }
    }
}
