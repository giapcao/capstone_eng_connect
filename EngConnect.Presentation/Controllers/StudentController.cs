using EngConnect.Application.UseCases.Students.Common;
using EngConnect.Application.UseCases.Students.CreateStudent;
using EngConnect.Application.UseCases.Students.DeleteStudent;
using EngConnect.Application.UseCases.Students.GetListStudents;
using EngConnect.Application.UseCases.Students.GetStudentById;
using EngConnect.Application.UseCases.Students.UpdateStatusStudent;
using EngConnect.Application.UseCases.Students.UpdateStudent;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

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
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStudentAsync([FromQuery] Guid userId, [FromRoute] Guid id,[FromBody] UpdateStudentCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id; command.UserId = userId;
        var result = await _commandDispatcher.DispatchAsync( command, cancellationToken);
        return FromResult(result);
    }
    
    /// <summary>
    /// Cập nhật trạng thái của học sinh 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("/updateStatus/{id:guid}")]
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