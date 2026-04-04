using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive;

internal enum DeleteFileFromDriveCase
{
    ValidExistingDriveFile,
    BoundaryShortFileId,
    InvalidRequestShape,
    InvalidDeleteRejected,
    ExceptionDriveThrows,
}

internal static class DeleteFileFromDriveTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteFileFromDrive",
        RequestTypeFullName = "EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/DeleteFileFromDrive/DeleteFileFromDriveCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/DeleteFileFromDrive/DeleteFileFromDriveCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/FileStorage/DeleteFileFromDrive/DeleteFileFromDriveCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteFileFromDriveCase.ValidExistingDriveFile)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteFileFromDriveCase.BoundaryShortFileId),
        BuildCase(DeleteFileFromDriveCase.InvalidRequestShape),
        BuildCase(DeleteFileFromDriveCase.InvalidDeleteRejected),
        BuildCase(DeleteFileFromDriveCase.ExceptionDriveThrows)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteFileFromDriveCase.BoundaryShortFileId)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteFileFromDriveCase.InvalidRequestShape),
        BuildCase(DeleteFileFromDriveCase.InvalidDeleteRejected)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteFileFromDriveCase.ExceptionDriveThrows)
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

    private static UseCaseCaseSet BuildCase(DeleteFileFromDriveCase caseId)
    {
        return caseId switch
        {
            DeleteFileFromDriveCase.ValidExistingDriveFile => CreateSuccessCase(
                "valid-existing-drive-file",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId)),
            DeleteFileFromDriveCase.BoundaryShortFileId => CreateSuccessCase(
                "boundary-short-file-id",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId)),
            DeleteFileFromDriveCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            DeleteFileFromDriveCase.InvalidDeleteRejected => CreateFailureCase(
                "invalid-delete-rejected",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var driveServiceMock = mocks.StrictMock<IDriveService>();
                    driveServiceMock
                        .Setup(service => service.DeleteFileAsync("missing-drive-file-id", It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);
                },
                expectedStatusCode: HttpStatusCode.BadRequest),
            DeleteFileFromDriveCase.ExceptionDriveThrows => CreateFailureCase(
                "exception-drive-throws",
                UseCaseCaseKind.Exception,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var driveServiceMock = mocks.StrictMock<IDriveService>();
                    driveServiceMock
                        .Setup(service => service.DeleteFileAsync("throw-drive-file-id", It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new InvalidOperationException("drive error"));
                },
                expectedStatusCode: HttpStatusCode.InternalServerError),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommand request)
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
                        var driveServiceMock = mocks.StrictMock<IDriveService>();
                        driveServiceMock
                            .Setup(service => service.DeleteFileAsync(request.FileId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(true);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommand request,
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

    private static global::EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommand CreateRequest(DeleteFileFromDriveCase caseId)
    {
        return caseId switch
        {
            DeleteFileFromDriveCase.ValidExistingDriveFile => new global::EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommand
            {
                FileId = "drive-file-001"
            },
            DeleteFileFromDriveCase.BoundaryShortFileId => new global::EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommand
            {
                FileId = "a"
            },
            DeleteFileFromDriveCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommand
            {
                FileId = string.Empty
            },
            DeleteFileFromDriveCase.InvalidDeleteRejected => new global::EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommand
            {
                FileId = "missing-drive-file-id"
            },
            DeleteFileFromDriveCase.ExceptionDriveThrows => new global::EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive.DeleteFileFromDriveCommand
            {
                FileId = "throw-drive-file-id"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
