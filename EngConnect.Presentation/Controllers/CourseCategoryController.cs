using EngConnect.Application.UseCases.CourseCategories.Common;
using EngConnect.Application.UseCases.CourseCategories.CreateCourseCategory;
using EngConnect.Application.UseCases.CourseCategories.DeleteCourseCategory;
using EngConnect.Application.UseCases.CourseCategories.GetCourseCategoryById;
using EngConnect.Application.UseCases.CourseCategories.GetListCourseCategory;
using EngConnect.Application.UseCases.CourseCategories.UpdateCourseCategory;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lý CourseCategory
/// </summary>
[ApiController]
[Route("api/course-categories")]
public class CourseCategoryController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CourseCategoryController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách CourseCategory
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseCategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListCourseCategoryQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy thông tin CourseCategory theo ID
    /// </summary>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseCategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetCourseCategoryByIdQuery(id);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo mới CourseCategory
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCourseCategoryCommand command)
    {
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật CourseCategory
    /// </summary>
    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCourseCategoryCommand command)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa CourseCategory
    /// </summary>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var command = new DeleteCourseCategoryCommand(id);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
