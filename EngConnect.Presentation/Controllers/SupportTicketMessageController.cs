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

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<GetSupportTicketMessageResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(new GetSupportTicketMessageByIdQuery { Id = id }, cancellationToken);
        return FromResult(result);
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<PaginationResult<GetSupportTicketMessageResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] GetListSupportTicketMessageQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateSupportTicketMessageCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSupportTicketMessageCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = id;
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(new DeleteSupportTicketMessageCommand { Id = id }, cancellationToken);
        return FromResult(result);
    }
}
