using EngConnect.Application.Common;
using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.Application.UseCases.Courses.CreateCourse;
using EngConnect.Application.UseCases.Courses.DeleteCourse;
using EngConnect.Application.UseCases.Courses.GetCourseById;
using EngConnect.Application.UseCases.Courses.GetListCourse;
using EngConnect.Application.UseCases.Courses.InactiveCourse;
using EngConnect.Application.UseCases.Courses.UpdateCourse;
using EngConnect.Application.UseCases.Courses.UpdateDemoVideoCourse;
using EngConnect.Application.UseCases.Courses.UpdateThumbnailCourse;
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
using System.Net;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quan ly Course
/// </summary>
[ApiController]
[Route("api/courses")]
public class CourseController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CourseController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lay danh sach Course
    /// </summary>
    [Authorize]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync([FromQuery] GetListCourseQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lay danh sach Course cua nguoi dung hien tai
    /// </summary>
    [Authorize(Roles = $"{nameof(UserRoleEnum.Student)},{nameof(UserRoleEnum.Tutor)}")]
    [HttpGet("my-course")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetCourseResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyCoursesAsync([FromQuery] GetListCourseQuery query, CancellationToken cancellationToken = default)
    {
        if (User.IsInRole(nameof(UserRoleEnum.Tutor)))
        {
            if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
            {
                return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("Khong tim thay Id cua gia su.")));
            }

            query.TutorId = tutorId;
        }
        else if (User.IsInRole(nameof(UserRoleEnum.Student)))
        {
            if (!Guid.TryParse(User.GetStudentId(), out var studentId))
            {
                return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("Khong tim thay Id cua hoc sinh.")));
            }

            query.StudentId = studentId;
        }

        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lay thong tin Course theo ID
    /// </summary>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetCourseResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetCourseByIdQuery(id);
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tao moi Course
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync(
        [FromForm] CreateCourseRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
        {
            return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                CommonErrors.ValidationFailed("Khong tim thay Id cua gia su.")));
        }

        var command = new CreateCourseCommand
        {
            TutorId = tutorId,
            ParentCourseId = request.ParentCourseId,
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            FullDescription = request.FullDescription,
            Outcomes = request.Outcomes,
            Level = request.Level,
            EstimatedTimeLesson = request.EstimatedTimeLesson,
            Price = request.Price,
            Currency = request.Currency,
            NumsSessionInWeek = request.NumsSessionInWeek,
            IsCertificate = request.IsCertificate,
            CategoryIds = request.CategoryIds,
            ThumbnailFile = request.ThumbnailFile != null
                ? new FileUpload
                {
                    FileName = request.ThumbnailFileName ?? request.ThumbnailFile.FileName,
                    ContentType = request.ThumbnailFile.ContentType ?? "application/octet-stream",
                    Length = request.ThumbnailFile.Length,
                    Content = request.ThumbnailFile.OpenReadStream()
                }
                : null,
            DemoVideoFile = request.DemoVideoFile != null
                ? new FileUpload
                {
                    FileName = request.DemoVideoFileName ?? request.DemoVideoFile.FileName,
                    ContentType = request.DemoVideoFile.ContentType ?? "video/mp4",
                    Length = request.DemoVideoFile.Length,
                    Content = request.DemoVideoFile.OpenReadStream()
                }
                : null
        };

        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cap nhat thumbnail khoa hoc
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPut("{id}/thumbnail")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateThumbnailAsync([FromRoute] Guid id, [FromForm] FileFormatRequest file, CancellationToken cancellationToken = default)
    {
        var fileUpload = new FileUpload
        {
            FileName = file.FileName,
            ContentType = string.IsNullOrWhiteSpace(file.File.ContentType) ? "application/octet-stream" : file.File.ContentType,
            Length = file.File.Length,
            Content = file.File.OpenReadStream()
        };

        var result = await _commandDispatcher.DispatchAsync(
            new UpdateThumbnailCourseCommand { File = fileUpload, Id = id }, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cap nhat demo video khoa hoc
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPut("{id}/demo-video")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateDemoVideoAsync([FromRoute] Guid id, [FromForm] FileFormatRequest file, CancellationToken cancellationToken = default)
    {
        var fileUpload = new FileUpload
        {
            FileName = file.FileName,
            ContentType = string.IsNullOrWhiteSpace(file.File.ContentType) ? "video/mp4" : file.File.ContentType,
            Length = file.File.Length,
            Content = file.File.OpenReadStream()
        };

        var result = await _commandDispatcher.DispatchAsync(
            new UpdateDemoVideoCourseCommand { File = fileUpload, Id = id }, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cap nhat Course
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCourseCommand command)
    {
        var tutorId = Guid.Parse(User.GetTutorId() ?? string.Empty);
        command.Id = id;
        command.TutorId = tutorId;
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Xoa Course
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var tutorId = Guid.Parse(User.GetTutorId() ?? string.Empty);
        var command = new DeleteCourseCommand(id, tutorId);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }

    /// <summary>
    /// Vo hieu hoa Course
    /// </summary>
    [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
    [HttpPatch("{id}/inactive")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> InactiveCourseAsync([FromRoute] Guid id)
    {
        var tutorId = Guid.Parse(User.GetTutorId() ?? string.Empty);
        var command = new InactiveCourseCommand(id, tutorId);
        var result = await _commandDispatcher.DispatchAsync(command);
        return FromResult(result);
    }
}
