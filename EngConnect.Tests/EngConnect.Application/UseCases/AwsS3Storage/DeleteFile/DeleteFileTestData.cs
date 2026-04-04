using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.DeleteFile;

internal enum DeleteFileCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class DeleteFileTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteFile",
        RequestTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.DeleteFile.DeleteFileCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.DeleteFile.DeleteFileCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.DeleteFile.DeleteFileCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/DeleteFile/DeleteFileCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/DeleteFile/DeleteFileCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/DeleteFile/DeleteFileCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteFileCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteFileCase.BoundaryDefault),
        BuildCase(DeleteFileCase.InvalidRequestShape),
        BuildCase(DeleteFileCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteFileCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteFileCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteFileCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteFileCase.ExceptionPath),
    ];

    public static IEnumerable<object[]> NormalHandlerCases()
    {
        return NormalCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> NormalValidatorCases()
    {
        return NormalCases
            .Where(caseSet => caseSet.ValidatorExpectation != UseCaseValidatorExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> HandlerBranchCases()
    {
        return BranchCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> ValidatorBranchCases()
    {
        return BranchCases
            .Where(caseSet => caseSet.ValidatorExpectation != UseCaseValidatorExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    private static UseCaseCaseSet BuildCase(DeleteFileCase caseId)
    {
        return caseId switch
        {
            DeleteFileCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            DeleteFileCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            DeleteFileCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            DeleteFileCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateCase(
        string name,
        UseCaseCaseKind kind,
        UseCaseHandlerExpectation handlerExpectation,
        UseCaseValidatorExpectation validatorExpectation,
        object request)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = kind,
            HandlerExpectation = handlerExpectation,
            ValidatorExpectation = validatorExpectation,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = request
                }
            }
        };
    }

    private static global::EngConnect.Application.UseCases.AwsS3Storage.DeleteFile.DeleteFileCommand CreateRequest(DeleteFileCase caseId)
    {
        return caseId switch
        {
            DeleteFileCase.ValidDefault => new global::EngConnect.Application.UseCases.AwsS3Storage.DeleteFile.DeleteFileCommand { FileName = "Sample" },
            DeleteFileCase.BoundaryDefault => new global::EngConnect.Application.UseCases.AwsS3Storage.DeleteFile.DeleteFileCommand { FileName = "Sample" },
            DeleteFileCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.AwsS3Storage.DeleteFile.DeleteFileCommand { FileName = "" },
            DeleteFileCase.ExceptionPath => new global::EngConnect.Application.UseCases.AwsS3Storage.DeleteFile.DeleteFileCommand { FileName = "Sample" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}