using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392.Manim.Repositories.ViewModel.ChapterVM;
using SWD392.Manim.Repositories;
using SWD392.Manim.Services.Services;
using SWD392.Manim.Repositories.ViewModel.DashboardVM;

namespace SWD392.Manim.API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashBoardService _dashBoardService;

        public DashboardsController(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }

        [HttpGet]
        public IActionResult GetDashBoard()
        {
            var result = _dashBoardService.GetDashBoard();
            return Ok(new BaseResponseModel<GetDashBoardVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
    }
}
