using EngConnect.Application.UseCases.CourseVerificationRequests.Common;
using EngConnect.Application.UseCases.CourseVerificationRequests.CreateCourseVerificationRequest;
using EngConnect.Application.UseCases.CourseVerificationRequests.DeleteCourseVerificationRequest;
using EngConnect.Application.UseCases.CourseVerificationRequests.GetCourseVerificationRequestById;
using EngConnect.Application.UseCases.CourseVerificationRequests.GetListCourseVerificationRequest;
using EngConnect.Application.UseCases.CourseVerificationRequests.UpdateCourseVerificationRequest;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Presentation.Controllers;
using EngConnect.BuildingBlock.Presentation.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lý CourseVerificationRequest
/// </summary>
[ApiController]
[Route("api/course-verification-requests")]
public class CourseVerificationRequestController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CourseVerificationRequestController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách CourseVerificationRequest
    /// </summary>
    [Authorize(Roles = $"{nameof(UserRoleEnum.Tutor)},{nameof(UserRoleEnum.Admin)}")]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseVerificationRequestResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListCourseVerificationRequestQuery query, CancellationToken cancellationToken = default)
    {
        var roles = User.GetRoles();
        if (roles.Contains(nameof(UserRoleEnum.Tutor)))
        {
            var tutorId = Guid.Parse(User.GetTutorId() ?? string.Empty);
            if (ValidationUtil.IsNullOrEmpty(tutorId))
            {
                return Unauthorized();
            }
            query.TutorId = tutorId;
        }
        
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy thông tin CourseVerificationRequest theo ID
    /// </summary>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseVerificationRequestResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetCourseVerificationRequestByIdQuery(id);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Gia sư tạo đơn xác thực khóa học
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCourseVerificationRequestCommand command)
    {
        // Add tutor id from token
        var tutorId = User.GetTutorId();
        if (ValidationUtil.IsNullOrEmpty(tutorId))
        {
            return Unauthorized();
        }
        command.TutorId = Guid.Parse(tutorId);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật đơn xác thực khóa học
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Admin))]
    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCourseVerificationRequestCommand command)
    {
        var userId = User.GetUserId();
        if (ValidationUtil.IsNullOrEmpty(userId))        {
            return Unauthorized();
        }
        command.UserId = userId.Value;
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa CourseVerificationRequest
    /// </summary>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var command = new DeleteCourseVerificationRequestCommand(id);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
