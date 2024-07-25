using DemoReactAPI.Enums;

namespace DemoReactAPI.Entities
{
    public class Question : BaseEntity
    {
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
        public QuestionType Type { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
