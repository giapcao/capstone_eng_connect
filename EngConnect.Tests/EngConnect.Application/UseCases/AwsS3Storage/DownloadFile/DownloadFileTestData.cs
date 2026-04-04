using System.Net;
using System.Text;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.DownloadFile;

internal enum DownloadFileCase
{
    ValidExistingFile,
    BoundarySingleCharacterFileName,
    InvalidRequestShape,
    InvalidFileMissing,
    ExceptionStorageThrows,
}

internal static class DownloadFileTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DownloadFile",
        RequestTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQueryHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQueryValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/DownloadFile/DownloadFileQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/DownloadFile/DownloadFileQueryHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/DownloadFile/DownloadFileQueryValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DownloadFileCase.ValidExistingFile)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DownloadFileCase.BoundarySingleCharacterFileName),
        BuildCase(DownloadFileCase.InvalidRequestShape),
        BuildCase(DownloadFileCase.InvalidFileMissing),
        BuildCase(DownloadFileCase.ExceptionStorageThrows)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DownloadFileCase.BoundarySingleCharacterFileName)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DownloadFileCase.InvalidRequestShape),
        BuildCase(DownloadFileCase.InvalidFileMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DownloadFileCase.ExceptionStorageThrows)
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

    private static UseCaseCaseSet BuildCase(DownloadFileCase caseId)
    {
        return caseId switch
        {
            DownloadFileCase.ValidExistingFile => CreateSuccessCase(
                "valid-existing-file",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                CreateFileResult("lesson-recordings/summary.pdf")),
            DownloadFileCase.BoundarySingleCharacterFileName => CreateSuccessCase(
                "boundary-single-character-file-name",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                CreateFileResult("a")),
            DownloadFileCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            DownloadFileCase.InvalidFileMissing => CreateFailureCase(
                "invalid-file-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var storageServiceMock = mocks.StrictMock<IAwsStorageService>();
                    storageServiceMock
                        .Setup(service => service.FileExistsAsync("missing-file.pdf", It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);
                },
                expectedStatusCode: HttpStatusCode.NotFound),
            DownloadFileCase.ExceptionStorageThrows => CreateFailureCase(
                "exception-storage-throws",
                UseCaseCaseKind.Exception,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var storageServiceMock = mocks.StrictMock<IAwsStorageService>();
                    storageServiceMock
                        .Setup(service => service.FileExistsAsync("broken-file.pdf", It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new InvalidOperationException("s3 unavailable"));
                },
                expectedStatusCode: HttpStatusCode.InternalServerError),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQuery request,
        FileReadResult expectedFile)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = kind,
            HandlerExpectation = UseCaseHandlerExpectation.Completes,
            ValidatorExpectation = UseCaseValidatorExpectation.Pass,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = mocks =>
                    {
                        var storageServiceMock = mocks.StrictMock<IAwsStorageService>();

                        storageServiceMock
                            .Setup(service => service.FileExistsAsync(request.FileName, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(true);
                        storageServiceMock
                            .Setup(service => service.GetFileStreamAsync(request.FileName, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(expectedFile);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<FileReadResult>>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(result.Data);
                        Assert.Equal(expectedFile.RelativePath, result.Data!.RelativePath);
                        Assert.Equal(expectedFile.FileName, result.Data.FileName);
                        Assert.Equal(expectedFile.ContentType, result.Data.ContentType);
                        Assert.Equal(expectedFile.Size, result.Data.Size);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQuery request,
        Action<UseCaseMockContext> arrangeMocks,
        HttpStatusCode expectedStatusCode)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = kind,
            HandlerExpectation = UseCaseHandlerExpectation.Failure,
            ValidatorExpectation = UseCaseValidatorExpectation.Pass,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = arrangeMocks,
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);

                        Assert.True(result.IsFailure);
                        Assert.Equal(expectedStatusCode, result.HttpStatusCode);
                        return Task.CompletedTask;
                    }
                }
            }
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

    private static FileReadResult CreateFileResult(string relativePath)
    {
        return new FileReadResult
        {
            FileName = Path.GetFileName(relativePath),
            ContentType = "application/pdf",
            RelativePath = relativePath,
            Size = 128,
            IsPrivate = true,
            Stream = new MemoryStream(Encoding.UTF8.GetBytes("seed-file"))
        };
    }

    private static global::EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQuery CreateRequest(DownloadFileCase caseId)
    {
        return caseId switch
        {
            DownloadFileCase.ValidExistingFile => new global::EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQuery
            {
                FileName = "lesson-recordings/summary.pdf"
            },
            DownloadFileCase.BoundarySingleCharacterFileName => new global::EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQuery
            {
                FileName = "a"
            },
            DownloadFileCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQuery
            {
                FileName = string.Empty
            },
            DownloadFileCase.InvalidFileMissing => new global::EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQuery
            {
                FileName = "missing-file.pdf"
            },
            DownloadFileCase.ExceptionStorageThrows => new global::EngConnect.Application.UseCases.AwsS3Storage.DownloadFile.DownloadFileQuery
            {
                FileName = "broken-file.pdf"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
