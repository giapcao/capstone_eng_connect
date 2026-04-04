namespace EngConnect.Tests.Common;

public sealed class UseCaseDefinition
{
    public required string UseCaseName { get; init; }

    public required string RequestTypeFullName { get; init; }

    public string? HandlerTypeFullName { get; init; }

    public string? ValidatorTypeFullName { get; init; }

    public required string RequestSourceRelativePath { get; init; }

    public string? HandlerSourceRelativePath { get; init; }

    public string? ValidatorSourceRelativePath { get; init; }
}
