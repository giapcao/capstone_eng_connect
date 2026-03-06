using EngConnect.Application.UseCases.Permissions.Common;
using EngConnect.Application.UseCases.Permissions.CreatePermission;
using EngConnect.Application.UseCases.Permissions.DeletePermission;
using EngConnect.Application.UseCases.Permissions.GetListPermission;
using EngConnect.Application.UseCases.Permissions.GetPermissionById;
using EngConnect.Application.UseCases.Permissions.UpdatePermission;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lý Permission
/// </summary>
[ApiController]
[Route("api/permissions")]
public class PermissionController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public PermissionController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách Permission
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetPermissionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListPermissionQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy thông tin Permission theo ID
    /// </summary>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetPermissionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetPermissionByIdQuery(id);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo mới Permission
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePermissionCommand command)
    {
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật Permission
    /// </summary>
    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdatePermissionCommand command)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa Permission
    /// </summary>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var command = new DeletePermissionCommand(id);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
