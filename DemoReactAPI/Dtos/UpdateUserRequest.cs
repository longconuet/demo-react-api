namespace DemoReactAPI.Dtos
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int Role { get; set; }
        public string? Avatar { get; set; }
        public string? AvatarPath { get; set; }
    }
}
