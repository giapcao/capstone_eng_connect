using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Tests.Common;

internal sealed class RecordingQueryDispatcher : IQueryDispatcher
{
    private readonly List<object> _queries = [];

    public IReadOnlyList<object> Queries => _queries;

    public int DispatchCount => _queries.Count;

    public Task<Result<TResult>> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        _queries.Add(query);

        var value = TestObjectFactory.CreateValue(typeof(TResult));
        return Task.FromResult(Result.Success((TResult?)value!));
    }
}
