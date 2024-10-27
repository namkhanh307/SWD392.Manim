using AutoMapper;
using SWD392.Manim.Repositories.Repository.Interface;

namespace SWD392.Manim.Services.Services
{
	public class ProblemServiceBase
	{
		private readonly IMapper _mapper;
		private IUnitOfWork _unitOfWork;
	}
}