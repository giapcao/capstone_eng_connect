using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;

namespace EngConnect.Tests.Common;

internal sealed class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private static readonly MethodInfo ExecuteMethod = typeof(IQueryProvider)
        .GetMethods()
        .Single(method => method.Name == nameof(IQueryProvider.Execute)
                          && method.IsGenericMethod
                          && method.GetParameters().Length == 1);

    private static readonly MethodInfo TaskFromResultMethod = typeof(Task)
        .GetMethods()
        .Single(method => method.Name == nameof(Task.FromResult)
                          && method.IsGenericMethod
                          && method.GetParameters().Length == 1);

    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        var elementType = expression.Type.GetGenericArguments().FirstOrDefault() ?? typeof(TEntity);
        return (IQueryable)Activator.CreateInstance(typeof(TestAsyncEnumerable<>).MakeGenericType(elementType), expression)!;
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object? Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        var resultType = typeof(TResult).GetGenericArguments().FirstOrDefault();
        if (resultType == null)
        {
            return Execute<TResult>(expression);
        }

        var executionResult = ExecuteMethod.MakeGenericMethod(resultType).Invoke(_inner, [expression]);
        return (TResult)TaskFromResultMethod.MakeGenericMethod(resultType).Invoke(null, [executionResult])!;
    }
}
