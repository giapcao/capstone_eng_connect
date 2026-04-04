using EngConnect.Application.UseCases.SupportTicketMessages.Common;
using EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage;
using EngConnect.Application.UseCases.SupportTicketMessages.DeleteSupportTicketMessage;
using EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages;
using EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById;
using EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

[ApiController]
[Route("api/supportticketmessages")]
public class SupportTicketMessageController : BaseApiController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public SupportTicketMessageController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Lấy thông tin tin nhắn phiếu hỗ trợ theo ID.
    /// </summary>
    /// <param name="id">Mã định danh duy nhất của tin nhắn phiếu hỗ trợ.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chứa chi tiết tin nhắn phiếu hỗ trợ.</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetSupportTicketMessageResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(new GetSupportTicketMessageByIdQuery { Id = id }, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy danh sách tin nhắn phiếu hỗ trợ có phân trang.
    /// </summary>
    /// <param name="query">Các tham số truy vấn để lọc và phân trang.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chứa danh sách tin nhắn phiếu hỗ trợ có phân trang.</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetSupportTicketMessageResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] GetListSupportTicketMessageQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo tin nhắn phiếu hỗ trợ mới.
    /// </summary>
    /// <param name="command">Lệnh chứa chi tiết tin nhắn phiếu hỗ trợ.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chỉ ra kết quả của việc tạo.</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateSupportTicketMessageCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Cập nhật tin nhắn phiếu hỗ trợ hiện có.
    /// </summary>
    /// <param name="id">Mã định danh duy nhất của tin nhắn phiếu hỗ trợ cần cập nhật.</param>
    /// <param name="command">Lệnh chứa chi tiết tin nhắn phiếu hỗ trợ đã cập nhật.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chỉ ra kết quả của việc cập nhật.</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSupportTicketMessageCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa tin nhắn phiếu hỗ trợ theo ID.
    /// </summary>
    /// <param name="id">Mã định danh duy nhất của tin nhắn phiếu hỗ trợ cần xóa.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Một IActionResult chỉ ra kết quả của việc xóa.</returns>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(new DeleteSupportTicketMessageCommand { Id = id }, cancellationToken);
        return FromResult(result);
    }
}
