using DemoReactAPI.Entities;
using DemoReactAPI.Models;
using DemoReactAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DemoReactAPI.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(JwtSettings jwtSettings, ApplicationDbContext context)
        {
            _jwtSettings = jwtSettings;
            _context = context;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string username, string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Username == username && rt.Token == refreshToken);
        }

        public async Task SaveRefreshTokenAsync(string username, string refreshToken)
        {
            var token = new RefreshToken
            {
                Token = refreshToken,
                Username = username,
                ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRefreshTokenAsync(string username, string newRefreshToken)
        {
            var token = _context.RefreshTokens
                .FirstOrDefault(rt => rt.Username == username);
            if (token != null)
            {
                token.Token = newRefreshToken;
                token.ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays);

                _context.Update(token);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveRefreshTokenAsync(string username, string refreshToken)
        {
            var token = _context.RefreshTokens
                .FirstOrDefault(rt => rt.Username == username && rt.Token == refreshToken);
            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}
