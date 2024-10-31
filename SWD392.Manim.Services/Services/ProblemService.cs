using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using SWD392.Manim.Repositories.ViewModel.ProblemVM;


namespace SWD392.Manim.Services.Services
{
    public class ProblemService : IProblemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string RedisConnectionString;
        private readonly ConnectionMultiplexer Connection;
        private readonly RedisChannel Channel;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProblemService(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        public async Task PurchaseProblem(PurchaseProblemVM model)
        {
            Problem? problem = await _unitOfWork.GetRepository<Problem>().GetByIdAsync(model.ProblemId) ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Vấn đề không tồn tại!");
            Topic? topic = await _unitOfWork.GetRepository<Topic>().GetByIdAsync(problem.TopicId) ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Chủ đề không tồn tại!");

            List<string> parameterList = new();
            foreach (var item in model.PostPPVMs)
            {
                Parameter? parameter = await _unitOfWork.GetRepository<Parameter>().GetByIdAsync(item.ParameterId) ?? throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Tham so không tồn tại!");

                parameterList.Add(parameter.Symbol + ":" + item.Value);
            }
            string result = String.Join(",", parameterList);
            var subscriber = Connection.GetSubscriber();
            if (topic.Name.Equals("Con lắc lò xo"))
            {
                topic.Name = "Spring";
            }
            else if (topic.Name.Equals("Con lắc đơn"))
            {
                topic.Name = "Pendulum";
            }
            var inputParameterJson = $"{UserId};{problem.Id};{problem.Name};{topic.Name};{problem.Type};{result}";

            RedisValue redisValue = new RedisValue(inputParameterJson);
            await subscriber.PublishAsync(Channel, redisValue);
        }
        public async Task PostProblem(PostProblemVM model)
        {
            Problem? existedProblem = await _unitOfWork.GetRepository<Problem>().Entities.Where(s => s.Name == model.Name).FirstOrDefaultAsync();
            if (existedProblem != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên vấn đề đã tồn tại!");
            }
            Topic? topic = await _unitOfWork.GetRepository<Topic>().GetByIdAsync(model.TopicId); 
            Problem problem = _mapper.Map<Problem>(model);
            foreach (var item in model.PostPPVMs)
            {
                ProblemParameter pp = new()
                {
                    ParameterId = item.ParameterId,
                    ProblemId = problem.Id,
                    Value = 0,
                    CreatedAt = DateTime.Now,
                };
                await _unitOfWork.GetRepository<ProblemParameter>().InsertAsync(pp);
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

