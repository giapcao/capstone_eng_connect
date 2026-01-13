using EngConnect.BuildingBlock.Contracts.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace EngConnect.BuildingBlock.Application.Base;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResult>> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = _serviceProvider.GetRequiredService(handlerType);
        
        var method = handlerType.GetMethod("HandleAsync");
        return await (Task<Result<TResult>>)method!.Invoke(handler, [query, cancellationToken])!;
    }
}