using EngConnect.Application.UseCases.CourseEnrollments.Common;
using EngConnect.Application.UseCases.CourseEnrollments.CreateCourseEnrollment;
using EngConnect.Application.UseCases.CourseEnrollments.DeleteCourseEnrollment;
using EngConnect.Application.UseCases.CourseEnrollments.GetCourseEnrollmentById;
using EngConnect.Application.UseCases.CourseEnrollments.GetListCourseEnrollments;
using EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollment;
using EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollmentStatus;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lí ghi danh khóa học
/// </summary>
[ApiController]
[Route("api/course-enrollments")]
public class CourseEnrollmentController : BaseApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public CourseEnrollmentController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy thông tin ghi danh theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseEnrollmentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseEnrollmentById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(new GetCourseEnrollmentByIdQuery { Id = id }, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy danh sách ghi danh khóa học (Có phân trang)
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseEnrollmentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListCourseEnrollmentsAsync([FromQuery] GetListCourseEnrollmentQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo ghi danh khóa học
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCourseEnrollmentAsync([FromBody] CreateCourseEnrollmentCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật thông tin ghi danh khóa học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCourseEnrollmentAsync([FromRoute] Guid id, [FromBody] UpdateCourseEnrollmentCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật trạng thái ghi danh khóa học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}/status")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCourseEnrollmentStatusAsync([FromRoute] Guid id, [FromBody] UpdateCourseEnrollmentStatusCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa ghi danh khóa học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCourseEnrollmentAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(new DeleteCourseEnrollmentCommand { Id = id }, cancellationToken);
        return FromResult(result);
    }
}
