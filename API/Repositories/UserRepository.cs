using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces.Contracts;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(AppUser user)
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await _context.users.ToListAsync(); 
        }

        public async Task<AppUser> GetByIdAsync(int id)
        {
            return await _context.users.FindAsync(id);
        }

        public async Task<AppUser> GetByUsernameAsync(string username)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task UpdateAsync(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}