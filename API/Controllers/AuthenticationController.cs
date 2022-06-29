using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using BC = BCrypt.Net.BCrypt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using API.DataModels;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/authenticate")]
    [Produces("application/json")]
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(_configuration["JWT:Issuer"],
                _configuration["JWT:Audience"], claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            var writeToken = new JwtSecurityTokenHandler().WriteToken(token);
            var tokenResponse = new TokenResponse
            {
                token = writeToken,
            };
            return tokenResponse;
        }

        private User Verify(UserAuthenticate userAuth)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Email.ToLower() == userAuth.Email.ToLower());
            if (currentUser == null) return null;
            if (BC.Verify(userAuth.Password, currentUser?.PasswordHash)) return currentUser;
            return null;

        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
