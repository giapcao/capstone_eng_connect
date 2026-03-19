using EngConnect.Application.UseCases.LessonScripts.Common;
using EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;
using EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript;
using EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts;
using EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById;
using EngConnect.Application.UseCases.AiSummerize.GetAiSummary;
using EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lí kịch bản bài học
/// </summary>
[ApiController]
[Route("api/lesson-scripts")]
public class LessonsScriptController : BaseApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public LessonsScriptController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }
    
    /// <summary>
    /// Lấy kịch bản bài học theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetLessonScriptResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLessonScriptById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(new GetLessonScriptByIdQuery { Id = id }, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy tóm tắt nội dung AI cho kịch bản bài học
    /// </summary>
    [HttpPost("{id:guid}/ai-summary")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<AnalysisResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAiSummaryAsync([FromRoute] Guid id, [FromBody] GetAiSummaryCommand command, CancellationToken cancellationToken = default)
    {
        command.LessonId = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Lấy danh sách kịch bản bài học (Có phân trang)
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetLessonScriptResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListLessonScriptsAsync([FromQuery] GetListLessonScriptsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Tạo kịch bản bài học
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateLessonScriptAsync([FromBody] CreateLessonScriptCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật kịch bản bài học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLessonScriptAsync([FromRoute] Guid id, [FromBody] UpdateLessonScriptCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Xóa kịch bản bài học
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteLessonScriptAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(new DeleteLessonScriptCommand { Id = id }, cancellationToken);
        return FromResult(result);
    }
}
