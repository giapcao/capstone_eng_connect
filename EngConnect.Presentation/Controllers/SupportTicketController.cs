using EngConnect.Application.UseCases.SupportTickets.Common;
using EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket;
using EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket;
using EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets;
using EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById;
using EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket;
using EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

[ApiController]
[Route("api/supporttickets")]
public class SupportTicketController : BaseApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public SupportTicketController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy thông tin phiếu hỗ trợ theo ID.
    /// </summary>
    /// <param name="id">Mã định danh duy nhất của phiếu hỗ trợ.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chứa chi tiết phiếu hỗ trợ.</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetSupportTicketResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(new GetSupportTicketByIdQuery { Id = id }, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy danh sách phiếu hỗ trợ có phân trang.
    /// </summary>
    /// <param name="query">Các tham số truy vấn để lọc và phân trang.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chứa danh sách phiếu hỗ trợ có phân trang.</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetSupportTicketResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] GetListSupportTicketQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo phiếu hỗ trợ mới.
    /// </summary>
    /// <param name="command">Lệnh chứa chi tiết phiếu hỗ trợ.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chỉ ra kết quả của việc tạo.</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateSupportTicketCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật phiếu hỗ trợ hiện có.
    /// </summary>
    /// <param name="id">Mã định danh duy nhất của phiếu hỗ trợ cần cập nhật.</param>
    /// <param name="command">Lệnh chứa chi tiết phiếu hỗ trợ đã cập nhật.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chỉ ra kết quả của việc cập nhật.</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSupportTicketCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật trạng thái của phiếu hỗ trợ.
    /// </summary>
    /// <param name="id">Mã định danh duy nhất của phiếu hỗ trợ.</param>
    /// <param name="command">Lệnh chứa trạng thái mới.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chỉ ra kết quả của việc cập nhật trạng thái.</returns>
    [HttpPatch("{id:guid}/status")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateSupportTicketStatusCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa phiếu hỗ trợ theo ID.
    /// </summary>
    /// <param name="id">Mã định danh duy nhất của phiếu hỗ trợ cần xóa.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chỉ ra kết quả của việc xóa.</returns>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(new DeleteSupportTicketCommand { Id = id }, cancellationToken);
        return FromResult(result);
    }
}
