using System.Linq.Expressions;
using EngConnect.BuildingBlock.Application.Params;
using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Application.Utils;

public static class SearchExtensions
{
    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, SearchParams searchParams, params Expression<Func<T, string>>[] searchProperties)
    {
        if (string.IsNullOrWhiteSpace(searchParams.SearchTerm) || searchProperties.Length == 0)
        {
            return query;
        }

        var normalizedSearchTerm = searchParams.NormalizedSearchTerm;

        Expression<Func<T, bool>>? searchExpression = null;
        // Create a parameter expression representing the entity type T
        var parameter = Expression.Parameter(typeof(T), "x");

        // Get the method info for string operations
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;
        var replaceMethod = typeof(string).GetMethod("Replace", [typeof(string), typeof(string)])!;
        var trimMethod = typeof(string).GetMethod("Trim", Type.EmptyTypes)!;
        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;

        // Iterate over each property expression provided for searching
        foreach (var property in searchProperties)
        {
            // Invoke the property expression on the parameter to access the property value
            var propertyAccess = Expression.Invoke(property, parameter);

            // Normalize the property value: property.Replace(" ", "").Trim().ToLower()
            var removeSpaces = Expression.Call(propertyAccess, replaceMethod, 
                Expression.Constant(" "), Expression.Constant(""));
            var trimmed = Expression.Call(removeSpaces, trimMethod);
            var normalizedProperty = Expression.Call(trimmed, toLowerMethod);

            // Create an expression to check if the normalized property contains the normalized search term
            var containsMethodCallExpression = Expression.Call(normalizedProperty, containsMethod, Expression.Constant(normalizedSearchTerm));

            var containsExpression = Expression.Lambda<Func<T, bool>>(containsMethodCallExpression, parameter);

            // Combine the current contains expression with the existing search expression using logical OR
            searchExpression = searchExpression == null ? containsExpression : searchExpression.CombineOrExpressions(containsExpression);
        }

        // If no valid search expression was created, return the original query
        if (searchExpression == null)
        {
            return query;
        }

        // Apply the search expression to the query using the Where method
        return query.Where(searchExpression);
    }
}