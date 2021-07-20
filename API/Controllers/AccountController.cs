using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
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
            if(await UserExists(registerDto.Username))
            {
                return BadRequest("Username is taken");
            }

            using(var hmac = new HMACSHA512())
            {
                var user = new AppUser
                {
                    UserName = registerDto.Username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key
                };

                await _context.users.AddAsync(user);
                await _context.SaveChangesAsync();

                return user;
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto dto)
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.UserName == dto.Username);
            if(user == null) return Unauthorized("Invalid username");

            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

                for(int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
                }

                return user;
            }
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}