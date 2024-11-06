using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using SWD392.Manim.Repositories.ViewModel.DashboardVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Services.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashBoardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public GetDashBoardVM GetDashBoard()
        {
            return new GetDashBoardVM()
            {
                TotalSubjects = _unitOfWork.GetRepository<Subject>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalChapters = _unitOfWork.GetRepository<Chapter>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalProblems = _unitOfWork.GetRepository<Problem>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalSolutions = _unitOfWork.GetRepository<Solution>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalUsers = _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalSuccessTransactions = _unitOfWork.GetRepository<Transaction>().Entities.Where(s => !s.DeletedAt.HasValue && s.Status == Repositories.Enum.EnumStatus.Complete).Count(),
                TotalRevenue = _unitOfWork.GetRepository<Transaction>().Entities.Where(s => !s.DeletedAt.HasValue && s.Status == Repositories.Enum.EnumStatus.Complete).Sum(s => s.Amount),
            };
        }
    }
}
