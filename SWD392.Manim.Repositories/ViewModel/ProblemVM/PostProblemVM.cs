using SWD392.Manim.Repositories.ViewModel.ProblemParameterVM;

namespace SWD392.Manim.Repositories.ViewModel.ProblemVM
{
    public class PostProblemVM
    {
        public string TopicId { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public ICollection<PostPPVM> PostPPVMs { get; set; } = new List<PostPPVM>();
        public string? Description { get; set; }
    }
}
