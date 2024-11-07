using AutoMapper;
using Microsoft.AspNetCore.Http;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using SWD392.Manim.Repositories.ViewModel.DashboardVM;
using SWD392.Manim.Repositories.ViewModel.ProblemParameterVM;
using SWD392.Manim.Repositories.ViewModel.ProblemVM;
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
        private readonly IMapper _mapper;

        public DashBoardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }

        public GetDashBoardVM GetDashBoard()
        {
            List<Problem> problems = _unitOfWork.GetRepository<Problem>().Entities.Where(p => !p.DeletedAt.HasValue && p.Status == false).ToList();
            List<GetProblemsVM> problemsVMs = _mapper.Map<List<Problem>, List<GetProblemsVM>>(problems);
            foreach (var item in problemsVMs)
            {
                Problem? problem = _unitOfWork.GetRepository<Problem>().Entities.Where(s => !s.DeletedAt.HasValue).FirstOrDefault() ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Vấn đề không tồn tại!");
                item.GetPPVM = problem.ProblemParameters.Select(pp => new GetPPVM
                {
                    ParameterId = pp.ParameterId,
                    Value = pp.Value,
                    Symbol = pp.Parameter?.Symbol
                }).ToList();
            }
            return new GetDashBoardVM()
            {
                TotalSubjects = _unitOfWork.GetRepository<Subject>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalChapters = _unitOfWork.GetRepository<Chapter>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalProblems = _unitOfWork.GetRepository<Problem>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalSolutions = _unitOfWork.GetRepository<Solution>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalUsers = _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(s => !s.DeletedAt.HasValue).Count(),
                TotalSuccessTransactions = _unitOfWork.GetRepository<Transaction>().Entities.Where(s => !s.DeletedAt.HasValue && s.Status == Repositories.Enum.EnumStatus.Complete).Count(),
                TotalRevenue = _unitOfWork.GetRepository<Transaction>().Entities.Where(s => !s.DeletedAt.HasValue && s.Status == Repositories.Enum.EnumStatus.Complete && s.Amount > 0).Sum(s => s.Amount),
                WaitingProblems = problemsVMs
            };
        }
    }
}
