using EngConnect.BuildingBlock.Application.Params;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.BuildingBlock.Application.Base;

public abstract record BaseQuery<TResult> : IQuery<TResult>
{
    private readonly SearchParams _searchParams = new();
    private readonly PaginationParams _paginationParams = new();
    private readonly SortParams _sortParams = [];

    [FromQuery(Name = "page")]
    public int PageNumber
    {
        get => _paginationParams.PageNumber;
        set => _paginationParams.PageNumber = value;
    }

    [FromQuery(Name = "page-size")]
    public int PageSize
    {
        get => _paginationParams.PageSize;
        set => _paginationParams.PageSize = value;
    }

    [FromQuery(Name = "search-term")]
    public string SearchTerm
    {
        get => _searchParams.SearchTerm;
        set => _searchParams.SearchTerm = value;
    }

    /// <summary>
    ///     Sort parameters. Format example: sort=field1-asc,field2-desc
    /// </summary>
    [FromQuery(Name = "sort-params")]
    public string? SortParams
    {
        get => string.Join(',', _sortParams.Select(p => $"{p.FieldName}-{(p.IsDescending ? "desc" : "asc")}"));
        set => _sortParams.AddRange(SortExtensions.ParseSortParameters(value ?? ""));
    }

    public PaginationParams GetPaginationParams() => _paginationParams;
    public SearchParams GetSearchParams() => _searchParams;
    public SortParams GetSortParams() => _sortParams;
}