using SWD392.Manim.Repositories.ViewModel.ProblemParameterVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Repositories.ViewModel.ProblemVM
{
    public class PurchaseProblemVM
    {
        public required string ProblemId { get; set; }
        public ICollection<PostPPVM> PostPPVMs { get; set; } = new List<PostPPVM>();
    }
}
