using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.ViewModel.AuthVM;
using SWD392.Manim.Repositories.ViewModel.UserVM;

namespace SWD392.Manim.Services.Services
{
    public interface IAuthService
    {
        Task<GetSignInVM> SignIn(PostSignInVM model);
        Task<string> SignUp(PostSignUpVM model);
        GetTokenVM GenerateTokens(ApplicationUser user, string role);
        Task<bool> VerifyOtp(string UserId, string otpCheck);
    }
}
