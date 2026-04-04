using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.EnsureFolder;

internal enum EnsureFolderCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestGuardFolderName,
    InvalidRequestGuardParentFolderId,
    ExceptionPath,
}

internal static class EnsureFolderTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "EnsureFolder",
        RequestTypeFullName = "EngConnect.Application.UseCases.FileStorage.EnsureFolder.EnsureFolderCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.FileStorage.EnsureFolder.EnsureFolderCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.FileStorage.EnsureFolder.EnsureFolderCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/EnsureFolder/EnsureFolderCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/EnsureFolder/EnsureFolderCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/EnsureFolder/EnsureFolderCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(EnsureFolderCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(EnsureFolderCase.BoundaryDefault),
        BuildCase(EnsureFolderCase.InvalidRequestGuardFolderName),
        BuildCase(EnsureFolderCase.InvalidRequestGuardParentFolderId),
        BuildCase(EnsureFolderCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(EnsureFolderCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(EnsureFolderCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(EnsureFolderCase.InvalidRequestGuardFolderName),
        BuildCase(EnsureFolderCase.InvalidRequestGuardParentFolderId),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(EnsureFolderCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(EnsureFolderCase caseId)
    {
        return caseId switch
        {
            EnsureFolderCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            EnsureFolderCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            EnsureFolderCase.InvalidRequestGuardFolderName => CreateCase("invalid-request-guard-FolderName", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            EnsureFolderCase.InvalidRequestGuardParentFolderId => CreateCase("invalid-request-guard-ParentFolderId", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            EnsureFolderCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.FileStorage.EnsureFolder.EnsureFolderCommand CreateRequest(EnsureFolderCase caseId)
    {
        return caseId switch
        {
            EnsureFolderCase.ValidDefault => new global::EngConnect.Application.UseCases.FileStorage.EnsureFolder.EnsureFolderCommand { FolderName = "Sample", ParentFolderId = "SampleValue" },
            EnsureFolderCase.BoundaryDefault => new global::EngConnect.Application.UseCases.FileStorage.EnsureFolder.EnsureFolderCommand { FolderName = "Sample", ParentFolderId = "SampleValue" },
            EnsureFolderCase.InvalidRequestGuardFolderName => new global::EngConnect.Application.UseCases.FileStorage.EnsureFolder.EnsureFolderCommand { FolderName = "", ParentFolderId = "SampleValue" },
            EnsureFolderCase.InvalidRequestGuardParentFolderId => new global::EngConnect.Application.UseCases.FileStorage.EnsureFolder.EnsureFolderCommand { FolderName = "Sample", ParentFolderId = "" },
            EnsureFolderCase.ExceptionPath => new global::EngConnect.Application.UseCases.FileStorage.EnsureFolder.EnsureFolderCommand { FolderName = "Sample", ParentFolderId = "SampleValue" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}