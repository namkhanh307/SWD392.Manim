using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.ViewModel.AuthVM;
using SWD392.Manim.Repositories.ViewModel.ChapterVM;
using SWD392.Manim.Repositories.ViewModel.UserVM;
using SWD392.Manim.Services.Services;

namespace SWD392.Manim.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, IUserService userService, IGoogleAuthenticationService googleAuthenticationService) : ControllerBase
    {
        public readonly IAuthService _authService = authService;
        private readonly IUserService _userService = userService;
        private readonly IGoogleAuthenticationService _googleAuthenticationService = googleAuthenticationService;


        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(PostSignUpVM model)
        {
            await _authService.SignUp(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Đăng ký thành công"));
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(PostSignInVM model)
        {
            GetSignInVM result = await _authService.SignIn(model);
            return Ok(new BaseResponseModel<GetSignInVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        [HttpGet("google-auth/login")]
        public IActionResult Login()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("SignInGoogle", "Auth")  // Generates the absolute path for redirect
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(new BaseResponseModel<GetUserVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        [HttpGet("google-auth/signin-google")]
        public async Task<IActionResult> SignInGoogle()
        {
            try
            {
                GoogleAuthVM googleAuthResponse = await _googleAuthenticationService.AuthenticateGoogleUser(HttpContext);

                var checkAccount = await _userService.GetAccountByEmail(googleAuthResponse.Email);
                if (!checkAccount)
                {
                    var response = await _userService.CreateNewUserAccountByGoogle(googleAuthResponse);
                    if (response == null)
                    {
                        return Problem("Account creation failed.");
                    }
                }

                var token = await _userService.CreateTokenByEmail(googleAuthResponse.Email);
                googleAuthResponse.Token = token;

                // Sanitize inputs if necessary and inject into HTML
                var email = System.Web.HttpUtility.JavaScriptStringEncode(googleAuthResponse.Email);
                var name = System.Web.HttpUtility.JavaScriptStringEncode(googleAuthResponse.Name);
                var accessToken = System.Web.HttpUtility.JavaScriptStringEncode(token.AccessToken);
                var refreshToken = System.Web.HttpUtility.JavaScriptStringEncode(token.RefreshToken);

                // HTML response with postMessage to return token and close the window
                var htmlContent = $@"
<html>
<body>
    <script type='text/javascript'>
        window.opener.postMessage({{
            data: {{
                token: {{
                    accessToken: '{accessToken}',
                    refreshToken: '{refreshToken}'
                }},
                email: '{email}',
                name: '{name}'
            }}
        }}, '{Request.Scheme}://{Request.Host}');

    </script>
</body>
</html>";

                return Content(htmlContent, "text/html");
            }
            catch (Exception ex)
            {
                // Log the exception and provide user feedback
                return Problem("An error occurred during Google sign-in.");
            }
        }
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(PutUserVM model)
        {
            await _userService.UpdateProfile(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Cập nhật thành công"));
        }
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            await _userService.ChangePassword(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Cập nhật thành công"));
        }
        [HttpPost("Verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] PostVerifyVM postVerifyVM)
        {
            // Gọi phương thức dịch vụ để xác thực OTP
            await _authService.VerifyOtp(postVerifyVM.UserId, postVerifyVM.Otp);

            return Ok(new BaseResponseModel<bool>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: true));
        }
    }
}
