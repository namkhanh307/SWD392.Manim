using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using SWD392.Manim.Repositories.ViewModel.ParameterVM;

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
        private string UserId => Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);

        public async Task<PaginatedList<GetParametersVM>?> GetParameters(int index, int pageSize, string? id, string? nameSearch, string? topicId)
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
            if (!string.IsNullOrWhiteSpace(topicId))
            {
                query = query.Where(lp => lp.TopicId == topicId);
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
        public async Task PostParameter(PostParameterVM model)
        {
            Guid id;
            if (Guid.TryParse(UserId, out id))
            {
                ApplicationUser? user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(u => u.Id.Equals(id)).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Tài khoản không tồn tại!");
            }
            Parameter? existedParameter = await _unitOfWork.GetRepository<Parameter>().Entities.Where(p => !p.DeletedAt.HasValue && p.Name == model.Name && p.TopicId == model.TopicId).FirstOrDefaultAsync();
            if (existedParameter != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên biến đã tồn tại");
            }
            Topic? topic = await _unitOfWork.GetRepository<Topic>().Entities.Where(p => !p.DeletedAt.HasValue && p.Id == model.TopicId).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Chủ đề không tồn tại");
            Parameter parameter = _mapper.Map<Parameter>(model);
            //.ProblemId = problemTypeId;
            var subscriber = Connection.GetSubscriber();
            //var inputParameterJson = $"{problemTypeId.ToString()};{parameter.Unit}";

            //RedisValue redisValue = new RedisValue(inputParameterJson);
            //await subscriber.PublishAsync(Channel, redisValue);

            await _unitOfWork.GetRepository<Parameter>().InsertAsync(parameter);
            await _unitOfWork.SaveAsync();
        }

        public async Task PutParameter(string id, PostParameterVM model)
        {
            Parameter? existedParameter = await _unitOfWork.GetRepository<Parameter>().Entities.Where(s => s.Id == id && !s.DeletedAt.HasValue).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Biến không tồn tại!");
            Parameter? existedParameterName = await _unitOfWork.GetRepository<Parameter>().Entities.Where(p => !p.DeletedAt.HasValue && p.Name == model.Name && p.TopicId == model.TopicId).FirstOrDefaultAsync();
            if (existedParameterName != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên biến đã tồn tại");
            }
            Topic? topic = await _unitOfWork.GetRepository<Topic>().Entities.Where(p => !p.DeletedAt.HasValue && p.Id == model.TopicId).FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Chủ đề không tồn tại"); ;
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
