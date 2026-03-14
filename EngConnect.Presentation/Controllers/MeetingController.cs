using EngConnect.Application.UseCases.Meetings.EndMeeting;
using EngConnect.Application.UseCases.Meetings.GetMeetingInfo;
using EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

[Route("api/meetings")]
[Authorize]
public class MeetingController : BaseApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public MeetingController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    /// <summary>
    /// Get meeting info for a lesson
    /// </summary>
    [HttpGet("{lessonId:guid}")]
    public async Task<IActionResult> GetMeetingInfo(Guid lessonId, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst("userId")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var query = new GetMeetingInfoQuery(lessonId, userId);
        var result = await _queryDispatcher.DispatchAsync<GetMeetingInfoResponse>(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// End a meeting (tutor only)
    /// </summary>
    [HttpPost("{lessonId:guid}/end")]
    public async Task<IActionResult> EndMeeting(Guid lessonId, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst("userId")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var command = new EndMeetingCommand(lessonId, userId);
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Upload a meeting recording chunk (chunk duration: ~30 seconds)
    /// </summary>
    [HttpPost("{lessonId:guid}/recordings/chunks")]
    public async Task<IActionResult> UploadRecordingChunk(
        Guid lessonId,
        [FromForm] IFormFile chunk,
        [FromForm] int chunkIndex,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst("userId")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var fileUpload = new FileUpload
        {
            FileName = string.IsNullOrWhiteSpace(chunk.FileName) ? $"chunk-{chunkIndex:D6}.webm" : chunk.FileName,
            ContentType = string.IsNullOrWhiteSpace(chunk.ContentType) ? "video/webm" : chunk.ContentType,
            Length = chunk.Length,
            Content = chunk.OpenReadStream()
        };

        var command = new UploadRecordingChunkCommand
        {
            LessonId = lessonId,
            UserId = userId,
            ChunkIndex = chunkIndex,
            File = fileUpload
        };

        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }
}