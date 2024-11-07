using Microsoft.AspNetCore.Http;

namespace SWD392.Manim.Repositories.ViewModel.SubjectVM
{
    public class PostSubjectVM
    {
        public required string Name { get; set; }
        public IFormFile ImageLink { get; set; }
    }
}
