using EngConnect.BuildingBlock.Contracts.Shared;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EngConnect.BuildingBlock.Application.Base;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResult>> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = _serviceProvider.GetRequiredService(handlerType);
        
        var method = handlerType.GetMethod("HandleAsync");
        return await (Task<Result<TResult>>)method!.Invoke(handler, [command, cancellationToken])!;
    }
    
    public async Task<Result> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        var handler = _serviceProvider.GetRequiredService(handlerType);
    
        var method = handlerType.GetMethod("HandleAsync");
        return await (Task<Result>)method!.Invoke(handler, [command, cancellationToken])!;
    }
}