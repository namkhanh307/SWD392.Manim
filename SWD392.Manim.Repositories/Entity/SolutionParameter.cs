namespace SWD392.Manim.Repositories.Entity
{
    public class SolutionParameter
    {
        public string ParameterId { get; set; } = string.Empty;
        public string SolutionId { get; set; } = string.Empty;
        public double Value { get; set; }
        public virtual Parameter? Parameter { get; set; }
        public virtual Solution? Solution { get; set; }
        public string? Createdby { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
