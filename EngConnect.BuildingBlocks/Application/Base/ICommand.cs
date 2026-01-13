using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Application.Base;

public interface ICommand<TResult>
{
}

public interface ICommand : ICommand<Result>
{
}