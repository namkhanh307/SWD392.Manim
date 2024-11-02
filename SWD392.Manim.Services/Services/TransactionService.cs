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

namespace SWD392.Manim.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork;

        public TransactionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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

        public async Task<Repositories.PaginatedList<GetTransactionsVM>?> GetTransactions(int index, int pageSize, string? id, string? nameSearch)
        {
            IQueryable<Transaction> query = _unitOfWork.GetRepository<Transaction>().Entities
                .Where(s => !s.DeletedAt.HasValue);

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(lp => lp.Id.ToString().Contains(id));
            }

            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                // Uncomment và chỉnh sửa nếu cần lọc theo tên
                // query = query.Where(lp => lp.Name.Contains(nameSearch));
            }

            // Sắp xếp theo thứ tự mới nhất (giảm dần theo CreatedAt)
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
