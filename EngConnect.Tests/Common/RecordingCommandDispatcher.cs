using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Tests.Common;

internal sealed class RecordingCommandDispatcher : ICommandDispatcher
{
    private readonly List<object> _commands = [];

    public IReadOnlyList<object> Commands => _commands;

    public int DispatchCount => _commands.Count;

    public Task<Result<TResult>> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        _commands.Add(command);

        var value = TestObjectFactory.CreateValue(typeof(TResult));
        return Task.FromResult(Result.Success((TResult?)value!));
    }

    public Task<Result> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        _commands.Add(command);
        return Task.FromResult(Result.Success());
    }
}
