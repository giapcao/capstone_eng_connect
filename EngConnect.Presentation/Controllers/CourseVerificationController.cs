using EngConnect.Application.UseCases.CourseVerification.CreateCourseVerificationRequest;
using EngConnect.Application.UseCases.CourseVerification.ReviewCourseVerificationRequest;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers
{
    /// <summary>
    /// API quản lý yêu cầu xác minh khóa học
    /// </summary>
    [ApiController]
    [Route("api/courses")]
    public class CourseVerificationController : BaseApiController
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public CourseVerificationController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// Gia sư gửi yêu cầu xác minh khóa học.
        /// </summary>
        [HttpPost("{courseId:guid}/verification-requests")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCourseVerificationRequestAsync(
            Guid courseId,
            CancellationToken cancellationToken = default)
        {
            var request = new CreateCourseVerificationRequest
            {
                CourseId = courseId
            };

            var command = new CreateCourseVerificationRequestCommand(request);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return FromResult(result);
            }

            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Quản trị viên duyệt / từ chối yêu cầu xác minh khóa học.
        /// </summary>
        [HttpPost("verification-requests/{requestId:guid}/review")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReviewCourseVerificationRequestAsync(
            Guid requestId,
            [FromBody] ReviewCourseVerificationRequest body,
            CancellationToken cancellationToken = default)
        {
            if (body is null)
            {
                return BadRequest(CommonErrors.ValidationFailed("Dữ liệu không thể null."));
            }

            var request = body with { RequestId = requestId };

            var command = new ReviewCourseVerificationRequestCommand(request);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return FromResult(result);
        }
    }
}