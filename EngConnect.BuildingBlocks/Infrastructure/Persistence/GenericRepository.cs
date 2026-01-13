using System.Linq.Expressions;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.BuildingBlock.Infrastructure.Persistence;

public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected readonly DbContext Context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext context)
    {
        Context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> FindByIdAsync(TKey id, bool tracking = true,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public async Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = true,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.AsSingleQuery().SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = true,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.AsSingleQuery().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = false,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query;
    }

    public IQueryable<TEntity> FindFromSqlInterpolated(FormattableString sql,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine("FindFromSqlInterpolated Tx Connection: " + Context.Database.GetDbConnection().GetHashCode());
        return _dbSet.FromSqlInterpolated(sql);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.CountAsync(cancellationToken);
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}