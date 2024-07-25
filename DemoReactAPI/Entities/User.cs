using DemoReactAPI.Enums;

namespace DemoReactAPI.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Avatar { get; set; }
        public RoleEnum Role { get; set; } = RoleEnum.USER;
        public UserStatusEnum Status { get; set; } = UserStatusEnum.Active;

        public List<QuizAttempt> QuizAttempts { get; set; }
    }
}
