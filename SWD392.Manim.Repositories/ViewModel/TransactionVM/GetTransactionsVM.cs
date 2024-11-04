using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Repositories.ViewModel.TransactionVM
{
    public class GetTransactionsVM
    {
        public string? WalletId { get; set; } = string.Empty;
        public string? SolutionId { get; set; } = string.Empty;
        public string? DepositId { get; set; } = string.Empty;
        public decimal? Amount { get; set; } 
        public long OrderCode { get; set; }
        public EnumStatus Status { get; set; }

    }
}

