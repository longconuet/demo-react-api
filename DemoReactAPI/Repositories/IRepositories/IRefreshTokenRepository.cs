using DemoReactAPI.Entities;

namespace DemoReactAPI.Repositories.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string username, string refreshToken);
        Task SaveRefreshTokenAsync(string username, string refreshToken);
        Task UpdateRefreshTokenAsync(string username, string newRefreshToken);
        Task RemoveRefreshTokenAsync(string username, string refreshToken);
    }
}
