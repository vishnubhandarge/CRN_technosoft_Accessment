using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetUserWithRefreshTokensAsync(string token);
}
