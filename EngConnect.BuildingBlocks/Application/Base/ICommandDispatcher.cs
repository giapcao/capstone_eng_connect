using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Application.Base;

public interface ICommandDispatcher
{
    Task<Result<TResult>> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task<Result> DispatchAsync(ICommand command, CancellationToken cancellationToken = default);

}