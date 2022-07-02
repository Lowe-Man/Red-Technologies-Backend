using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Authorization;
using API.DataModels;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<GenericMessageResponse>> PostUser(User user)
        {
            if (_context.User.FirstOrDefault(u => u.Email == user.Email) != null) return Conflict(new GenericMessageResponse { message = "User already exists" });
            user.Password = BC.HashPassword(user.Password);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            var message = new GenericMessageResponse
            {
                message = "User created"
            };

            return Created("/api/user" + user.Id, message);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
