using EngConnect.Application.UseCases.Tutor.Common;
using EngConnect.Application.UseCases.Tutor.CreateTutor;
using EngConnect.Application.UseCases.Tutor.DeleteTutor;
using EngConnect.Application.UseCases.Tutor.GetListTutor;
using EngConnect.Application.UseCases.Tutor.GetTutorById;
using EngConnect.Application.UseCases.Tutor.UpdateTutor;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers
{
    /// <summary>
    /// Api quản lí gia sư
    /// </summary>
    [ApiController]
    [Route("api/tutors")]
    public class TutorController : BaseApiController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public TutorController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Lấy danh sách gia sư (Có phân trang)
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<PaginationResult<GetTutorResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListTutorAsync(
            [FromQuery] GetListTutorQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
            return FromResult(result);
        }

        /// <summary>
        /// Lấy thông tin chi tiết gia sư
        /// </summary>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<GetTutorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTutorByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = new GetTutorByIdQuery(id);
            var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
            return FromResult(result);
        }

        /// <summary>
        /// Tạo mới gia sư
        /// </summary>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTutorAsync(
            [FromBody] CreateTutorRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateTutorCommand(request);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return FromResult(result);
            }

            return Created();
        }

        /// <summary>
        /// Cập nhật gia sư
        /// </summary>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTutorAsync(
            Guid id,
            [FromBody] UpdateTutorRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateTutorCommand(id, request);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
            return FromResult(result);
        }

        /// <summary>
        /// Xóa gia sư
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTutorAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteTutorCommand(id);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
            return FromResult(result);
        }
    }
}