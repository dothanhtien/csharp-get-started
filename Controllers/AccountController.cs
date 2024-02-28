using System.Security.Cryptography;
using System.Text;
using CSharpGetStarted.Data;
using CSharpGetStarted.DTOs;
using CSharpGetStarted.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharpGetStarted.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("Username has been taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.Trim().ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password.Trim())),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.Equals(loginDto.Username.Trim().ToLower()));

            if (user == null) return Unauthorized("Invalid username or password");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password.Trim()));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid username or password");
            }

            return Ok(user);
        }

        private async Task<bool> UserExist(string username)
        {
            return await _context
                .Users
                .AnyAsync(x => 
                    x.UserName.Equals(username.Trim().ToLower())
                );
        }
    }
}
