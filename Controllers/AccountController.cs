using AutoMapper;
using CSharpGetStarted.DTOs;
using CSharpGetStarted.Entities;
using CSharpGetStarted.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharpGetStarted.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("Username has been taken");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.Trim().ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password.Trim());

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok
            (
                new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager
                .Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName.Equals(loginDto.Username.Trim().ToLower()));

            if (user == null) return Unauthorized("Invalid username or password");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password.Trim());

            if (!result) return Unauthorized("Invalid username or password");

            return Ok(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        private async Task<bool> UserExist(string username)
        {
            return await _userManager
                .Users
                .AnyAsync(x => 
                    x.UserName.Equals(username.Trim().ToLower())
                );
        }
    }
}
