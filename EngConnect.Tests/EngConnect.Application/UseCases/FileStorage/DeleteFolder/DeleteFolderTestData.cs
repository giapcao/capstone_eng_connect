using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.DeleteFolder;

internal enum DeleteFolderCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestGuardFolderId,
    ExceptionPath,
}

internal static class DeleteFolderTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteFolder",
        RequestTypeFullName = "EngConnect.Application.UseCases.FileStorage.DeleteFolder.DeleteFolderCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.FileStorage.DeleteFolder.DeleteFolderCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.FileStorage.DeleteFolder.DeleteFolderCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/DeleteFolder/DeleteFolderCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/DeleteFolder/DeleteFolderCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/DeleteFolder/DeleteFolderCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteFolderCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteFolderCase.BoundaryDefault),
        BuildCase(DeleteFolderCase.InvalidRequestGuardFolderId),
        BuildCase(DeleteFolderCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteFolderCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteFolderCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteFolderCase.InvalidRequestGuardFolderId),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteFolderCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteFolderCase caseId)
    {
        return caseId switch
        {
            DeleteFolderCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            DeleteFolderCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            DeleteFolderCase.InvalidRequestGuardFolderId => CreateCase("invalid-request-guard-FolderId", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            DeleteFolderCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.FileStorage.DeleteFolder.DeleteFolderCommand CreateRequest(DeleteFolderCase caseId)
    {
        return caseId switch
        {
            DeleteFolderCase.ValidDefault => new global::EngConnect.Application.UseCases.FileStorage.DeleteFolder.DeleteFolderCommand { FolderId = "SampleValue" },
            DeleteFolderCase.BoundaryDefault => new global::EngConnect.Application.UseCases.FileStorage.DeleteFolder.DeleteFolderCommand { FolderId = "SampleValue" },
            DeleteFolderCase.InvalidRequestGuardFolderId => new global::EngConnect.Application.UseCases.FileStorage.DeleteFolder.DeleteFolderCommand { FolderId = "" },
            DeleteFolderCase.ExceptionPath => new global::EngConnect.Application.UseCases.FileStorage.DeleteFolder.DeleteFolderCommand { FolderId = "SampleValue" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}