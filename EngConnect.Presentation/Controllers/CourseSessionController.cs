using System.Net;
using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.Application.UseCases.CourseSessions.CreateCourseSession;
using EngConnect.Application.UseCases.CourseSessions.DeleteCourseSession;
using EngConnect.Application.UseCases.CourseSessions.GetCourseSessionById;
using EngConnect.Application.UseCases.CourseSessions.GetListCourseSession;
using EngConnect.Application.UseCases.CourseSessions.GetListCourseSessionByTutor;
using EngConnect.Application.UseCases.CourseSessions.UpdateCourseSession;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.Presentation.Controllers;
using EngConnect.BuildingBlock.Presentation.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lý CourseSession
/// </summary>
[ApiController]
[Route("api/course-sessions")]
public class CourseSessionController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CourseSessionController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách CourseSession theo CourseModuleId
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseSessionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListCourseSessionQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy danh sách CourseSession của tutor hiện tại chưa thuộc module.
    /// Migrate toàn bộ từ GetListCourseSession cũ, giữ nguyên filter, search, sort và paging.
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpGet("by-tutor")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseSessionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListByTutorAsync([FromQuery] GetListCourseSessionByTutorQuery query, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
        {
            return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                CommonErrors.ValidationFailed("Khong tim thay Id cua gia su.")));
        }

        query.TutorId = tutorId;
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy thông tin CourseSession theo ID
    /// </summary>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseSessionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetCourseSessionByIdQuery(id);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo mới CourseSession
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseSessionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCourseSessionCommand command)
    {
        var tutorId = Guid.Parse(User.GetTutorId() ?? string.Empty);
        command.TutorId = tutorId;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật CourseSession
    /// </summary>
    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseSessionListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCourseSessionCommand command)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa CourseSession
    /// </summary>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var command = new DeleteCourseSessionCommand(id);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
