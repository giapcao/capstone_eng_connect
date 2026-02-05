using EngConnect.Application.UseCases.Categories.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Categories.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IQuery<GetCategoryResponse>;
