using DemoReactAPI.Models;
using DemoReactAPI.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoReactAPI.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtBlackListService _blackListService;

        public TokenValidationMiddleware(RequestDelegate next, JwtSettings jwtSettings, JwtBlackListService jwtBlackListService)
        {
            _next = next;
            _jwtSettings = jwtSettings;
            _blackListService = jwtBlackListService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null && endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() != null)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null || (token != null && _blackListService.IsTokenBlackListed(token)))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                AttachUserToContext(context, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                // Attach user to context on successful jwt validation
                context.Items["UserName"] = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
                context.Items["Role"] = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            }
            catch (Exception e)
            {
                // Do nothing if jwt validation fails
                // User is not attached to context so request won't have access to secure routes
            }
        }
    }

}
