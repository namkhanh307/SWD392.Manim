using AutoMapper;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.ViewModel.TransactionVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Manim.Services.Mapper
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile() 
        {
            CreateMap<Transaction, GetTransactionsVM>().ReverseMap();
        }
    }
}
