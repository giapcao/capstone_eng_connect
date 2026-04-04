using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.GetFolder;

internal enum GetFolderCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestGuardFolderName,
    InvalidRequestGuardParentFolderId,
    ExceptionPath,
}

internal static class GetFolderTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetFolder",
        RequestTypeFullName = "EngConnect.Application.UseCases.FileStorage.GetFolder.GetFolderCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.FileStorage.GetFolder.GetFolderCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.FileStorage.GetFolder.GetFolderCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/GetFolder/GetFolderCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/GetFolder/GetFolderCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/GetFolder/GetFolderCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetFolderCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetFolderCase.BoundaryDefault),
        BuildCase(GetFolderCase.InvalidRequestGuardFolderName),
        BuildCase(GetFolderCase.InvalidRequestGuardParentFolderId),
        BuildCase(GetFolderCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetFolderCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetFolderCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetFolderCase.InvalidRequestGuardFolderName),
        BuildCase(GetFolderCase.InvalidRequestGuardParentFolderId),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetFolderCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetFolderCase caseId)
    {
        return caseId switch
        {
            GetFolderCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetFolderCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetFolderCase.InvalidRequestGuardFolderName => CreateCase("invalid-request-guard-FolderName", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            GetFolderCase.InvalidRequestGuardParentFolderId => CreateCase("invalid-request-guard-ParentFolderId", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            GetFolderCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.FileStorage.GetFolder.GetFolderCommand CreateRequest(GetFolderCase caseId)
    {
        return caseId switch
        {
            GetFolderCase.ValidDefault => new global::EngConnect.Application.UseCases.FileStorage.GetFolder.GetFolderCommand { FolderName = "Sample", ParentFolderId = "SampleValue" },
            GetFolderCase.BoundaryDefault => new global::EngConnect.Application.UseCases.FileStorage.GetFolder.GetFolderCommand { FolderName = "Sample", ParentFolderId = "SampleValue" },
            GetFolderCase.InvalidRequestGuardFolderName => new global::EngConnect.Application.UseCases.FileStorage.GetFolder.GetFolderCommand { FolderName = "", ParentFolderId = "SampleValue" },
            GetFolderCase.InvalidRequestGuardParentFolderId => new global::EngConnect.Application.UseCases.FileStorage.GetFolder.GetFolderCommand { FolderName = "Sample", ParentFolderId = "" },
            GetFolderCase.ExceptionPath => new global::EngConnect.Application.UseCases.FileStorage.GetFolder.GetFolderCommand { FolderName = "Sample", ParentFolderId = "SampleValue" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}