using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.Application.UseCases.Tutors.CreateTutor;
using EngConnect.Application.UseCases.Tutors.DeleteTutor;
using EngConnect.Application.UseCases.Tutors.GetAvatarTutor;
using EngConnect.Application.UseCases.Tutors.GetCvUrlTutor;
using EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor;
using EngConnect.Application.UseCases.Tutors.GetListTutor;
using EngConnect.Application.UseCases.Tutors.GetTutorById;
using EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor;
using EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor;
using EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor;
using EngConnect.Application.UseCases.Tutors.UpdateTutor;
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
        /// Lấy thông tin profile của gia sư hiện tại (từ claims)
        /// </summary>
        [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
        [HttpGet("profile")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<GetTutorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfileAsync(CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
            {
                return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("Không tìm thấy Id của gia sư.")));
            }

            var query = new GetTutorByIdQuery(tutorId);
            var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
            return FromResult(result);
        }

        /// <summary>
        /// Lấy ảnh avatar gia sư
        /// </summary>
        [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
        [HttpGet("avatar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<GetAvatarTutorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvatarTutorAsync(CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
            {
                return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("Không tìm thấy Id của gia sư.")));
            }

            var result = await _queryDispatcher.DispatchAsync(new GetAvatarTutorQuery { Id = tutorId }, cancellationToken);
            return FromResult(result);
        }

        /// <summary>
        /// Lấy CV của gia sư
        /// </summary>
        [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
        [HttpGet("cv")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<GetCvUrlTutorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCvUrlTutorAsync(CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
            {
                return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("Không tìm thấy Id của gia sư.")));
            }

            var result = await _queryDispatcher.DispatchAsync(new GetCvUrlTutorQuery { Id = tutorId }, cancellationToken);
            return FromResult(result);
        }

        /// <summary>
        /// Lấy video giới thiệu của gia sư
        /// </summary>
        [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
        [HttpGet("intro-video")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<GetIntroVideoUrlTutorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIntroVideoUrlTutorAsync(CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
            {
                return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("Không tìm thấy Id của gia sư.")));
            }

            var result = await _queryDispatcher.DispatchAsync(new GetIntroVideoUrlTutorQuery { Id = tutorId }, cancellationToken);
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

            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Cập nhật avatar gia sư
        /// </summary>
        [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
        [HttpPut("avatar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAvatarTutorAsync([FromForm]FileFormatRequest file, CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
            {
                return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("TutorId claim is missing or invalid.")));
            }

            var fileUpload = new FileUpload
            {
                FileName = file.FileName,
                ContentType = string.IsNullOrWhiteSpace(file.File.ContentType) ? "application/octet-stream" : file.File.ContentType,
                Length = file.File.Length,
                Content = file.File.OpenReadStream()
            };

            var result = await _commandDispatcher.DispatchAsync(
                new UpdateAvatarTutorCommand { File = fileUpload, Id = tutorId }, cancellationToken);
            return FromResult(result);
        }

        /// <summary>
        /// Cập nhật CV gia sư
        /// </summary>
        [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
        [HttpPut("cv")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCvUrlTutorAsync([FromForm]FileFormatRequest file, CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
            {
                return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("TutorId claim is missing or invalid.")));
            }
            
            var fileUpload = new FileUpload
            {
                FileName = file.FileName,
                ContentType = string.IsNullOrWhiteSpace(file.File.ContentType) ? "application/pdf" : file.File.ContentType,
                Length = file.File.Length,
                Content = file.File.OpenReadStream()
            };

            var result = await _commandDispatcher.DispatchAsync(
                new UpdateCvUrlTutorCommand { File = fileUpload, Id = tutorId }, cancellationToken);
            return FromResult(result);
        }

        /// <summary>
        /// Cập nhật video giới thiệu gia sư
        /// </summary>
        [Authorize(Roles = nameof(UserRoleEnum.Tutor))]
        [HttpPut("intro-video")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateIntroVideoUrlTutorAsync([FromForm]FileFormatRequest file, CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(User.GetTutorId(), out var tutorId))
            {
                return FromResult(Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("TutorId claim is missing or invalid.")));
            }

            var fileUpload = new FileUpload
            {
                FileName = file.FileName,
                ContentType = string.IsNullOrWhiteSpace(file.File.ContentType) ? "video/mp4" : file.File.ContentType,
                Length = file.File.Length,
                Content = file.File.OpenReadStream()
            };

            var result = await _commandDispatcher.DispatchAsync(
                new UpdateIntroVideoUrlTutorCommand { File = fileUpload, Id = tutorId }, cancellationToken);
            return FromResult(result);
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
