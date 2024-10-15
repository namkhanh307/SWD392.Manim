using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Implement;
using SWD392.Manim.Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly IUnitOfWork _unitOfWork;

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
                Guid inputParameterId = Guid.Parse(message.ToString().Split(";")[0]);
                //string userId = message.ToString().Split(";")[1];
                string solutionLink = message.ToString().Split(";")[2];

                Transaction transaction = new Transaction()
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = 10000,
                    //Description = $"Transaction for generating for input parameter {inputParameterId}",
                    CreatedAt = DateTime.Now,
                    //UserId = userId
                };

                Solution solution = new Solution()
                {
                    Id = Guid.NewGuid().ToString(),
                    //Link = solutionLink,
                    //Description = $"Solution for input parameter {inputParameterId}",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    //Active = true,
                    //InputParameterId = inputParameterId,
                    //TransactionId = transaction.Id
                };

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    await unitOfWork.GetRepository<Solution>().InsertAsync(solution);
                    await unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);

                    var wallet = await unitOfWork.GetRepository<Wallet>()
                        .Entities.Where(w => w.UserId == id)
                        .FirstOrDefaultAsync();

                    if (wallet == null)
                    {
                        throw new Exception("Ví không tồn tại cho người dùng này.");
                    }

                    wallet.Balance -= transaction.Amount;

                    await unitOfWork.SaveAsync();
                }

            });
        }
    }
}

