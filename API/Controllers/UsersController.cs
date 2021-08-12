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

namespace API.Controllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = _mapper.Map<IEnumerable<MemberDTO>>(await _userRepository.GetAllAsync());
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            var user = _mapper.Map<MemberDTO>(await _userRepository.GetByUsernameAsync(username));
            return Ok(user);
        }
    }
}