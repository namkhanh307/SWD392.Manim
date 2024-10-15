using AutoMapper;
using SWD392.Manim.Repositories.Repository.Interface;
using Microsoft.AspNetCore.Http;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.ViewModel.ParameterVM;
using SWD392.Manim.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace SWD392.Manim.Services.Services
{
    public class ParameterService : IParameterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string RedisConnectionString;
        private readonly ConnectionMultiplexer Connection;
        private readonly RedisChannel Channel;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ParameterService(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            RedisConnectionString = configuration.GetSection("Redis").GetSection("ConnectionString").Value;
            Connection = ConnectionMultiplexer.Connect(RedisConnectionString);
            Channel = new RedisChannel(configuration.GetSection("Redis").GetSection("Channel1").Value, RedisChannel.PatternMode.Literal);
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PaginatedList<GetParametersVM>?> GetParameters(int index, int pageSize, string? id, string? nameSearch)
        {
            IQueryable<Parameter> query = _unitOfWork.GetRepository<Parameter>().Entities.Where(s => !s.DeletedAt.HasValue);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(lp => lp.Id.ToString().Contains(id));
            }

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(lp => lp.Name.Contains(nameSearch));
            }

            var resultQuery = await _unitOfWork.GetRepository<Parameter>().GetPagging(query, index, pageSize);

            var responseItems = resultQuery.Items.Select(item => _mapper.Map<GetParametersVM>(item)).ToList();

            // Create paginated response
            var responsePaginatedList = new PaginatedList<GetParametersVM>(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.TotalPages
            );
            return responsePaginatedList;
        }
        public async Task<GetParametersVM?> GetParameterById(string id)
        {
            Parameter? existedParam = await _unitOfWork.GetRepository<Parameter>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Biến không tồn tại!");
            return _mapper.Map<GetParametersVM?>(existedParam);
        }
        public async Task PostParameter(PostParameterVM model, string problemTypeId)
        {
            string userId = Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            Guid id;
            ApplicationUser? user = null;
            if (Guid.TryParse(userId, out id))
            {
                user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(u => u.Id.Equals(id)).FirstOrDefaultAsync();
            }

            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Tài khoản không tồn tại!");
            }
            var problemType = await _unitOfWork.GetRepository<Problem>().Entities.Where(p => p.Id == problemTypeId).FirstOrDefaultAsync();
            if (problemType == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Problem Type không tồn tại");
            }
            Parameter? existedParameter = await _unitOfWork.GetRepository<Parameter>().Entities.Where(p => !p.DeletedAt.HasValue && p.Name == model.Name).FirstOrDefaultAsync();
            if (existedParameter != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên biến đã tồn tại");
            }

            Parameter parameter = _mapper.Map<Parameter>(model);
            parameter.ProblemId = problemTypeId;
            var subscriber = Connection.GetSubscriber();
            var inputParameterJson = $"{problemTypeId.ToString()};{parameter.Unit}";

            RedisValue redisValue = new RedisValue(inputParameterJson);
            await subscriber.PublishAsync(Channel, redisValue);

            await _unitOfWork.GetRepository<Parameter>().InsertAsync(parameter);
            await _unitOfWork.SaveAsync();
        }

        public async Task PutParameter(string id, PostParameterVM model)
        {
            Parameter? existedParameter = await _unitOfWork.GetRepository<Parameter>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Biến không tồn tại!");
            Parameter? existedParameterName = await _unitOfWork.GetRepository<Parameter>().Entities.Where(p => !p.DeletedAt.HasValue && p.Name == model.Name).FirstOrDefaultAsync();
            if (existedParameterName != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên biến đã tồn tại");
            }
            _mapper.Map(model, existedParameter);
            existedParameter.UpdatedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Parameter>().UpdateAsync(existedParameter);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteParameter(string id)
        {
            Parameter? existedParameter = await _unitOfWork.GetRepository<Parameter>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Biến không tồn tại!");
            existedParameter.DeletedAt = DateTime.Now;
            await _unitOfWork.GetRepository<Parameter>().UpdateAsync(existedParameter);
            await _unitOfWork.SaveAsync();
        }


    }
}
