namespace DemoReactAPI.Entities
{
    public class QuizAttempt : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Score { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }
    }
}
