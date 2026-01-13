using System.Net;
using System.Runtime.InteropServices.JavaScript;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Shared;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EngConnect.BuildingBlock.Application.Validation.Validation;

public class ValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> 
    where TQuery : IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _decorated;
    private readonly IServiceProvider _serviceProvider;

    public ValidationQueryHandlerDecorator(
        IQueryHandler<TQuery, TResult> decorated,
        IServiceProvider serviceProvider)
    {
        _decorated = decorated;
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        var validator = _serviceProvider.GetService<IValidator<TQuery>>();
        
        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(query, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var error = new Error("Validation.Error", string.Join(", ", errors));
                return Result.Failure<TResult>(HttpStatusCode.BadRequest, error);
            }
        }
        
        return await _decorated.HandleAsync(query, cancellationToken);
    }
}