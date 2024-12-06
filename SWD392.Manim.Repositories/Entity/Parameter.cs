using SWD392.Manim.Repositories.Base;

namespace SWD392.Manim.Repositories.Entity
{
    public class Parameter : BaseEntity
    {
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string TopicId { get; set; } = string.Empty;
        public virtual Topic? Topic { get; set; }
        public virtual ICollection<ProblemParameter> ProblemParameters { get; set; } = new List<ProblemParameter>();
    }
}
