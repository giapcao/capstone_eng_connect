using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Shared;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EngConnect.BuildingBlock.Application.Validation.Validation;

public class ValidationCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _decorated;
    private readonly IServiceProvider _serviceProvider;

    public ValidationCommandHandlerDecorator(
        ICommandHandler<TCommand, TResult> decorated,
        IServiceProvider serviceProvider)
    {
        _decorated = decorated;
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResult>> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var validator = _serviceProvider.GetService<IValidator<TCommand>>();

        if (validator == null) return await _decorated.HandleAsync(command, cancellationToken);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid) return await _decorated.HandleAsync(command, cancellationToken);
        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        var error = new Error("Validation.Error", string.Join(", ", errors));
        return Result.Failure<TResult>(HttpStatusCode.BadRequest, error);

    }
}

public class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> 
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _decorated;
    private readonly IServiceProvider _serviceProvider;

    public ValidationCommandHandlerDecorator(
        ICommandHandler<TCommand> decorated,
        IServiceProvider serviceProvider)
    {
        _decorated = decorated;
        _serviceProvider = serviceProvider;
    }

    public async Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var validator = _serviceProvider.GetService<IValidator<TCommand>>();

        if (validator == null) return await _decorated.HandleAsync(command, cancellationToken);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid) return await _decorated.HandleAsync(command, cancellationToken);
        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        var error = new Error("Validation.Error", string.Join(", ", errors));
        return Result.Failure(HttpStatusCode.BadRequest, error);
    }
}
