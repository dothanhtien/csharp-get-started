using AutoMapper;
using CSharpGetStarted.Data;
using CSharpGetStarted.DTOs;
using CSharpGetStarted.Entities;
using CSharpGetStarted.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharpGetStarted.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("Username has been taken");

            var user = _mapper.Map<AppUser>(registerDto);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var result = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.Equals(loginDto.Username.Trim().ToLower()));

            if (user == null) return Unauthorized("Invalid username or password");

            var result = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(result);
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
