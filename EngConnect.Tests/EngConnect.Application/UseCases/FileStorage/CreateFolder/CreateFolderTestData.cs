using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.CreateFolder;

internal enum CreateFolderCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestGuardFolderName,
    InvalidRequestGuardParentFolderId,
    ExceptionPath,
}

internal static class CreateFolderTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateFolder",
        RequestTypeFullName = "EngConnect.Application.UseCases.FileStorage.CreateFolder.CreateFolderCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.FileStorage.CreateFolder.CreateFolderCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.FileStorage.CreateFolder.CreateFolderCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/CreateFolder/CreateFolderCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/CreateFolder/CreateFolderCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/CreateFolder/CreateFolderCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateFolderCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateFolderCase.BoundaryDefault),
        BuildCase(CreateFolderCase.InvalidRequestGuardFolderName),
        BuildCase(CreateFolderCase.InvalidRequestGuardParentFolderId),
        BuildCase(CreateFolderCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreateFolderCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateFolderCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateFolderCase.InvalidRequestGuardFolderName),
        BuildCase(CreateFolderCase.InvalidRequestGuardParentFolderId),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateFolderCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreateFolderCase caseId)
    {
        return caseId switch
        {
            CreateFolderCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateFolderCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateFolderCase.InvalidRequestGuardFolderName => CreateCase("invalid-request-guard-FolderName", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateFolderCase.InvalidRequestGuardParentFolderId => CreateCase("invalid-request-guard-ParentFolderId", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateFolderCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.FileStorage.CreateFolder.CreateFolderCommand CreateRequest(CreateFolderCase caseId)
    {
        return caseId switch
        {
            CreateFolderCase.ValidDefault => new global::EngConnect.Application.UseCases.FileStorage.CreateFolder.CreateFolderCommand { FolderName = "Sample", ParentFolderId = "SampleValue" },
            CreateFolderCase.BoundaryDefault => new global::EngConnect.Application.UseCases.FileStorage.CreateFolder.CreateFolderCommand { FolderName = "Sample", ParentFolderId = "SampleValue" },
            CreateFolderCase.InvalidRequestGuardFolderName => new global::EngConnect.Application.UseCases.FileStorage.CreateFolder.CreateFolderCommand { FolderName = "", ParentFolderId = "SampleValue" },
            CreateFolderCase.InvalidRequestGuardParentFolderId => new global::EngConnect.Application.UseCases.FileStorage.CreateFolder.CreateFolderCommand { FolderName = "Sample", ParentFolderId = "" },
            CreateFolderCase.ExceptionPath => new global::EngConnect.Application.UseCases.FileStorage.CreateFolder.CreateFolderCommand { FolderName = "Sample", ParentFolderId = "SampleValue" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}