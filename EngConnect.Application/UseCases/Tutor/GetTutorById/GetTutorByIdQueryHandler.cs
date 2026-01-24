using EngConnect.Application.UseCases.Tutor.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.GetTutorById
{
    public class GetTutorByIdQueryHandler : IQueryHandler<GetTutorByIdQuery, GetTutorResponse>
    {
        private readonly ILogger<GetTutorByIdQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetTutorByIdQueryHandler(ILogger<GetTutorByIdQueryHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetTutorResponse>> HandleAsync(
            GetTutorByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start GetTutorByIdQueryHandler: {@Query}", query);

            try
            {
                var repo = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>();

                var tutor = await repo
                    .FindAll()
                    .Where(x => x.Id == query.Id)
                    .Select(x => new GetTutorResponse
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        Headline = x.Headline,
                        Bio = x.Bio,
                        IntroVideoUrl = x.IntroVideoUrl,
                        YearsExperience = x.YearsExperience,
                        CvUrl = x.CvUrl,
                        Tags = x.Tags,
                        SlotsCount = x.SlotsCount,
                        Status = x.Status,
                        VerifiedStatus = x.VerifiedStatus,
                        RatingAverage = x.RatingAverage,
                        RatingCount = x.RatingCount,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        IsDeleted = x.IsDeleted,
                        DeletedAt = x.DeletedAt
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (tutor is null)
                {
                    return Result.Failure<GetTutorResponse>(HttpStatusCode.NotFound, CommonErrors.NotFound<Domain.Persistence.Models.Tutor>("Tutor"));
                }

                _logger.LogInformation("End GetTutorByIdQueryHandler");
                return Result.Success(tutor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetTutorByIdQueryHandler {@Message}", ex.Message);
                return Result.Failure<GetTutorResponse>(HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}