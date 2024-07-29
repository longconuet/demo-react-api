using Azure.Core;
using DemoReactAPI.Dtos;
using DemoReactAPI.Entities;
using DemoReactAPI.Enums;
using DemoReactAPI.Helpers;
using DemoReactAPI.Repositories.IRepositories;
using DemoReactAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoReactAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtBlackListService _blackListService;

        public AuthController(
            JwtService jwtService, 
            IUserRepository userRepository, 
            IRefreshTokenRepository refreshTokenRepository,
            JwtBlackListService jwtBlackListService)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _blackListService = jwtBlackListService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ResponseDto<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                return new ResponseDto<LoginResponse>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = "User not found",
                };
            }

            if (!PasswordHelper.VerifyPassword(request.Password, user.Password))
            {
                return new ResponseDto<LoginResponse>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = "Incorrect password",
                };
            }
            var role = user.Role == RoleEnum.ADMIN ? "Admin" : "User";

            // gen token
            var accessToken = _jwtService.GenerateAccessToken(request.Username, role);
            var refreshToken = _jwtService.GenerateRefreshToken();
            await _refreshTokenRepository.SaveRefreshTokenAsync(request.Username, refreshToken);

            return new ResponseDto<LoginResponse>
            {
                Status = ResponseStatusEnum.SUCCEED,
                Data = new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    FullName = user.FullName,
                    Role = user.Role,
                    Avatar = user.Avatar
                }
            };
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ResponseDto<LoginResponse>> RefreshToken([FromBody] TokenRequest request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity?.Name;

            var storedRefreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(username, request.RefreshToken);
            if (storedRefreshToken == null || storedRefreshToken.ExpiryDate < DateTime.Now)
            {
                return new ResponseDto<LoginResponse>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = "Invalid token",
                };
            }

            var newAccessToken = _jwtService.GenerateAccessTokenByClaims(principal.Claims);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _refreshTokenRepository.UpdateRefreshTokenAsync(username, newRefreshToken);

            // get user info
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return new ResponseDto<LoginResponse>
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = "User not found",
                };
            }

            return new ResponseDto<LoginResponse>
            {
                Status = ResponseStatusEnum.SUCCEED,
                Data = new LoginResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    FullName = user.FullName,
                    Role = user.Role,
                    Avatar = user.Avatar
                }
            };
        }


        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke([FromBody] TokenRequest request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity!.Name;

            await _refreshTokenRepository.RemoveRefreshTokenAsync(username, request.RefreshToken);

            return NoContent();
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ResponseDto> Logout([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.AccessToken) || string.IsNullOrEmpty(request.RefreshToken))
            {
                return new ResponseDto
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = "Token is required",
                };
            }

            // add access token to black list
            _blackListService.AddTokenToBlacklist(request.AccessToken);

            // revoke refresh token
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return new ResponseDto
                {
                    Status = ResponseStatusEnum.FAILED,
                    Message = "Invalid token",
                };
            }
            var username = principal.Identity!.Name;
            await _refreshTokenRepository.RemoveRefreshTokenAsync(username, request.RefreshToken);

            return new ResponseDto
            {
                Status = ResponseStatusEnum.SUCCEED,
                Message = "Logout successfully",
            };
        }
    }
}
