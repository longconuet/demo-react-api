using DemoReactAPI.Enums;

namespace DemoReactAPI.Entities
{
    public class Quiz : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public QuizStatus Status { get; set; }
        public List<Question> Questions { get; set; }
    }
}
