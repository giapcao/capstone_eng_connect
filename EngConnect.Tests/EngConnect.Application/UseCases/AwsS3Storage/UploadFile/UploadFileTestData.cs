using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.UploadFile;

internal enum UploadFileCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class UploadFileTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UploadFile",
        RequestTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.UploadFile.UploadFileCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.UploadFile.UploadFileCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.UploadFile.UploadFileCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/UploadFile/UploadFileCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/UploadFile/UploadFileCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/UploadFile/UploadFileCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UploadFileCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UploadFileCase.BoundaryDefault),
        BuildCase(UploadFileCase.InvalidRequestShape),
        BuildCase(UploadFileCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UploadFileCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UploadFileCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UploadFileCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UploadFileCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UploadFileCase caseId)
    {
        return caseId switch
        {
            UploadFileCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadFileCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadFileCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UploadFileCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.AwsS3Storage.UploadFile.UploadFileCommand CreateRequest(UploadFileCase caseId)
    {
        return caseId switch
        {
            UploadFileCase.ValidDefault => new global::EngConnect.Application.UseCases.AwsS3Storage.UploadFile.UploadFileCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Prefix = "SampleValue" },
            UploadFileCase.BoundaryDefault => new global::EngConnect.Application.UseCases.AwsS3Storage.UploadFile.UploadFileCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Prefix = "SampleValue" },
            UploadFileCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.AwsS3Storage.UploadFile.UploadFileCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Prefix = "" },
            UploadFileCase.ExceptionPath => new global::EngConnect.Application.UseCases.AwsS3Storage.UploadFile.UploadFileCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Prefix = "SampleValue" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}