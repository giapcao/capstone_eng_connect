using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.Application.UseCases.CourseReviews.CreateCourseReview;
using EngConnect.Application.UseCases.CourseReviews.DeleteCourseReview;
using EngConnect.Application.UseCases.CourseReviews.GetCourseReviewById;
using EngConnect.Application.UseCases.CourseReviews.GetCourseReviewsByCourseId;
using EngConnect.Application.UseCases.CourseReviews.UpdateCourseReview;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// API quản lý đánh giá khóa học
/// </summary>
[ApiController]
[Route("api/course-reviews")]
public class CourseReviewController : BaseApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public CourseReviewController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy đánh giá khóa học theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseReviewResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseReviewById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(new GetCourseReviewByIdQuery { Id = id }, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy danh sách đánh giá khóa học theo CourseId (Có phân trang)
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseReviewResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseReviewsByCourseIdAsync([FromQuery] GetCourseReviewsByCourseIdQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo đánh giá khóa học
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCourseReviewAsync([FromBody] CreateCourseReviewCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật đánh giá khóa học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCourseReviewAsync([FromRoute] Guid id, [FromBody] UpdateCourseReviewCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa đánh giá khóa học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCourseReviewAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(new DeleteCourseReviewCommand(id), cancellationToken);
        return FromResult(result);
    }
}