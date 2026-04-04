using System.Net;
using EngConnect.Application.UseCases.TutorDocuments.Common;
using EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument;
using EngConnect.Application.UseCases.TutorDocuments.GetTutorDocumentById;
using EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument;
using EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.Presentation.Controllers;
using EngConnect.BuildingBlock.Presentation.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// API quản lý tài liệu của gia sư
/// </summary>
[ApiController]
[Route("api/tutor-documents")]
public class TutorDocumentController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public TutorDocumentController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách tài liệu của gia sư
    /// </summary>
    [Authorize]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetTutorDocumentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListTutorDocumentQuery query, CancellationToken cancellationToken = default)
    {
        if (Guid.TryParse(User.GetTutorId(), out var tutorId))
        {
            query.TutorId = tutorId;
        }

        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy thông tin tài liệu theo ID
    /// </summary>
    [Authorize]
    [HttpGet("{documentId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetTutorDocumentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid documentId, CancellationToken cancellationToken = default)
    {
        var query = new GetTutorDocumentByIdQuery(documentId);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Upload tài liệu cho gia sư hiện tại
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadTutorDocumentAsync(
        [FromForm] UploadTutorDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
        {
            return FromResult(Result.Failure(
                HttpStatusCode.BadRequest,
                CommonErrors.ValidationFailed("TutorId claim is missing or invalid.")));
        }

        if (request.File is null)
        {
            return FromResult(Result.Failure(
                HttpStatusCode.BadRequest,
                CommonErrors.ValidationFailed("File")));
        }

        var command = new UploadTutorDocumentCommand
        {
            TutorId = tutorId,
            Name = request.Name,
            DocType = request.DocType,
            IssuedBy = request.IssuedBy,
            IssuedAt = request.IssuedAt,
            ExpiredAt = request.ExpiredAt,
            Status = request.Status,
            File = new FileUpload
            {
                FileName = request.FileName ?? request.File.FileName,
                ContentType = string.IsNullOrWhiteSpace(request.File.ContentType)
                    ? "application/octet-stream"
                    : request.File.ContentType,
                Length = request.File.Length,
                Content = request.File.OpenReadStream()
            }
        };

        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        if (!result.IsSuccess)
        {
            return FromResult(result);
        }

        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Xóa tài liệu của gia sư hiện tại
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpDelete("{documentId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveTutorDocumentAsync(
        [FromRoute] Guid documentId,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
        {
            return FromResult(Result.Failure(
                HttpStatusCode.BadRequest,
                CommonErrors.ValidationFailed("TutorId claim is missing or invalid.")));
        }

        var command = new RemoveTutorDocumentCommand
        {
            DocumentId = documentId,
            TutorId = tutorId
        };

        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }
}
