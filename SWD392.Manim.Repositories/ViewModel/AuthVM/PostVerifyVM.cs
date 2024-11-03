using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Repositories.ViewModel.AuthVM
{
    public class PostVerifyVM
    {
        public required string UserId { get; set; }
        public required string Otp { get; set; }
    }
}
