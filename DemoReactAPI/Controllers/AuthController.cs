using DemoReactAPI.Dtos;
using DemoReactAPI.Enums;
using DemoReactAPI.Helpers;
using DemoReactAPI.Repositories.IRepositories;
using DemoReactAPI.Services;
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

        public AuthController(JwtService jwtService, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        [HttpPost("login")]
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

            // gen token
            var accessToken = _jwtService.GenerateAccessToken(request.Username, "Admin");
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

            var newAccessToken = _jwtService.GenerateAccessTokenByClaims(principal.Claims);
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
            var username = principal.Identity!.Name;

            await _refreshTokenRepository.RemoveRefreshTokenAsync(username, request.RefreshToken);

            return NoContent();
        }
    }
}
