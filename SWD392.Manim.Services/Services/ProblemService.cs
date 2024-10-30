using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using SWD392.Manim.Repositories.ViewModel.ProblemVM;


namespace SWD392.Manim.Services.Services
{
    public class ProblemService(IMapper mapper, IUnitOfWork unitOfWork) : IProblemService
    {
        private readonly IMapper _mapper = mapper;
        private IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PaginatedList<GetProblemsVM>?> GetProblems(int index, int pageSize, string? id, string? nameSearch)
        {
            IQueryable<Problem> query = _unitOfWork.GetRepository<Problem>().Entities.Where(s => !s.DeletedAt.HasValue);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(lp => lp.Id.ToString().Contains(id));
            }

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(lp => lp.Name.Contains(nameSearch));
            }

            var resultQuery = await _unitOfWork.GetRepository<Problem>().GetPagging(query, index, pageSize);

            var responseItems = resultQuery.Items.Select(item => _mapper.Map<GetProblemsVM>(item)).ToList();

            // Create paginated response
            var responsePaginatedList = new PaginatedList<GetProblemsVM>(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.TotalPages
            );
            return responsePaginatedList;
        }

        public async Task<GetProblemsVM?> GetProblemById(string id)
        {
            Problem? existedProblem = await _unitOfWork.GetRepository<Problem>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Vấn đề không tồn tại!");
            return _mapper.Map<GetProblemsVM?>(existedProblem);
        }
        public async Task PostProblem(PostProblemVM model)
        {
            Problem? existedProblem = await _unitOfWork.GetRepository<Problem>().Entities.Where(s => s.Name == model.Name).FirstOrDefaultAsync();
            if (existedProblem != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên vấn đề đã tồn tại!");
            }
            Problem problem = _mapper.Map<Problem>(model);
            foreach (var item in model.PostPPVMs)
            {
                ProblemParameter pp = new()
                {
                    ParameterId = item.ParameterId,
                    ProblemId = problem.Id,
                    Value = item.Value,
                    CreatedAt = DateTime.Now,
                };
            }
            problem.CreatedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Problem>().InsertAsync(problem);
            await _unitOfWork.SaveAsync();
        }

        public async Task PutProblem(string id, PostProblemVM model)
        {
            Problem? existedProblem = await _unitOfWork.GetRepository<Problem>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Vấn đề không tồn tại!");

            _mapper.Map(model, existedProblem);
            ICollection<ProblemParameter> pps = await _unitOfWork.GetRepository<ProblemParameter>().Entities.Where(s => s.ProblemId == id && !s.DeletedAt.HasValue).ToListAsync();
            foreach (var item in pps)
            {
                await _unitOfWork.GetRepository<ProblemParameter>().DeleteAsync(item);
                await _unitOfWork.SaveAsync();
            }
            foreach (var item in model.PostPPVMs)
            {
                ProblemParameter pp = new()
                {
                    ParameterId = item.ParameterId,
                    ProblemId = id,
                    Value = item.Value,
                    CreatedAt = DateTime.Now,
                };
            }

            existedProblem.UpdatedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Problem>().UpdateAsync(existedProblem);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteProblem(string id)
        {
            Problem? existedProblem = await _unitOfWork.GetRepository<Problem>().GetByIdAsync(id) ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Vấn đề không tồn tại!");
            ICollection<ProblemParameter> pps = await _unitOfWork.GetRepository<ProblemParameter>().Entities.Where(s => s.ProblemId == id && !s.DeletedAt.HasValue).ToListAsync();
            foreach (var item in pps)
            {
                await _unitOfWork.GetRepository<ProblemParameter>().DeleteAsync(item);
                await _unitOfWork.SaveAsync();
            }
            existedProblem.DeletedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Problem>().UpdateAsync(existedProblem);
            await _unitOfWork.SaveAsync();
        }


    }
}

