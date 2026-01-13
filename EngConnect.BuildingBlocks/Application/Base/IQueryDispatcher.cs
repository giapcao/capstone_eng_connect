using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Application.Base;

public interface IQueryDispatcher
{
    Task<Result<TResult>> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}