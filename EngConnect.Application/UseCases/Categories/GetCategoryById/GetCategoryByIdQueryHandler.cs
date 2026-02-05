using System.Net;
using EngConnect.Application.UseCases.Categories.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Categories.GetCategoryById;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, GetCategoryResponse>
{
    private readonly ILogger<GetCategoryByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(ILogger<GetCategoryByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetCategoryResponse>> HandleAsync(GetCategoryByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCategoryByIdQueryHandler {@Query}", query);
        try
        {
            var categoryRepo = _unitOfWork.GetRepository<Category, Guid>();

            var category = await categoryRepo.FindAll(
                    x => x.Id == query.Id,
                    tracking: false)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (category == null)
            {
                _logger.LogWarning("Category not found with ID: {Id}", query.Id);
                return Result.Failure<GetCategoryResponse>(HttpStatusCode.NotFound, new Error("CategoryNotFound", "Danh mục không tồn tại"));
            }
            
            //Map to response
            var result = _mapper.Map<GetCategoryResponse>(category);

            _logger.LogInformation("End GetCategoryByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCategoryByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCategoryResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
