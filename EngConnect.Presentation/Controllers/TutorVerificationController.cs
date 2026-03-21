using EngConnect.Application.UseCases.TutorVerification.Common;
using EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest;
using EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest;
using EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest;
using EngConnect.Application.UseCases.TutorVerification.GetTutorVerificationRequestById;
using EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.Presentation.Controllers;
using EngConnect.BuildingBlock.Presentation.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers
{
    /// <summary>
    /// API quản lý yêu cầu xác minh gia sư
    /// </summary>
    [ApiController]
    [Route("api/tutor-verification-requests")]
    public class TutorVerificationController : BaseApiController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public TutorVerificationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Gia sư gửi yêu cầu xác minh hồ sơ.
        /// </summary>
        [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTutorVerificationRequestAsync(
            CancellationToken cancellationToken = default)
        {
            var tutorId = Guid.Parse(User.GetTutorId() ?? string.Empty);

            var request = new CreateTutorVerificationRequest
            {
                TutorId = tutorId
            };

            var command = new CreateTutorVerificationRequestCommand(request);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return FromResult(result);
        }

        /// <summary>
        /// Quản trị viên duyệt / từ chối yêu cầu xác minh gia sư.
        /// </summary>
        [Authorize(Roles = $"{nameof(UserRoleEnum.Admin)},{nameof(UserRoleEnum.Staff)}")]        
        [HttpPost("review/{requestId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReviewTutorVerificationRequestAsync(
            Guid requestId,
            [FromBody] ReviewTutorVerificationRequest body,
            CancellationToken cancellationToken = default)
        {
            if (ValidationUtil.IsNullOrEmpty(body))
            {
                return BadRequest(CommonErrors.ValidationFailed("Dữ liệu không thể null."));
            }
            
            // Take admin/staff id
            var reviewUserId = User.GetUserId();

            if (ValidationUtil.IsNullOrEmpty(reviewUserId))
            {
                return BadRequest(CommonErrors.ValidationFailed("Không tìm thấy thông tin admin/staff."));
            }
            var request = body with
            {
                RequestId = requestId,
                AdminUserId = reviewUserId,
            };

            var command = new ReviewTutorVerificationRequestCommand(request);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return FromResult(result);
        }

        /// <summary>
        /// Lấy danh sách yêu cầu xác minh gia sư (có filter/search/sort/pagination).
        /// </summary>
        [Authorize(Roles = $"{nameof(UserRoleEnum.Admin)},{nameof(UserRoleEnum.Staff)}, {nameof(UserRoleEnum.Tutor)}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<PaginationResult<GetTutorVerificationRequestResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListTutorVerificationRequestsAsync(
            [FromQuery] GetListTutorVerificationRequestQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
            return FromResult(result);
        }

        /// <summary>
        /// Lấy chi tiết 1 yêu cầu xác minh gia sư theo RequestId.
        /// </summary>
        [Authorize(Roles = $"{nameof(UserRoleEnum.Admin)},{nameof(UserRoleEnum.Staff)}, {nameof(UserRoleEnum.Tutor)}")]
        [HttpGet("{requestId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<GetTutorVerificationRequestResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTutorVerificationRequestByIdAsync(
            Guid requestId,
            CancellationToken cancellationToken = default)
        {
            var query = new GetTutorVerificationRequestByIdQuery(requestId);
            var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            return FromResult(result);
        }

        /// <summary>
        /// Xóa 1 yêu cầu xác minh gia sư theo RequestId.
        /// </summary>
        [HttpDelete("{requestId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTutorVerificationRequestAsync(
            Guid requestId,
            CancellationToken cancellationToken = default)
        {
            var command = new DeleteTutorVerificationRequestCommand(requestId);
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return FromResult(result);
        }
    }
}