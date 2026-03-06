using EngConnect.Application.UseCases.Users.ChangePassword;
using EngConnect.Application.UseCases.Users.Common;
using EngConnect.Application.UseCases.Users.CreateUser;
using EngConnect.Application.UseCases.Users.ForgotPassword;
using EngConnect.Application.UseCases.Users.GetListUser;
using EngConnect.Application.UseCases.Users.GetUserById;
using EngConnect.Application.UseCases.Users.ResetPassword;
using EngConnect.Application.UseCases.Users.UpdateUser;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Presentation.Controllers;
using EngConnect.BuildingBlock.Presentation.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lí tài khoản người dùng
/// </summary>
[ApiController]
[Route("api/users")]
public class UserController: BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public UserController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }
    
    /// <summary>
    /// Lấy danh sách người dùng (Có phân trang)
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetUserResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListUserAsync([FromQuery] GetListUserQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy thông tin người dùng theo ID
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetUserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid userId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery(userId);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo mới người dùng (dùng cho quản trị viên)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    // [Authorize(Roles = nameof(UserRoleEnum.Admin))]
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserCommand command)
    {
        var res = await _commandDispatcher.DispatchAsync(command);
        return FromResult(res);
    }

    /// <summary>
    /// Cập nhật thông tin người dùng
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPatch("{userId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid userId,
        [FromBody] UpdateUserCommand command)
    {
        command.UserId = userId;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Đổi mật khẩu người dùng
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPatch("change-password")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordCommand command)
    {
        var userId = User.GetUserId();
        var commandWithId = command with { UserId = userId.Value };
        var result = await _commandDispatcher.DispatchAsync(commandWithId);
        return FromResult(result);
    }
    
    /// <summary>
    /// Quên mật khẩu (Gửi email đặt lại mật khẩu)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("forgot-password")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordCommand command)
    {
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Đặt lại mật khẩu (Cần có mã xác thực từ email)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordCommand command)
    {
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}