using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.ViewModel.SolutionVM;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.ViewModel.TransactionVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SWD392.Manim.Repositories.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWD392.Manim.Repositories.Enum;

namespace SWD392.Manim.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetTransactionsVM> GetTransactionById(string id)
        {
            // Lấy transaction theo id và sắp xếp theo CreatedAt giảm dần
            Transaction? existedSolution = await _unitOfWork.GetRepository<Transaction>().Entities
                .Where(s => s.Id == id && !s.DeletedAt.HasValue)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Giao dịch không tồn tại!");

            return _mapper.Map<GetTransactionsVM?>(existedSolution);
        }

        private string UserId => Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);

        public async Task<PaginatedList<GetTransactionsVM>?> GetTransactions(int index, int pageSize)
        {
            Guid uid;
            ApplicationUser? user = null;
            if (Guid.TryParse(UserId, out uid))
            {
                user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(u => u.Id.Equals(uid)).FirstOrDefaultAsync();
            }
            var wallet = await _unitOfWork.GetRepository<Wallet>().Entities.Where(w => w.UserId == user.Id).FirstOrDefaultAsync();
            IQueryable<Transaction> query = _unitOfWork.GetRepository<Transaction>().Entities
                .Where(s => !s.DeletedAt.HasValue && s.WalletId.Equals(wallet.Id) && s.Status.Equals(EnumStatus.Complete));

            query = query.OrderByDescending(t => t.CreatedAt);

            var resultQuery = await _unitOfWork.GetRepository<Transaction>().GetPagging(query, index, pageSize);
            var responseItems = resultQuery.Items.Select(item => _mapper.Map<GetTransactionsVM>(item)).ToList();

            // Tạo danh sách phân trang cho phản hồi
            var responsePaginatedList = new PaginatedList<GetTransactionsVM>(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.TotalPages
            );
            return responsePaginatedList;
        }
    }
}
