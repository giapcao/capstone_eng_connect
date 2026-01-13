using System.Collections;
using System.Data;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using EngConnect.BuildingBlock.Infrastructure.Persistence;
using EngConnect.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EngConnect.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _currentTransaction;
    private bool _disposed;
    private Hashtable? _repositories;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : class, IEntity<TKey>
    {
        _repositories ??= [];
        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<,>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T), typeof(TKey)),
                _context);

            _repositories.Add(type, repositoryInstance);
        }

        return (IGenericRepository<T, TKey>)_repositories[type]!;
    }

    public async Task<int> SaveChangesAsync(bool trackAudit = true, bool trackSoftDelete = true)
    {
        if (trackAudit)
        {
            ApplyAuditInfo();
        }

        if (trackSoftDelete)
        {
            ApplySoftDelete();
        }

        return await _context.SaveChangesAsync();
    }

    public async Task<IDbTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        if (_currentTransaction != null)
        {
            return _currentTransaction.GetDbTransaction();
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync(isolationLevel);
        return _currentTransaction.GetDbTransaction();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit");
        }

        try
        {
            await _currentTransaction.CommitAsync();
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        // Don't create a transaction if one already exists
        var hasExistingTransaction = _currentTransaction != null;

        try
        {
            if (!hasExistingTransaction)
            {
                await BeginTransactionAsync(isolationLevel);
            }

            var result = await action();

            if (!hasExistingTransaction)
            {
                // SaveChanges must be called explicitly by the action
                // before we commit the transaction
                await CommitTransactionAsync();
            }

            return result;
        }
        catch
        {
            if (!hasExistingTransaction)
            {
                await RollbackTransactionAsync();
            }

            throw;
        }
    }

    public async Task ExecuteTransactionAsync(Func<Task> action,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        await ExecuteTransactionAsync(async () =>
        {
            await action();
            return true;
        }, isolationLevel);
    }

    public Task<bool> EntityModifyTrackingContextAsync<T>(T entity) where T : class
    {
        var trackedEntity = _context.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity == entity);
        if (trackedEntity == null)
        {
            return Task.FromResult(false);
        }

        if (trackedEntity.State == EntityState.Modified)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    private void ApplyAuditInfo()
    {
        var entries = _context.ChangeTracker.Entries()
            .Where(e => e is { Entity: IAuditable, State: EntityState.Added or EntityState.Modified });

        foreach (var entry in entries)
        {
            var entity = (IAuditable)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

    private void ApplySoftDelete()
    {
        var entries = _context.ChangeTracker.Entries()
            .Where(e => e is { Entity: ISoftDeletable, State: EntityState.Deleted });

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            var entity = (ISoftDeletable)entry.Entity;
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
            _currentTransaction?.Dispose();
        }

        _disposed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            await _context.DisposeAsync();

            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }

            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}