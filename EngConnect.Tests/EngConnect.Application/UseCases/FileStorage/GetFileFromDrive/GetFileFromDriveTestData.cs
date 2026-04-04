using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.GetFileFromDrive;

internal enum GetFileFromDriveCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class GetFileFromDriveTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetFileFromDrive",
        RequestTypeFullName = "EngConnect.Application.UseCases.FileStorage.GetFileFromDrive.GetFileFromDriveCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.FileStorage.GetFileFromDrive.GetFileFromDriveCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.FileStorage.GetFileFromDrive.GetFileFromDriveCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/GetFileFromDrive/GetFileFromDriveCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/GetFileFromDrive/GetFileFromDriveCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/GetFileFromDrive/GetFileFromDriveCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetFileFromDriveCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetFileFromDriveCase.BoundaryDefault),
        BuildCase(GetFileFromDriveCase.InvalidRequestShape),
        BuildCase(GetFileFromDriveCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetFileFromDriveCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetFileFromDriveCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetFileFromDriveCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetFileFromDriveCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetFileFromDriveCase caseId)
    {
        return caseId switch
        {
            GetFileFromDriveCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetFileFromDriveCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetFileFromDriveCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            GetFileFromDriveCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.FileStorage.GetFileFromDrive.GetFileFromDriveCommand CreateRequest(GetFileFromDriveCase caseId)
    {
        return caseId switch
        {
            GetFileFromDriveCase.ValidDefault => new global::EngConnect.Application.UseCases.FileStorage.GetFileFromDrive.GetFileFromDriveCommand { FileId = "SampleValue" },
            GetFileFromDriveCase.BoundaryDefault => new global::EngConnect.Application.UseCases.FileStorage.GetFileFromDrive.GetFileFromDriveCommand { FileId = "SampleValue" },
            GetFileFromDriveCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.FileStorage.GetFileFromDrive.GetFileFromDriveCommand { FileId = "" },
            GetFileFromDriveCase.ExceptionPath => new global::EngConnect.Application.UseCases.FileStorage.GetFileFromDrive.GetFileFromDriveCommand { FileId = "SampleValue" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}