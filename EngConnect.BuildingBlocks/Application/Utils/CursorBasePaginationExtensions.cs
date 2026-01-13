using System.Net;
using EngConnect.BuildingBlock.Application.Params;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.BuildingBlock.Application.Utils;

public static class CursorBasePaginationExtensions
{
    #region CusrorBasePaginationResult without projection

    public static async Task<CursorBasePaginationResult<T>> ToCursorPaginatedListAsync<T>(
        this IQueryable<T> source,
        CursorBaseParams paginationParams) where T : AuditableEntity<Guid>
    {
        try
        {
            if (!string.IsNullOrEmpty(paginationParams.After))
            {
                return await PassAfterParamsNonProjected(source, paginationParams);
            }

            if (!string.IsNullOrEmpty(paginationParams.Before))
            {
                return await PassBeforeParamsNonProjected(source, paginationParams);
            }

            return await DefaultWithNoParamsNonProjected(source, paginationParams);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // Return empty result on error since we can't return Result.Failure
            return new CursorBasePaginationResult<T>(
                new List<T>(),
                hasPrevious: false,
                hasNext: false,
                previousCursor: null,
                nextCursor: null
            );
        }
    }

    private static async Task<CursorBasePaginationResult<T>> DefaultWithNoParamsNonProjected<T>(
        IQueryable<T> source, CursorBaseParams paginationParams) where T : AuditableEntity<Guid>
    {
        var defaultSources = source
            .OrderByDescending(m => m.CreatedAt)
            .ThenByDescending(m => m.Id);

        var fetched = await defaultSources
            .Take(paginationParams.Limit + 1)
            .ToListAsync();

        var hasNext = fetched.Count > paginationParams.Limit;
        string? nextCursor = null;
        if (hasNext)
        {
            var originalItems = await defaultSources.Take(paginationParams.Limit).ToListAsync();
            nextCursor = Helper.EncodeCompoundCursor(originalItems.Last().CreatedAt, originalItems.Last().Id);
        }

        return new CursorBasePaginationResult<T>(
            await defaultSources.Take(paginationParams.Limit).ToListAsync(),
            hasPrevious: false,
            hasNext: hasNext,
            previousCursor: null,
            nextCursor: nextCursor
        );
    }

    private static async Task<CursorBasePaginationResult<T>> PassBeforeParamsNonProjected<T>(
        IQueryable<T> source, CursorBaseParams paginationParams)
        where T : AuditableEntity<Guid>
    {
        var (createdAt, id) = Helper.DecodeCompoundCursor(paginationParams.Before!);
        var orderedSource = source
            .Where(m => m.CreatedAt > createdAt || (m.CreatedAt == createdAt && m.Id.CompareTo(id) > 0))
            .OrderBy(m => m.CreatedAt).ThenBy(m => m.Id);

        var fetched = await orderedSource
            .Take(paginationParams.Limit + 1)
            .ToListAsync();

        var items = fetched.Take(paginationParams.Limit).Reverse().ToList();
        var originalItems = await orderedSource.Take(paginationParams.Limit).Reverse().ToListAsync();
        var hasMore = fetched.Count > paginationParams.Limit;

        return new CursorBasePaginationResult<T>(
            items,
            hasPrevious: hasMore,
            hasNext: true,
            previousCursor: hasMore
                ? Helper.EncodeCompoundCursor(originalItems.First().CreatedAt, originalItems.First().Id)
                : null,
            nextCursor: Helper.EncodeCompoundCursor(originalItems.Last().CreatedAt, originalItems.Last().Id)
        );
    }

    private static async Task<CursorBasePaginationResult<T>> PassAfterParamsNonProjected<T>(
        IQueryable<T> source, CursorBaseParams paginationParams)
        where T : AuditableEntity<Guid>
    {
        var (createdAt, id) = Helper.DecodeCompoundCursor(paginationParams.After!);
        var orderedSource = source
            .Where(m => m.CreatedAt < createdAt || (m.CreatedAt == createdAt && m.Id.CompareTo(id) < 0))
            .OrderByDescending(m => m.CreatedAt).ThenByDescending(m => m.Id);

        var fetched = await orderedSource
            .Take(paginationParams.Limit + 1)
            .ToListAsync();

        var items = fetched.Take(paginationParams.Limit).ToList();
        var originalItems = await orderedSource.Take(paginationParams.Limit).ToListAsync();
        var hasMore = fetched.Count > paginationParams.Limit;

        return new CursorBasePaginationResult<T>(
            items,
            hasPrevious: true,
            hasNext: hasMore,
            previousCursor: Helper.EncodeCompoundCursor(originalItems.First().CreatedAt, originalItems.First().Id),
            nextCursor: hasMore
                ? Helper.EncodeCompoundCursor(originalItems.Last().CreatedAt, originalItems.Last().Id)
                : null
        );
    }

    #endregion

    #region CursorBasePaginationResult with projection

    /// <summary>
    ///     Converts an IQueryable to a cursor-based paginated list with projection to a different type.
    /// </summary>
    /// <typeparam name="TSource">The source entity type</typeparam>
    /// <typeparam name="TResult">The result DTO type</typeparam>
    /// <param name="source">The source queryable</param>
    /// <param name="paginationParams">The cursor-based pagination parameters</param>
    /// <returns>A cursor-based paginated result with projected items</returns>
    public static async Task<Result<CursorBasePaginationResult<TResult>>> ProjectToCursorPaginatedListAsync
        <TSource, TResult>(this IQueryable<TSource> source, CursorBaseParams paginationParams)
        where TSource : AuditableEntity<Guid>
    {
        try
        {
            if (!string.IsNullOrEmpty(paginationParams.After))
            {
                return await PassAfterParams<TSource, TResult>(source, paginationParams);
            }

            if (!string.IsNullOrEmpty(paginationParams.Before))
            {
                return await PassBeforeParams<TSource, TResult>(source, paginationParams);
            }

            return await DefaultWithNoParams<TSource, TResult>(source, paginationParams);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Failure<CursorBasePaginationResult<TResult>>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }

    private static async Task<Result<CursorBasePaginationResult<TResult>>> DefaultWithNoParams<TSource, TResult>(
        IQueryable<TSource> source, CursorBaseParams paginationParams) where TSource : AuditableEntity<Guid>
    {
        var defaultSources = source
            .OrderByDescending(m => m.CreatedAt)
            .ThenByDescending(m => m.Id);

        var fetched = await defaultSources
            .Take(paginationParams.Limit + 1)
            .ProjectToType<TResult>()
            .ToListAsync();

        var hasNext = fetched.Count > paginationParams.Limit;
        string? nextCursor = null;
        if (hasNext)
        {
            var originalItems = await defaultSources.Take(paginationParams.Limit).ToListAsync();
            nextCursor = Helper.EncodeCompoundCursor(originalItems.Last().CreatedAt, originalItems.Last().Id);
        }

        var result = new CursorBasePaginationResult<TResult>(
            defaultSources.Take(paginationParams.Limit).ProjectToType<TResult>().ToList(),
            hasPrevious: false,
            hasNext: hasNext,
            previousCursor: null,
            nextCursor: nextCursor
        );

        return Result.Success(result);
    }

    private static async Task<Result<CursorBasePaginationResult<TResult>>> PassBeforeParams<TSource, TResult>(
        IQueryable<TSource> source, CursorBaseParams paginationParams)
        where TSource : AuditableEntity<Guid>
    {
        var (createdAt, id) = Helper.DecodeCompoundCursor(paginationParams.Before!);
        var orderedSource = source
            .Where(m => m.CreatedAt > createdAt || (m.CreatedAt == createdAt && m.Id.CompareTo(id) > 0))
            .OrderBy(m => m.CreatedAt).ThenBy(m => m.Id);

        var fetched = await orderedSource
            .Take(paginationParams.Limit + 1)
            .ProjectToType<TResult>()
            .ToListAsync();

        // Take only the requested number of items
        var items = fetched.Take(paginationParams.Limit).Reverse().ToList();

        var originalItems = await orderedSource.Take(paginationParams.Limit).Reverse().ToListAsync();

        var hasMore = fetched.Count > paginationParams.Limit;

        var result = new CursorBasePaginationResult<TResult>(
            items,
            hasPrevious: hasMore,
            hasNext: true,
            previousCursor: hasMore
                ? Helper.EncodeCompoundCursor(originalItems.First().CreatedAt, originalItems.First().Id)
                : null,
            nextCursor: Helper.EncodeCompoundCursor(originalItems.Last().CreatedAt, originalItems.Last().Id)
        );

        return Result.Success(result);
    }

    private static async Task<Result<CursorBasePaginationResult<TResult>>> PassAfterParams<TSource, TResult>(
        IQueryable<TSource> source, CursorBaseParams paginationParams)
        where TSource : AuditableEntity<Guid>
    {
        var (createdAt, id) = Helper.DecodeCompoundCursor(paginationParams.After!);
        var orderedSource = source
            .Where(m => m.CreatedAt < createdAt || (m.CreatedAt == createdAt && m.Id.CompareTo(id) < 0))
            .OrderByDescending(m => m.CreatedAt).ThenByDescending(m => m.Id);

        var fetched = await orderedSource
            .Take(paginationParams.Limit + 1)
            .ProjectToType<TResult>()
            .ToListAsync();

        // Take only the requested number of items
        var items = fetched.Take(paginationParams.Limit).ToList();

        var originalItems = await orderedSource.Take(paginationParams.Limit).ToListAsync();

        var hasMore = fetched.Count > paginationParams.Limit;

        var result = new CursorBasePaginationResult<TResult>(
            items,
            hasPrevious: true,
            hasNext: hasMore,
            previousCursor: Helper.EncodeCompoundCursor(originalItems.First().CreatedAt, originalItems.First().Id),
            nextCursor: hasMore
                ? Helper.EncodeCompoundCursor(originalItems.Last().CreatedAt, originalItems.Last().Id)
                : null
        );

        return Result.Success(result);
    }

    #endregion
}