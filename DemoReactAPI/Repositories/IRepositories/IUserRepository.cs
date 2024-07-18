using DemoReactAPI.Entities;
using System.Linq.Expressions;

namespace DemoReactAPI.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync(Expression<Func<User, bool>>? filter = null, bool tracked = true);
        Task<PaginatedList<User>> GetPaginatedUsersAsync(int pageNumber, int pageSize, Expression<Func<User, bool>>? filter = null, bool tracked = true);
        Task<User?> GetUserByIdAsync(Guid id, bool tracked = true);
        Task<User?> GetUserByPhoneAsync(string input);
        Task<User?> GetUserByEmailAsync(string input);
        Task<User?> GetUserByUsernameAsync(string input);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
