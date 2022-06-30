using API.Data;
using API.DataModels;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace API.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Authenticate([FromBody] UserAuthenticate userAuth)
        {
            var user = Verify(userAuth);
            if (user == null) return NotFound("User not found");
            var token = Generate(user);
            return Ok(token);
        }

        private TokenResponse Generate(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
                }),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenResponse
            {
                token = tokenHandler.WriteToken(token)
            };
        }

        private User Verify(UserAuthenticate userAuth)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Email.ToLower() == userAuth.Email.ToLower());
            if (currentUser == null) return null;
            if (BC.Verify(userAuth.Password, currentUser?.Password)) return currentUser;
            return null;

        }
    }
}
