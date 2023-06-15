using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Proiect.Data;
using Proiect.DataModels;
using Proiect.DataModels.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Proiect.Controllers
{
    public class AuthController : Controller
    {
        private readonly SocialDbContext context;
        private readonly IConfiguration config;

        public AuthController(SocialDbContext context ,IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterUserDTO user)
        {   try
            {
                if (context.Users.Any(u => u.Email == user.Email))
                {
                    return BadRequest("Email-ul exista deja!");

                }
                byte[] passwordHash, passwordKey;
                using (var hmac = new HMACSHA512())
                {
                    passwordKey = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
                }
                User Test = new User
                {
                    Email = user.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordKey,
                    Name = user.Name,
                    Surname = user.Surname,
                    BirthDay = user.BirthDay,
                    ProfilePicturePath = user.ProfilePicturePath,

                };
                context.Users.Add(Test);
                context.SaveChanges();

                return Ok(user);
            }
            catch(Exception ex) { return StatusCode(500, "A apărut o eroare în server. Vă rugăm să încercați din nou mai târziu!"); }

            
        }
        [HttpPost("LogIn")]
        public IActionResult LogIn([FromBody ] LogInDTO user)
        {
            try
            {
                var userInfo = context.Users.SingleOrDefault(u => u.Email.Equals(user.Email));
                if(userInfo == null || userInfo.PasswordSalt==null) {
                    return BadRequest("Email-ul nu exista!");
                }
                if (!MatchPasswordHash(user.Password, userInfo.PasswordHash, userInfo.PasswordSalt))
                {
                    return BadRequest("Parola nu se potriveste!");
                }
                return Ok(CreateJWT(userInfo));
            }
             catch(Exception ex) { return StatusCode(500, "A apărut o eroare în server. Vă rugăm să încercați din nou mai târziu!"); }
            
        }
        private bool MatchPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < hash.Length; i++)
                {
                    if (hash[i] != passwordHash[i]) return false;
                }
            }



            return true;
        }
        private string CreateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
               config["Jwt:Audience"],
               claims,
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
