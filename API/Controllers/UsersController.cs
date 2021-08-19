using System.Collections.Generic;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces.Contracts;
using API.DTOs;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using API.Interfaces;

namespace API.Controllers
{
    //[Authorize]
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
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await _userRepository.GetAllMembersAsync();
            return Ok(users);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberAsync(username);
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateMemberDto member)
        {
            var username = User.GetUserName();
            var user = await _userRepository.GetByUsernameAsync(username);

            _mapper.Map(member, user);

            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file) 
        {
            var user = await _userRepository.GetByUsernameAsync(User.GetUserName());

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo 
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            return CreatedAtRoute("GetUser", new { username = user.UserName },_mapper.Map<PhotoDTO>(photo));
        }
    }
}