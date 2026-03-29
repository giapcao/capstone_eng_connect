using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.Application.UseCases.Authentication.LoginByUser;
using EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth;
using EngConnect.Application.UseCases.Authentication.Logout;
using EngConnect.Application.UseCases.Authentication.RefreshToken;
using EngConnect.Application.UseCases.Authentication.RegisterStudent;
using EngConnect.Application.UseCases.Authentication.RegisterTutor;
using EngConnect.Application.UseCases.Authentication.RegisterUser;
using EngConnect.Application.UseCases.Authentication.RegisterUserStaff;
using EngConnect.Application.UseCases.Authentication.VerifyEmail;
using EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Presentation.Controllers;
using EngConnect.BuildingBlock.Presentation.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// API xác thực và phân quyền
/// </summary>
[ApiController]
[Route("api/auth/v1")]
public class AuthController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly RedirectUrlSettings _redirectUrlSettings;

    public AuthController(ICommandDispatcher commandDispatcher,
        IOptions<RedirectUrlSettings> redirectUrlSettings)
    {
        _commandDispatcher = commandDispatcher;
        _redirectUrlSettings = redirectUrlSettings.Value;
    }

    /// <summary>
    /// Đăng ký người dùng mới
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserCommand command)
    {
        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }

    /// <summary>
    /// Xác thực email người dùng
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("verify-email")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyEmailAsync([FromBody] VerifyEmailCommand command)
    {
        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }

    /// <summary>
    /// Đăng nhập người dùng
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginByUserCommand command)
    {
        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }


    /// <summary>
    /// Đăng xuất người dùng
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost("Logout")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> LogoutAsync()
    {
        var accessToken = HttpContext.GetAccessToken();

        if (ValidationUtil.IsNullOrEmpty(accessToken))
        {
            return Unauthorized();
        }

        var command = new LogoutCommand
        {
            AccessToken = accessToken
        };

        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }

    /// <summary>
    /// Làm mới access token bằng refresh token 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenCommand command)
    {
        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }

    /// <summary>
    /// Đăng nhập bằng tài khoản Google và redirect về FE với token
    /// </summary>
    /// <returns></returns>
    [HttpGet("google-login")]
    [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme)]
    public async Task<IActionResult> LoginByGoogle()
    {
        var authenticationResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!authenticationResult.Succeeded)
        {
            return Unauthorized();
        }

        var command = new LoginWithGoogleOAuthCommand
        {
            Principal = authenticationResult.Principal
        };

        var result = await _commandDispatcher.DispatchAsync(command);
        var redirectUrl = _redirectUrlSettings.GoogleLoginFailedUrl;

        if (result.IsSuccess && ValidationUtil.IsNotNullOrEmpty(result.Value))
        {
            redirectUrl = result.Value;
        }

        //Sign out of Google authentication scheme to clear the cookie
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect(redirectUrl);
    }

    [HttpPost("google-login/verify")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyGoogleLoginAsync([FromBody] VerifyGoogleLoginCommand command)
    {
        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }

    /// <summary>
    /// Đăng ký student mới
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("register-student")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterStudentAsync([FromBody] RegisterStudentCommand command)
    {
        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }

    /// <summary>
    /// Đăng ký tutor mới
    /// </summary>
    [Authorize]
    [HttpPost("register-tutor")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterTutorAsync([FromForm] RegisterTutorRequest request)
    {
        var userId = User.GetUserId();
        if (ValidationUtil.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var command = new RegisterTutorCommand
        {
            UserId = userId.Value,
            Headline = request.Headline,
            Bio = request.Bio,
            YearsExperience = request.YearsExperience,
            IntroVideoFile = request.IntroVideoFile != null ? new FileUpload
            {
                FileName = request.IntroVideoFileName ?? request.IntroVideoFile.FileName,
                ContentType = request.IntroVideoFile.ContentType ?? "video/mp4",
                Length = request.IntroVideoFile.Length,
                Content = request.IntroVideoFile.OpenReadStream()
            } : null,
            CvFile = request.CvFile != null ? new FileUpload
            {
                FileName = request.CvFileName ?? request.CvFile.FileName,
                ContentType = request.CvFile.ContentType ?? "application/pdf",
                Length = request.CvFile.Length,
                Content = request.CvFile.OpenReadStream()
            } : null
        };

        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }

    /// <summary>
    /// Đăng ký Staff mới (không gửi email xác thực)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("register-staff")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<RegisterUserStaffResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterUserStaffAsync([FromBody] RegisterUserStaffCommand command)
    {
        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }
    
    
}