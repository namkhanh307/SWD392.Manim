using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.ViewModel.TransactionVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Services.Services
{
    public interface ITransactionService
    {
        Task<GetTransactionsVM> GetTransactionById(string id);
        Task<PaginatedList<GetTransactionsVM>?> GetTransactions(int index, int pageSize, string? id, string? nameSearch);

    }
}
