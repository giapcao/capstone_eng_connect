using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.CourseVerification.CreateCourseVerificationRequest
{
    public class CreateCourseVerificationRequestCommandHandler
        : ICommandHandler<CreateCourseVerificationRequestCommand>
    {
        private readonly ILogger<CreateCourseVerificationRequestCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCourseVerificationRequestCommandHandler(
            ILogger<CreateCourseVerificationRequestCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            CreateCourseVerificationRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Start CreateCourseVerificationRequestCommandHandler: {@Command}",
                command);

            try
            {
                var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
                var requestRepo = _unitOfWork.GetRepository<CourseVerificationRequest, Guid>();

                var course = await courseRepo.FindFirstAsync(c => c.Id == command.Request.CourseId);

                if (course is null)
                {
                    return Result.Failure(HttpStatusCode.NotFound,
                        CourseErrors.NotFound(command.Request.CourseId));
                }

                var hasPending = await requestRepo.AnyAsync(r =>
                        r.CourseId == command.Request.CourseId &&
                        r.Status == "pending",
                        cancellationToken);

                if (hasPending)
                {
                    return Result.Failure(HttpStatusCode.BadRequest,
                        CourseErrors.VerificationRequestAlreadyPending(command.Request.CourseId));
                }

                var entity = new CourseVerificationRequest
                {
                    CourseId = command.Request.CourseId,
                    Status = "pending",
                    SubmittedAt = DateTime.UtcNow
                };

                requestRepo.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation(
                    "End CreateCourseVerificationRequestCommandHandler: {RequestId}",
                    entity.Id);

                return Result.Success(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred in CreateCourseVerificationRequestCommandHandler {@Message}",
                    ex.Message);

                return Result.Failure<Guid>(
                    HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}