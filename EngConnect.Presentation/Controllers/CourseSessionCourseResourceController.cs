using EngConnect.Application.UseCases.CourseSessionCourseResources.AddCourseResourceToCourseSession;
using EngConnect.Application.UseCases.CourseSessionCourseResources.Common;
using EngConnect.Application.UseCases.CourseSessionCourseResources.GetListCourseSessionCourseResource;
using EngConnect.Application.UseCases.CourseSessionCourseResources.RemoveCourseResourceFromCourseSession;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lý liên kết giữa CourseSession và CourseResource
/// </summary>
[ApiController]
[Route("api/sessions-resources")]
public class CourseSessionCourseResourceController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CourseSessionCourseResourceController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách các tài nguyên trong một buổi học
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseSessionCourseResourceResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListCourseSessionCourseResourceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Thêm tài nguyên vào buổi học
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddResourceToSessionAsync([FromBody] AddCourseResourceToCourseSessionCommand command)
    {
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa tài nguyên khỏi buổi học
    /// </summary>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveResourceFromSessionAsync([FromRoute] Guid id)
    {
        var command = new RemoveCourseResourceFromCourseSessionCommand(id);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
