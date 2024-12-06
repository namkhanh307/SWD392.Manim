using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Repositories.ViewModel.DashboardVM
{
    public class GetDashBoardVM
    {
        //Số lượng user, số lượng transaction (thành công) / tháng, số lượng solution đã bán, số lượng problem có trên hệ thống, số môn học => chapter
        public int TotalUsers { get; set; } 
        public int TotalSuccessTransactions {  get; set; }
        public int TotalSolutions { get; set; }
        public int TotalProblems { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalChapters { get; set; }
        public decimal? TotalRevenue { get; set; }
    }
}
