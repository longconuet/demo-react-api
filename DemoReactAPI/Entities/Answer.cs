namespace DemoReactAPI.Entities
{
    public class Answer : BaseEntity
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}
