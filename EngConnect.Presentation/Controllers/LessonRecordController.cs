using EngConnect.Application.UseCases.LessonRecords.Common;
using EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;
using EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord;
using EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById;
using EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords;
using EngConnect.Application.UseCases.LessonRecords.UpdateLessonRecord;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

[ApiController]
[Route("api/lesson-records")]
public class LessonRecordController : BaseApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public LessonRecordController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetLessonRecordResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLessonRecordById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(new GetLessonRecordByIdQuery { Id = id }, cancellationToken);
        return FromResult(result);
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetLessonRecordResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListLessonRecordsAsync([FromQuery] GetListLessonRecordQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateLessonRecordAsync([FromBody] CreateLessonRecordCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLessonRecordAsync([FromRoute] Guid id, [FromBody] UpdateLessonRecordCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteLessonRecordAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(new DeleteLessonRecordCommand { Id = id }, cancellationToken);
        return FromResult(result);
    }
}
