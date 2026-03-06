using System.Data;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface IUnitOfWork
{
    IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : class, IEntity<TKey>;
    
    /// <summary>
    ///     Save changes
    /// </summary>
    /// <param name="trackAudit">Whether to track audit changes</param>
    /// <param name="trackSoftDelete">Whether to track soft delete changes, if false, will delete entity from database</param>
    /// <returns>Number of affected rows</returns>
    Task<int> SaveChangesAsync(bool trackAudit = true, bool trackSoftDelete = true);

    /// <summary>
    ///     Begin a new transaction
    /// </summary>
    /// <param name="isolationLevel">Transaction isolation level</param>
    /// <returns>Transaction object</returns>
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead);

    /// <summary>
    ///     Commit the current transaction
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    ///     Rollback the current transaction
    /// </summary>
    Task RollbackTransactionAsync();

    /// <summary>
    ///     Execute operations within a transaction
    /// </summary>
    /// <param name="action">Action to execute within the transaction</param>
    /// <param name="isolationLevel">Transaction isolation level</param>
    /// <returns>Result of the operation</returns>
    Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead);

    /// <summary>
    ///     Execute operations within a transaction with no return value
    /// </summary>
    /// <param name="action">Action to execute within the transaction</param>
    /// <param name="isolationLevel">Transaction isolation level</param>
    Task ExecuteTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.RepeatableRead);

    Task<bool> EntityModifyTrackingContextAsync<T>(T entity) where T : class;
}