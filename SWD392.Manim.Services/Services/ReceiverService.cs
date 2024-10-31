using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using System.Runtime.InteropServices;

namespace SWD392.Manim.Services.Services
{
    public class ReceiverService : BackgroundService
    {
        private readonly string ConnectionString;
        private readonly IConnectionMultiplexer Connection;
        private readonly RedisChannel Channel;
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private const string Channel = "Channel1";

        public ReceiverService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.serviceScopeFactory = serviceScopeFactory;
            ConnectionString = this.configuration.GetSection("Redis").GetSection("ConnectionString").Value;
            Connection = ConnectionMultiplexer.Connect(ConnectionString);
            Channel = new RedisChannel(this.configuration.GetSection("Redis").GetSection("Channel2").Value, RedisChannel.PatternMode.Literal);
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = Connection.GetSubscriber();
            await subscriber.SubscribeAsync(Channel, async (channel, message) =>
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    string input = message.ToString();
                string[] parameters = input.Split(";");

                Guid userId = Guid.Parse(parameters[0]);
                string problemId = parameters[1];
                string type = parameters[2];
                string parameterList = parameters[3];
                string url = parameters[4];

                var parameterMap = new Dictionary<string, string>();
                string descriptionParam = "";
                foreach (var part in parameterList.Split(','))
                {
                    var keyValue = part.Split(':', 2); 
                    if (keyValue.Length == 2) 
                    {
                        parameterMap[keyValue[0]] = keyValue[1];
                        descriptionParam += keyValue[0] + ": " + keyValue[1] + "; " ;

                    }
                }
                Wallet? existedWallet = await unitOfWork.GetRepository<Wallet>().Entities.Where(u => u.UserId.Equals(userId)).FirstOrDefaultAsync() ??
                    throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Vi không tồn tại!");
                ApplicationUser? user = await unitOfWork.GetRepository<ApplicationUser>().Entities.Where(u => u.Id.Equals(userId)).FirstOrDefaultAsync() ??
                    throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Tài khoản không tồn tại!");
                Problem? problem = await unitOfWork.GetRepository<Problem>().GetByIdAsync(problemId) ??
                    throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Van de không tồn tại!");
         
                Transaction transaction = new Transaction()
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = problem.Price,
                    CreatedAt = DateTime.Now,
                    WalletId = existedWallet.Id,
                    BillingDate = DateTime.Now,  
                };

                Solution solution = new Solution()
                {
                    Url = url,
                    Description = $"Solution for {problem.Description} with {descriptionParam}",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ProblemId = problemId,
                    UserId = userId,
                    Name = problem.Name,                   
                };
                    await unitOfWork.GetRepository<Solution>().InsertAsync(solution);
                    await unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
                    existedWallet.Balance -= transaction.Amount;
                    await unitOfWork.GetRepository<Wallet>().UpdateAsync(existedWallet);
                    await unitOfWork.SaveAsync();
                }

            });
        }
    }
}

