using SWD392.Manim.Repositories.Base;

namespace SWD392.Manim.Repositories.Entity;

public partial class Subject : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
    public string Image { get; set; } = string.Empty;
    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
}
