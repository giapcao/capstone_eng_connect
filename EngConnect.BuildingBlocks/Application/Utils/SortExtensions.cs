using System.Linq.Expressions;
using System.Reflection;
using EngConnect.BuildingBlock.Application.Params;
using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Application.Utils;

public static class SortExtensions
{
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, SortParams sortParams)
    {
        // [21-05-2025] If no sorting parameters are provided, default to sorting by 'createdAt' in descending order.
        if (sortParams.Count == 0)
        {
            sortParams.Add(new SortParameter("createdat", true));
        }

        // Apply the first sorting condition
        var firstSortParam = sortParams[0];
        var firstOrderByExpression = GetOrderByExpression<T>(firstSortParam);
        var orderedQuery = query.ApplySorting(firstSortParam.IsDescending, firstOrderByExpression);

        // Apply subsequent sorting conditions
        for (var i = 1; i < sortParams.Count; i++)
        {
            var currentSortParam = sortParams[i];
            var currentOrderByExpression = GetOrderByExpression<T>(currentSortParam);
            orderedQuery = orderedQuery?.ThenBy(currentSortParam.IsDescending, currentOrderByExpression);
        }

        return orderedQuery == null ? query : orderedQuery.AsQueryable();
    }

    private static Expression<Func<T, object>> GetOrderByExpression<T>(SortParameter sortParameter)
    {
        var propertyInfo = typeof(T).GetProperty(sortParameter.FieldName,
            BindingFlags.IgnoreCase |
            BindingFlags.Public |
            BindingFlags.Instance);
        if (propertyInfo == null)
        {
            throw new ArgumentException($"No property '{sortParameter.FieldName}' found on type '{typeof(T).Name}'");
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
        var orderByExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);
        return orderByExpression;
    }
    
    public static List<SortParameter> ParseSortParameters(string sortParams)
    {
        // Example input: "field1-asc,field2-desc"
        var result = new List<SortParameter>();
        foreach (var param in sortParams.Split(','))
            if (!string.IsNullOrEmpty(param))
            {
                var parts = param.Split('-');
                if (parts.Length != 2) throw new ArgumentException("Invalid sort parameter");
                if (parts[1] == "desc" || parts[1] == "asc")
                {
                    result.Add(new SortParameter(parts[0], parts[1] == "desc"));
                }
                else throw new ArgumentException("Invalid sort direction");
            }
        return result;
    }
}