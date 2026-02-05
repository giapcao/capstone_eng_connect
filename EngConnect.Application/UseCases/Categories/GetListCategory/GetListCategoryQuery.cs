using EngConnect.Application.UseCases.Categories.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.Categories.GetListCategory;

public record GetListCategoryQuery : BaseQuery<PaginationResult<GetCategoryResponse>>
{
    public string? Type { get; set; }
}
