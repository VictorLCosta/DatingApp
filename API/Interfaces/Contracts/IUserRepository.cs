using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces.Contracts
{
    public interface IUserRepository
    {
        Task CreateAsync(AppUser user);
        Task UpdateAsync(AppUser user);
        Task<AppUser> GetByIdAsync(int id);
        Task<AppUser> GetByUsernameAsync(string username);
        Task<IEnumerable<AppUser>> GetAllAsync();
    }
}