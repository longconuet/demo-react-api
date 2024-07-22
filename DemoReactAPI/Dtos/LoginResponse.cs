using DemoReactAPI.Enums;

namespace DemoReactAPI.Dtos
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public RoleEnum Role { get; set; }
        public string FullName { get; set; }
    }
}
