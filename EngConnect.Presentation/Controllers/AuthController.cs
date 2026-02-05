using EngConnect.Application.UseCases.Authentication.LoginByUser;
using EngConnect.Application.UseCases.Authentication.Logout;
using EngConnect.Application.UseCases.Authentication.RefreshToken;
using EngConnect.Application.UseCases.Authentication.RegisterUser;
using EngConnect.Application.UseCases.Authentication.VerifyEmail;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Presentation.Controllers;
using EngConnect.BuildingBlock.Presentation.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// API xác thực và phân quyền
/// </summary>
[ApiController]
[Route("api/auth/v1")]
public class AuthController: BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    public AuthController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
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
        var res =  await _commandDispatcher.DispatchAsync(command);
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
}