using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.UploadFileToDrive;

internal enum UploadFileToDriveCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class UploadFileToDriveTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UploadFileToDrive",
        RequestTypeFullName = "EngConnect.Application.UseCases.FileStorage.UploadFileToDrive.UploadFileToDriveCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.FileStorage.UploadFileToDrive.UploadFileToDriveCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.FileStorage.UploadFileToDrive.UploadFileToDriveCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/UploadFileToDrive/UploadFileToDriveCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/UploadFileToDrive/UploadFileToDriveCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/UploadFileToDrive/UploadFileToDriveCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UploadFileToDriveCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UploadFileToDriveCase.BoundaryDefault),
        BuildCase(UploadFileToDriveCase.InvalidRequestShape),
        BuildCase(UploadFileToDriveCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UploadFileToDriveCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UploadFileToDriveCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UploadFileToDriveCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UploadFileToDriveCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UploadFileToDriveCase caseId)
    {
        return caseId switch
        {
            UploadFileToDriveCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadFileToDriveCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadFileToDriveCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UploadFileToDriveCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.FileStorage.UploadFileToDrive.UploadFileToDriveCommand CreateRequest(UploadFileToDriveCase caseId)
    {
        return caseId switch
        {
            UploadFileToDriveCase.ValidDefault => new global::EngConnect.Application.UseCases.FileStorage.UploadFileToDrive.UploadFileToDriveCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Prefix = "SampleValue" },
            UploadFileToDriveCase.BoundaryDefault => new global::EngConnect.Application.UseCases.FileStorage.UploadFileToDrive.UploadFileToDriveCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Prefix = "SampleValue" },
            UploadFileToDriveCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.FileStorage.UploadFileToDrive.UploadFileToDriveCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Prefix = "" },
            UploadFileToDriveCase.ExceptionPath => new global::EngConnect.Application.UseCases.FileStorage.UploadFileToDrive.UploadFileToDriveCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Prefix = "SampleValue" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}