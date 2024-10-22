using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Repositories.ViewModel.Wallet
{
    public class ExtendedPaymentInfo
    {
        public int Amount { get; set; }
        public string Description { get; set; }
        public string BuyerName { get; set; }
        public string BuyerPhone { get; set; }
        public string BuyerEmail { get; set; }
        public string Status { get; set; }
    }
}
