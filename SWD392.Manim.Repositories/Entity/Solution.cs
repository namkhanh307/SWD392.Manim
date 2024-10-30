using SWD392.Manim.Repositories.Base;

namespace SWD392.Manim.Repositories.Entity;

public partial class Solution : BaseEntity
{
    //public string SolutionTypeId { get; set; } = string.Empty;
    public string ProblemId { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual Problem? Problem { get; set; }
    //public virtual SolutionOutput? SolutionOutput { get; set; }
    public virtual ApplicationUser? User { get; set; }
}
