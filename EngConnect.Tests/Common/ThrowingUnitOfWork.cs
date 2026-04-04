using System.Data;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace EngConnect.Tests.Common;

internal sealed class ThrowingUnitOfWork : IUnitOfWork
{
    private static readonly InvalidOperationException Exception =
        new("Forced test failure for handler invalid-path coverage.");

    public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : class, IEntity<TKey>
    {
        throw Exception;
    }

    public Task<int> SaveChangesAsync(bool trackAudit = true, bool trackSoftDelete = true)
    {
        throw Exception;
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        throw Exception;
    }

    public Task CommitTransactionAsync()
    {
        throw Exception;
    }

    public Task RollbackTransactionAsync()
    {
        throw Exception;
    }

    public Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action, IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        throw Exception;
    }

    public Task ExecuteTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        throw Exception;
    }

    public Task<bool> EntityModifyTrackingContextAsync<T>(T entity) where T : class
    {
        throw Exception;
    }
}
