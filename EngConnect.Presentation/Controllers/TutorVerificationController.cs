using EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest;
using EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers
{
    /// <summary>
    /// API quản lý yêu cầu xác minh gia sư
    /// </summary>
    [ApiController]
    [Route("api/tutors")]
    public class TutorVerificationController : BaseApiController
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public TutorVerificationController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// Gia sư gửi yêu cầu xác minh hồ sơ.
        /// </summary>
        [HttpPost("{tutorId:guid}/verification-requests")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTutorVerificationRequestAsync(
            Guid tutorId,
            CancellationToken cancellationToken = default)
        {

            var request = new CreateTutorVerificationRequest
            {
                TutorId = tutorId
            };

            var command = new CreateTutorVerificationRequestCommand(request);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return FromResult(result);
            }

            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Quản trị viên duyệt / từ chối yêu cầu xác minh gia sư.
        /// </summary>
        [HttpPost("verification-requests/{requestId:guid}/review")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReviewTutorVerificationRequestAsync(
            Guid requestId,
            [FromBody] ReviewTutorVerificationRequest body,
            CancellationToken cancellationToken = default)
        {
            if (body is null)
            {
                return BadRequest(CommonErrors.ValidationFailed("Dữ liệu không thể null."));
            }

            var request = body with { RequestId = requestId };

            var command = new ReviewTutorVerificationRequestCommand(request);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return FromResult(result);
        }
    }
}