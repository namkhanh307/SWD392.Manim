using SWD392.Manim.Repositories.Base;

namespace SWD392.Manim.Repositories.Entity;

public partial class Problem : BaseEntity
{
    public string TopicId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Type { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    //public virtual ICollection<SolutionType> SolutionTypes { get; set; } = new List<SolutionType>();
    public virtual ICollection<Solution> Solutions { get; set; } = new List<Solution>();
    public virtual ICollection<ProblemParameter> ProblemParameters { get; set; } = new List<ProblemParameter>();
    public virtual Topic? Topic { get; set; }
}
