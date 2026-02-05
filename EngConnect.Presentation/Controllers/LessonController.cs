using EngConnect.Application.UseCases.Lessons.Common;
using EngConnect.Application.UseCases.Lessons.CreateLesson;
using EngConnect.Application.UseCases.Lessons.DeleteLesson;
using EngConnect.Application.UseCases.Lessons.GetListLessons;
using EngConnect.Application.UseCases.Lessons.GetLessonById;
using EngConnect.Application.UseCases.Lessons.UpdateLesson;
using EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// API quản lý bài học
/// </summary>
[ApiController]
[Route("api/lessons")]
public class LessonController : BaseApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public LessonController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }
    
    /// <summary>
    /// Lấy bài học theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetLessonResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLessonById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(new GetLessonByIdQuery { Id = id }, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Lấy danh sách bài học (Có phân trang)
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetLessonResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListLessonsAsync([FromQuery] GetListLessonQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Tạo bài học
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateLessonAsync([FromBody] CreateLessonCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật bài học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLessonAsync([FromRoute] Guid id, [FromBody] UpdateLessonCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Cập nhật trạng thái bài học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}/status")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLessonStatusAsync([FromRoute] Guid id, [FromBody] UpdateLessonStatusCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Xóa bài học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteLessonAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(new DeleteLessonCommand { Id = id }, cancellationToken);
        return FromResult(result);
    }
}
