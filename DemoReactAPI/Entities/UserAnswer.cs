namespace DemoReactAPI.Entities
{
    public class UserAnswer : BaseEntity
    {
        public Guid QuizAttemptId { get; set; }
        public QuizAttempt QuizAttempt { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public Guid? AnswerId { get; set; }
        public Answer? Answer { get; set; }
        public string TextAnswer { get; set; }
    }
}
