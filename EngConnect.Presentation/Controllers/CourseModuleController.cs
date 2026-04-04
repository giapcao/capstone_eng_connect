using System.Net;
using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.Application.UseCases.CourseModules.CreateCourseModule;
using EngConnect.Application.UseCases.CourseModules.DeleteCourseModule;
using EngConnect.Application.UseCases.CourseModules.GetCourseModuleById;
using EngConnect.Application.UseCases.CourseModules.GetCourseModuleTree;
using EngConnect.Application.UseCases.CourseModules.GetListCourseModule;
using EngConnect.Application.UseCases.CourseModules.GetListCourseModuleByTutor;
using EngConnect.Application.UseCases.CourseModules.UpdateCourseModule;
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
/// Api quản lý CourseModule
/// </summary>
[ApiController]
[Route("api/course-modules")]
public class CourseModuleController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CourseModuleController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách CourseModule theo CourseId
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseModuleResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListCourseModuleQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy danh sách CourseModule của tutor hiện tại chưa thuộc course.
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpGet("by-tutor")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseModuleResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListByTutorAsync([FromQuery] GetListCourseModuleByTutorQuery query, CancellationToken cancellationToken = default)
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
    /// Lấy thông tin CourseModule theo ID
    /// </summary>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseModuleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetCourseModuleByIdQuery(id);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lay cay parent cua CourseModule theo ID (chi di nguoc len parent chain)
    /// </summary>
    [HttpGet("{id}/tree")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseModuleTreeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTreeByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetCourseModuleTreeQuery(id);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo mới CourseModule
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseModuleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCourseModuleCommand command)
    {
        var tutorId = Guid.Parse(User.GetTutorId() ?? string.Empty);
        command.TutorId = tutorId;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật CourseModule
    /// </summary>
    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseModuleListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCourseModuleCommand command)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa CourseModule
    /// </summary>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var command = new DeleteCourseModuleCommand(id);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
