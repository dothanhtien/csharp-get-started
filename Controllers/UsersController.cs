using AutoMapper;
using CSharpGetStarted.DTOs;
using CSharpGetStarted.Entities;
using CSharpGetStarted.Extensions;
using CSharpGetStarted.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSharpGetStarted.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberAsync(username);

            if (user == null) return NotFound("User not found");

            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateMemberDto updateMemberDto)
        {
            var username = User.GetUsername();

            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null) return NotFound("User not found");

            _mapper.Map(updateMemberDto, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("An error occurred when updating user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound("User not found");

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync()) {
                return CreatedAtAction
                    (
                        nameof(GetUser), 
                        new {username = user.UserName}, 
                        _mapper.Map<PhotoDto>(photo)
                    );
            }

            return BadRequest("An error occurred when uploading photo");
        }
    }
}
