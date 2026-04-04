using EngConnect.BuildingBlock.Contracts.Abstraction;
using Moq;

namespace EngConnect.Tests.Common;

public sealed class UseCaseMockContext
{
    private readonly Dictionary<Type, object?> _overrides = [];

    public IReadOnlyDictionary<Type, object?> Overrides => _overrides;

    public void Merge(IReadOnlyDictionary<Type, object?> overrides)
    {
        foreach (var (type, value) in overrides)
        {
            _overrides[type] = value;
        }
    }

    public void UseSeededUnitOfWork(bool seedData = true)
    {
        _overrides[typeof(IUnitOfWork)] = new InMemoryUnitOfWork(seedData);
    }

    public void UseThrowingUnitOfWork()
    {
        _overrides[typeof(IUnitOfWork)] = new ThrowingUnitOfWork();
    }

    public void Override<T>(T instance)
    {
        _overrides[typeof(T)] = instance;
    }

    public Mock<T> LooseMock<T>() where T : class
    {
        var mock = new Mock<T>(MockBehavior.Loose)
        {
            DefaultValue = DefaultValue.Mock
        };

        _overrides[typeof(T)] = mock.Object;
        return mock;
    }

    public Mock<T> StrictMock<T>() where T : class
    {
        var mock = new Mock<T>(MockBehavior.Strict)
        {
            DefaultValue = DefaultValue.Mock
        };

        _overrides[typeof(T)] = mock.Object;
        return mock;
    }
}
