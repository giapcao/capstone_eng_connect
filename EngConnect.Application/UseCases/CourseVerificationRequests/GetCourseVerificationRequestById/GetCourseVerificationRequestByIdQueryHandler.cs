using System.Net;
using EngConnect.Application.UseCases.CourseVerificationRequests.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.GetCourseVerificationRequestById;

public class GetCourseVerificationRequestByIdQueryHandler : IQueryHandler<GetCourseVerificationRequestByIdQuery, GetCourseVerificationRequestResponse>
{
    private readonly ILogger<GetCourseVerificationRequestByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCourseVerificationRequestByIdQueryHandler(ILogger<GetCourseVerificationRequestByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetCourseVerificationRequestResponse>> HandleAsync(GetCourseVerificationRequestByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseVerificationRequestByIdQueryHandler {@Query}", query);
        try
        {
            var courseVerificationRequestRepo = _unitOfWork.GetRepository<CourseVerificationRequest, Guid>();

            var courseVerificationRequest = await courseVerificationRequestRepo.FindSingleAsync(
                x => x.Id == query.Id,
                tracking: false,
                cancellationToken: cancellationToken);

            if (courseVerificationRequest == null)
            {
                _logger.LogWarning("CourseVerificationRequest not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseVerificationRequestResponse>(HttpStatusCode.NotFound, new Error("CourseVerificationRequestNotFound", "Yêu cầu xác thực không tồn tại"));
            }
            
            //Map to response
            var result = _mapper.Map<GetCourseVerificationRequestResponse>(courseVerificationRequest);

            _logger.LogInformation("End GetCourseVerificationRequestByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseVerificationRequestByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseVerificationRequestResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
