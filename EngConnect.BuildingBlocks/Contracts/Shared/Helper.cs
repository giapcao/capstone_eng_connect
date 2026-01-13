using System.Linq.Expressions;

namespace EngConnect.BuildingBlock.Contracts.Shared;

public static class Helper
{
    /// <summary>
    ///     Combine two expressions with AndAlso operator (&&)
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CombineAndAlsoExpressions<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return CombineExpressions(first, second, Expression.AndAlso);
    }

    /// <summary>
    ///     Combine two expressions with OrElse operator (||)
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CombineOrExpressions<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return CombineExpressions(first, second, Expression.OrElse);
    }

    private static Expression<Func<T, bool>> CombineExpressions<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2, Func<Expression, Expression, BinaryExpression> combiner)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        return Expression.Lambda<Func<T, bool>>(combiner(left, right), parameter);
    }

    // Apply initial sorting 
    public static IOrderedQueryable<T> ApplySorting<T>(this IQueryable<T> source, bool isDescending,
        Expression<Func<T, object>> sortProperty)
    {
        return isDescending ? source.OrderByDescending(sortProperty) : source.OrderBy(sortProperty);
    }

    // Apply more sorting
    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, bool isDescending,
        Expression<Func<T, object>> sortProperty)
    {
        return isDescending ? source.ThenByDescending(sortProperty) : source.ThenBy(sortProperty);
    }

    public static bool IsNullOrGuidEmpty(this Guid? guid)
    {
        return guid == null || guid == Guid.Empty;
    }

    /// <summary>
    /// Validates if a string is a valid non-empty GUID.
    /// </summary>
    public static bool IsValidGuid(this string? value, out Guid parsedGuid)
    {
        parsedGuid = Guid.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (!Guid.TryParse(value, out var guid))
        {
            return false;
        }

        // Ensure it's not Guid.Empty
        if (guid == Guid.Empty)
        {
            return false;
        }

        parsedGuid = guid;
        return true;
    }
}

internal class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
{
    public override Expression Visit(Expression? node)
    {
        return node == oldValue ? newValue : base.Visit(node)!;
    }
}