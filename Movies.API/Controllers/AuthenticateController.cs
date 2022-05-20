using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Movies.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly algebramssqlhost_moviesContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticateController(algebramssqlhost_moviesContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/authenticate/login
        [HttpPost]
        [Route("login")]
        public ActionResult Login(LoginModel model)
        {
            var user = _context.AspNetUsers.FirstOrDefault(u => u.UserName == model.UserName);

            if (user != null)
            {
                // Korisnik postoji, ali provjeri i lozinku
                var hasher = new PasswordHasher<AspNetUser>();

                // VerifyHashPassword() -> verificira hash lozinke iz tablice sa stringom za prijavu
                var is_valid = hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                if (is_valid == PasswordVerificationResult.Success)
                {
                    // I korisnik i lozinka se poklapaju
                    var auth_claim = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName)
                    };

                    var token = GetToken(auth_claim);

                    return Ok(
                        new 
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        }
                    );
                }

                return Unauthorized();
            }

            // Korisnik ne postoji
            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> auth_claim)
        {
            var auth_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:ValidIssuer"],
                audience: _configuration["JwtSettings:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: auth_claim,
                signingCredentials: new SigningCredentials(auth_key, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
