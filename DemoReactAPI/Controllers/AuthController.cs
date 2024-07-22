﻿using DemoReactAPI.Dtos;
using DemoReactAPI.Entities;
using DemoReactAPI.Helpers;
using DemoReactAPI.Repositories.IRepositories;
using DemoReactAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoReactAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthController(JwtService jwtService, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            if (PasswordHelper.VerifyPassword(request.Password, user.Password))
            {
                var userRole = "Admin";

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, userRole)
                };

                var accessToken = _jwtService.GenerateAccessToken(claims);
                var refreshToken = _jwtService.GenerateRefreshToken();

                await _refreshTokenRepository.SaveRefreshTokenAsync(request.Username, refreshToken);

                return Ok(new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }

            return Unauthorized();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity?.Name;

            var storedRefreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(username, request.RefreshToken);
            if (storedRefreshToken == null || storedRefreshToken.ExpiryDate < DateTime.Now)
            {
                return Unauthorized();
            }

            var newAccessToken = _jwtService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _refreshTokenRepository.UpdateRefreshTokenAsync(username, newRefreshToken);

            return Ok(new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] TokenRequest request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity.Name;

            await _refreshTokenRepository.RemoveRefreshTokenAsync(username, request.RefreshToken);

            return NoContent();
        }
    }
}
