namespace DemoReactAPI.Dtos
{
    public class CreateUserRequest
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string? Avatar { get; set; }
    }
}
