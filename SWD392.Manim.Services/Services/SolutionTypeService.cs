namespace SWD392.Manim.Services.Services
{
    public class SolutionTypeService : ISolutionTypeService
    {
        //      private readonly IMapper _mapper;
        //      private IUnitOfWork _unitOfWork;
        //      public SolutionTypeService(IMapper mapper, IUnitOfWork unitOfWork)
        //      {
        //          _mapper = mapper;
        //          _unitOfWork = unitOfWork;
        //      }
        //      public async Task<PaginatedList<GetSolutionTypesVM>?> GetSolutionTypes(int index, int pageSize, string? id, string? nameSearch)
        //      {
        //	IQueryable<SolutionType> query = _unitOfWork.GetRepository<SolutionType>().Entities.Where(s => !s.DeletedAt.HasValue);

        //	if (!string.IsNullOrWhiteSpace(id))
        //	{
        //		query = query.Where(lp => lp.Id.ToString().Contains(id));
        //	}

        //	if (!string.IsNullOrWhiteSpace(nameSearch))
        //	{
        //		query = query.Where(lp => lp.Name.Contains(nameSearch));
        //	}

        //	var resultQuery = await _unitOfWork.GetRepository<SolutionType>().GetPagging(query, index, pageSize);

        //	var responseItems = resultQuery.Items.Select(item => _mapper.Map<GetSolutionTypesVM>(item)).ToList();

        //	// Create paginated response
        //	var responsePaginatedList = new PaginatedList<GetSolutionTypesVM>(
        //		responseItems,
        //		resultQuery.TotalCount,
        //		resultQuery.PageNumber,
        //		resultQuery.TotalPages
        //	);
        //	return responsePaginatedList;
        //}
        //      public async Task<GetSolutionTypesVM?> GetSolutionTypeById(string id)
        //      {
        //	SolutionType? existedSolutionType = await _unitOfWork.GetRepository<SolutionType>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Chủ đề không tồn tại!");
        //	return _mapper.Map<GetSolutionTypesVM?>(existedSolutionType);
        //}
        //      public async Task PostSolutionType(PostSolutionTypeVM model)
        //      {
        //	SolutionType? existedSolutionType = await _unitOfWork.GetRepository<SolutionType>().Entities.Where(s => s.Name == model.Name).FirstOrDefaultAsync();
        //	if (existedSolutionType != null)
        //	{
        //		throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên Solution đã tồn tại");
        //	}
        //	// khai bao solutionType va time tao
        //	SolutionType solutionType = _mapper.Map<SolutionType>(model);
        //          solutionType.CreatedAt = DateTime.Now;

        //          await _unitOfWork.GetRepository<SolutionType>().InsertAsync(solutionType);
        //          await _unitOfWork.SaveAsync();
        //      }

        //      public async Task PutSolutionType(string id, PostSolutionTypeVM model)
        //      {
        //	SolutionType? existedSolutionType = await _unitOfWork.GetRepository<SolutionType>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Chủ đề không tồn tại!");

        //	_mapper.Map(model, existedSolutionType);
        //	existedSolutionType.UpdatedAt = DateTime.Now;
        //	await _unitOfWork.GetRepository<SolutionType>().UpdateAsync(existedSolutionType);
        //	await _unitOfWork.SaveAsync();
        //}
        //      public async Task DeleteSolutionType(string id)
        //      {
        //	SolutionType? existedSolutionType = await _unitOfWork.GetRepository<SolutionType>().GetByIdAsync(id) ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Chủ đề không tồn tại!");
        //	existedSolutionType.DeletedAt = DateTime.Now;
        //	await _unitOfWork.GetRepository<SolutionType>().UpdateAsync(existedSolutionType);
        //	await _unitOfWork.SaveAsync();
        //}


    }
}
