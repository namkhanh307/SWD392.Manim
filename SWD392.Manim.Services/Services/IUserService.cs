using SWD392.Manim.Repositories.ViewModel.AuthVM;
using SWD392.Manim.Repositories.ViewModel.UserVM;

namespace SWD392.Manim.Services.Services
{
    public interface IUserService
    {
        Task<GetTokenVM> CreateTokenByEmail(string email);
        Task<bool> GetAccountByEmail(string email);
        Task<GetSignInByGoogleVM> CreateNewUserAccountByGoogle(GoogleAuthVM response);
        Task UpdateProfile(PutUserVM model);
        Task ChangePassword(ChangePasswordVM model);

    }
}
