using SWD392.Manim.Repositories.ViewModel.ProblemParameterVM;

namespace SWD392.Manim.Repositories.ViewModel.ProblemVM
{
    public class PostProblemVM
    {
        public string TopicId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ICollection<PostPPVM> PostPPVMs { get; set; } = new List<PostPPVM>();
        public string? Description { get; set; }
    }
}
