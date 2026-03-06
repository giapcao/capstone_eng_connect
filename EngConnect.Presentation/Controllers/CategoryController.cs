using EngConnect.Application.UseCases.Categories.Common;
using EngConnect.Application.UseCases.Categories.CreateCategory;
using EngConnect.Application.UseCases.Categories.DeleteCategory;
using EngConnect.Application.UseCases.Categories.GetCategoryById;
using EngConnect.Application.UseCases.Categories.GetListCategory;
using EngConnect.Application.UseCases.Categories.UpdateCategory;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lý Category
/// </summary>
[ApiController]
[Route("api/categories")]
public class CategoryController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CategoryController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách Category
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListCategoryQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy thông tin Category theo ID
    /// </summary>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo mới Category
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryCommand command)
    {
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật Category
    /// </summary>
    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCategoryCommand command)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa Category
    /// </summary>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
