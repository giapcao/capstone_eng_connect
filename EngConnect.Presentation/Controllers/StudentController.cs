using EngConnect.Application.UseCases.Students.Common;
using EngConnect.Application.UseCases.Students.CreateStudent;
using EngConnect.Application.UseCases.Students.DeleteStudent;
using EngConnect.Application.UseCases.Students.GetAvatarStudent;
using EngConnect.Application.UseCases.Students.GetListStudents;
using EngConnect.Application.UseCases.Students.GetStudentById;
using EngConnect.Application.UseCases.Students.UpdateAvatarStudent;
using EngConnect.Application.UseCases.Students.UpdateStatusStudent;
using EngConnect.Application.UseCases.Students.UpdateStudent;
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
using EngConnect.Application.Common;

namespace EngConnect.Presentation.Controllers;

/// <summary>
/// Api quản lí học sinh
/// </summary>
[ApiController]
[Route("api/students")]
public class StudentController : BaseApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public StudentController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }
    
    /// <summary>
    /// Lấy học sinh theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetStudentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync( new GetStudentByIdQuery{Id = id},cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Lấy danh sách học sinh (Có phân trang)
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetStudentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListStudentsAsync([FromQuery] GetListStudentQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Lấy thông tin profile của học sinh hiện tại (từ claims)
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = nameof(UserRoleEnum.Student))]
    [HttpGet("profile")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetStudentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.GetStudentId(), out var studentId))
        {
            return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                CommonErrors.ValidationFailed("Không tìm thấy Id của học sinh.")));
        }

        var result = await _queryDispatcher.DispatchAsync(new GetStudentByIdQuery { Id = studentId }, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Lấy ảnh avatar
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = nameof(UserRoleEnum.Student))]
    [HttpGet("avatar")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetAvatarResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvatarStudentAsync(CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.GetStudentId(), out var studentId))
        {
            return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                CommonErrors.ValidationFailed("StudentId claim is missing or invalid.")));
        }

        var result = await _queryDispatcher.DispatchAsync(new GetAvatarQuery { Id = studentId }, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Tạo học sinh 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateStudentAsync([FromBody] CreateStudentCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật học sinh 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid id,[FromBody] UpdateStudentCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id; 
        var result = await _commandDispatcher.DispatchAsync( command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// cập nhật avatar
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize(Roles = nameof(UserRoleEnum.Student))]
    [HttpPut("avatar")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAvatarStudentAsync([FromForm]FileFormatRequest file, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.GetStudentId(), out var studentId))
        {
            return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                CommonErrors.ValidationFailed("Không tìm thấy Id của học sinh.")));
        }

        var fileUpload = new FileUpload
        {
            FileName = file.FileName,
            ContentType = string.IsNullOrWhiteSpace(file.File.ContentType) ? "application/octet-stream" : file.File.ContentType,
            Length = file.File.Length,
            Content = file.File.OpenReadStream()
        };
        
        var result = await _commandDispatcher.DispatchAsync(new UpdateAvatarStudentCommand { File = fileUpload, Id = studentId }, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Cập nhật trạng thái của học sinh 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}/status")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStatusStudentAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync( new UpdateStatusStudentCommand{Id = id},cancellationToken);
        return FromResult(result);
    }
    
    
    /// <summary>
    /// Xóa học sinh 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteStudentAsync([FromQuery] Guid userId, [FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(new DeleteStudentCommand{Id = id, UserId = userId},cancellationToken);
        return FromResult(result);
    }
}
