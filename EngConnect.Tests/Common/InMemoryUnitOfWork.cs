using System.Data;
using System.Linq.Expressions;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace EngConnect.Tests.Common;

internal sealed class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly Dictionary<(Type EntityType, Type KeyType), object> _repositories = [];
    private readonly IDbContextTransaction _transaction = Mock.Of<IDbContextTransaction>();
    private readonly bool _seedData;

    public InMemoryUnitOfWork(bool seedData = true)
    {
        _seedData = seedData;
    }

    public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : class, IEntity<TKey>
    {
        var key = (typeof(T), typeof(TKey));
        if (!_repositories.TryGetValue(key, out var repository))
        {
            repository = new InMemoryGenericRepository<T, TKey>(_seedData);
            _repositories[key] = repository;
        }

        return (IGenericRepository<T, TKey>)repository;
    }

    public Task<int> SaveChangesAsync(bool trackAudit = true, bool trackSoftDelete = true)
    {
        return Task.FromResult(1);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        return Task.FromResult(_transaction);
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action, IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        return action();
    }

    public Task ExecuteTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
    {
        return action();
    }

    public Task<bool> EntityModifyTrackingContextAsync<T>(T entity) where T : class
    {
        return Task.FromResult(true);
    }

    private sealed class InMemoryGenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly List<TEntity> _entities;

        public InMemoryGenericRepository(bool seedData)
        {
            _entities = seedData ? [CreateSeedEntity()] : [];
        }

        public Task<TEntity?> FindByIdAsync(TKey id, bool tracking = true, CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var entity = _entities.FirstOrDefault(candidate => EqualityComparer<TKey>.Default.Equals(candidate.Id, id));

            return Task.FromResult(entity);
        }

        public Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = true,
            CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            return Task.FromResult(ApplyPredicate(predicate).FirstOrDefault());
        }

        public Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = true,
            CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            return Task.FromResult(ApplyPredicate(predicate).FirstOrDefault());
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            return Task.FromResult(ApplyPredicate(predicate).Any());
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = false,
            CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            return CreateQueryable(ApplyPredicate(predicate));
        }

        public IQueryable<TEntity> FindFromSqlInterpolated(FormattableString sql, CancellationToken cancellationToken = default)
        {
            return CreateQueryable(_entities);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            return Task.FromResult(ApplyPredicate(predicate).Count());
        }

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void Update(TEntity entity)
        {
            var index = _entities.FindIndex(candidate => EqualityComparer<TKey>.Default.Equals(candidate.Id, entity.Id));
            if (index >= 0)
            {
                _entities[index] = entity;
                return;
            }

            _entities.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _entities.RemoveAll(candidate => EqualityComparer<TKey>.Default.Equals(candidate.Id, entity.Id));
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        private IEnumerable<TEntity> ApplyPredicate(Expression<Func<TEntity, bool>>? predicate)
        {
            if (predicate == null)
            {
                return _entities;
            }

            try
            {
                return _entities.AsQueryable().Where(predicate).ToList();
            }
            catch
            {
                return _entities;
            }
        }

        private static IQueryable<TEntity> CreateQueryable(IEnumerable<TEntity> entities)
        {
            return new TestAsyncEnumerable<TEntity>(entities);
        }

        private static TEntity CreateSeedEntity()
        {
            var entity = TestObjectFactory.CreateValue<TEntity>();
            if (entity == null)
            {
                throw new InvalidOperationException($"Cannot create seed entity for {typeof(TEntity).FullName}.");
            }

            if (EqualityComparer<TKey>.Default.Equals(entity.Id, default!))
            {
                entity.Id = (TKey)(TestObjectFactory.CreateValue(typeof(TKey))
                                   ?? throw new InvalidOperationException($"Cannot create key for {typeof(TKey).FullName}."));
            }

            return entity;
        }
    }
}
