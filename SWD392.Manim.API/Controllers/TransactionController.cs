using Microsoft.AspNetCore.Mvc;
using SWD392.Manim.Repositories.ViewModel.SolutionVM;
using SWD392.Manim.Repositories;
using SWD392.Manim.Services.Services;
using SWD392.Manim.Repositories.ViewModel.TransactionVM;

namespace SWD392.Manim.API.Controllers
{
    [Route("api/transaction")]
    [ApiController]


    public class TransactionController(ITransactionService transactionService) : ControllerBase
    {
        public readonly ITransactionService _transactionService = transactionService;


        [HttpGet]
        public async Task<IActionResult> GetTransactions(int index = 1, int pageSize = 10, string? id = null, string? nameSearch = null)
        {
            var result = await _transactionService.GetTransactions(index, pageSize, id, nameSearch);
            return Ok(new BaseResponseModel<PaginatedList<GetTransactionsVM>?>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(string id)
        {
            var result = await _transactionService.GetTransactionById(id);
            return Ok(new BaseResponseModel<GetTransactionsVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

    }
}
