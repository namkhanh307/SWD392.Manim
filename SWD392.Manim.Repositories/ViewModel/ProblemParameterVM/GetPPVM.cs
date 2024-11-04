using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Repositories.ViewModel.ProblemParameterVM
{
    public class GetPPVM
    {
        public string ParameterId { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;

        public double Value { get; set; }
    }
}
