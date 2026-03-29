using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.Application.UseCases.CourseResources.CreateCourseResource;
using EngConnect.Application.UseCases.CourseResources.DeleteCourseResource;
using EngConnect.Application.UseCases.CourseResources.GetCourseResourceById;
using EngConnect.Application.UseCases.CourseResources.GetListCourseResource;
using EngConnect.Application.UseCases.CourseResources.UpdateCourseResource;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.Presentation.Controllers;
using EngConnect.BuildingBlock.Presentation.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lý CourseResource
/// </summary>
[ApiController]
[Route("api/course-resources")]
public class CourseResourceController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CourseResourceController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy danh sách CourseResource
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseResourceResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListCourseResourceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy thông tin CourseResource theo ID
    /// </summary>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseResourceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetCourseResourceByIdQuery(id);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo mới CourseResource
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseResourceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromForm] CreateCourseResourceRequest request)
    {
        var tutorId = Guid.Parse(User.GetTutorId() ?? string.Empty);
        var command = new CreateCourseResourceCommand
        {
            TutorId = tutorId,
            CourseSessionId = request.CourseSessionId,
            Title = request.Title,
            ResourceType = request.ResourceType,
            ResourceFile = new FileUpload
            {
                FileName =  request.ResourceFileName ?? request.ResourceFile.FileName,
                ContentType = request.ResourceFile.ContentType ?? "application/octet-stream",
                Content = request.ResourceFile.OpenReadStream(),
                Length =  request.ResourceFile.Length
            }
        };
        
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật CourseResource
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseResourceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCourseResourceCommand command)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa CourseResource
    /// </summary>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var command = new DeleteCourseResourceCommand(id);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
