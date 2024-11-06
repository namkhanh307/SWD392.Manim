using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using SWD392.Manim.Repositories.ViewModel.SolutionVM;

namespace SWD392.Manim.Services.Services
{
    public class SolutionService : ISolutionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public SolutionService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }
        private string UserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
        public async Task<PaginatedList<GetSolutionsVM>?> GetSolutions(int index, int pageSize, string? id, string? nameSearch)
        {
            IQueryable<Solution> query = _unitOfWork.GetRepository<Solution>().Entities.Where(s => !s.DeletedAt.HasValue);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(lp => lp.Id.ToString().Contains(id));
            }

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(lp => lp.Name.Contains(nameSearch));
            }

            var resultQuery = await _unitOfWork.GetRepository<Solution>().GetPagging(query, index, pageSize);

            var responseItems = resultQuery.Items.Select(item => _mapper.Map<GetSolutionsVM>(item)).ToList();

            // Create paginated response
            var responsePaginatedList = new PaginatedList<GetSolutionsVM>(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.TotalPages
            );
            return responsePaginatedList;
        }
        public async Task<GetSolutionsVM?> GetSolutionById(string id)
        {
            Solution? existedSolution = await _unitOfWork.GetRepository<Solution>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Giải pháp không tồn tại!");
            return _mapper.Map<GetSolutionsVM?>(existedSolution);
        }

        public async Task PostSolution(PostSolutionVM model)
        {
            Solution? existedSolution = await _unitOfWork.GetRepository<Solution>().Entities.Where(s => s.Name == model.Name).FirstOrDefaultAsync();
            if (existedSolution != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên môn học đã tồn tại!");
            }
            Solution solution = _mapper.Map<Solution>(model);
            solution.CreatedAt = DateTime.Now;

            await _unitOfWork.GetRepository<Solution>().InsertAsync(solution);
            await _unitOfWork.SaveAsync();
        }

        public async Task PutSolution(string id, PostSolutionVM model)
        {
            Solution? existedSolution = await _unitOfWork.GetRepository<Solution>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Giải pháp không tồn tại!");

            _mapper.Map(model, existedSolution);
            existedSolution.UpdatedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Solution>().UpdateAsync(existedSolution);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteSolution(string id)
        {
            Solution? existedSolution = await _unitOfWork.GetRepository<Solution>().GetByIdAsync(id) ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Giải pháp không tồn tại!");
            existedSolution.DeletedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Solution>().UpdateAsync(existedSolution);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<GetSolutionsVM?>> GetSolutionByProblemId(string problemId)
        {
            Guid id;
            Guid.TryParse(UserId, out id);
            IEnumerable<Solution>? existedSolution = await _unitOfWork.GetRepository<Solution>().Entities.Where(s => s.UserId == id && s.ProblemId == problemId && !s.DeletedAt.HasValue).ToListAsync();
            if (existedSolution == null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Bạn chưa mua giải pháp nào!");
            }
            return _mapper.Map<IEnumerable<GetSolutionsVM?>>(existedSolution);
        }
    }
}
