using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Repositories.ViewModel.AuthVM
{
    public class ChangePasswordVM
    {
        public required string OldPassword { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
