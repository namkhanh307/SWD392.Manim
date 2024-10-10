using SWD392.Manim.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Repositories.Entity
{
    public class OTP : BaseEntity
    {
        public string UserId {  get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiredAt { get; set; }
        public bool IsValid {  get; set; } = false;
    }
}
