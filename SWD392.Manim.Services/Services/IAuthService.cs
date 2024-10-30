using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.ViewModel.AuthVM;
using SWD392.Manim.Repositories.ViewModel.UserVM;

namespace SWD392.Manim.Services.Services
{
    public interface IAuthService
    {
        Task<GetSignInVM> SignIn(PostSignInVM model);
        Task SignUp(PostSignUpVM model);
        GetTokenVM GenerateTokens(ApplicationUser user, string role);
    }
}
