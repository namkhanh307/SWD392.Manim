namespace SWD392.Manim.Repositories.Entity
{
    public class ProblemParameter
    {
        public string ParameterId { get; set; } = string.Empty;
        public string ProblemId { get; set; } = string.Empty;
        public double Value { get; set; }
        public virtual Parameter? Parameter { get; set; }
        public virtual Problem? Problem { get; set; }

        public string? Createdby { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
