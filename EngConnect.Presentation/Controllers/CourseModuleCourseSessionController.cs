using EngConnect.Application.UseCases.CourseModuleCourseSessions.AddCourseSessionToCourseModule;
using EngConnect.Application.UseCases.CourseModuleCourseSessions.Common;
using EngConnect.Application.UseCases.CourseModuleCourseSessions.GetListCourseModuleCourseSession;
using EngConnect.Application.UseCases.CourseModuleCourseSessions.RemoveCourseSessionFromCourseModule;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lý liên kết giữa CourseModule và CourseSession
/// </summary>
[ApiController]
[Route("api/modules-sessions")]
public class CourseModuleCourseSessionController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CourseModuleCourseSessionController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách các buổi học trong một module
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseModuleCourseSessionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListCourseModuleCourseSessionQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Thêm buổi học vào module
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddSessionToModuleAsync([FromBody] AddCourseSessionToCourseModuleCommand command)
    {
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa buổi học khỏi module
    /// </summary>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveSessionFromModuleAsync([FromRoute] Guid id)
    {
        var command = new RemoveCourseSessionFromCourseModuleCommand(id);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
