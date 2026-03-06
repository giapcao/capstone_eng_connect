using System.Net;
using EngConnect.Application.UseCases.CourseCategories.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseCategories.GetCourseCategoryById;

public class GetCourseCategoryByIdQueryHandler : IQueryHandler<GetCourseCategoryByIdQuery, GetCourseCategoryResponse>
{
    private readonly ILogger<GetCourseCategoryByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCourseCategoryByIdQueryHandler(ILogger<GetCourseCategoryByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetCourseCategoryResponse>> HandleAsync(GetCourseCategoryByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseCategoryByIdQueryHandler {@Query}", query);
        try
        {
            var courseCategoryRepo = _unitOfWork.GetRepository<CourseCategory, Guid>();

            var courseCategory = await courseCategoryRepo.FindAll(
                    x => x.Id == query.Id,
                    tracking: false)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (courseCategory == null)
            {
                _logger.LogWarning("CourseCategory not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseCategoryResponse>(HttpStatusCode.NotFound, new Error("CourseCategoryNotFound", "CourseCategory không tồn tại"));
            }
            
            var result = _mapper.Map<GetCourseCategoryResponse>(courseCategory);

            _logger.LogInformation("End GetCourseCategoryByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseCategoryByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseCategoryResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
