using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Tests.Common;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Domain.Constants;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor;

internal enum UpdateIntroVideoUrlTutorCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidTutorExistMissing,
    InvalidUpdateFileRejected,
    ExceptionPath,
}

internal static class UpdateIntroVideoUrlTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateIntroVideoUrlTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateIntroVideoUrlTutor/UpdateIntroVideoUrlTutorCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateIntroVideoUrlTutor/UpdateIntroVideoUrlTutorCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateIntroVideoUrlTutor/UpdateIntroVideoUrlTutorValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateIntroVideoUrlTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateIntroVideoUrlTutorCase.BoundaryDefault),
        BuildCase(UpdateIntroVideoUrlTutorCase.InvalidRequestShape),
        BuildCase(UpdateIntroVideoUrlTutorCase.InvalidTutorExistMissing),
        BuildCase(UpdateIntroVideoUrlTutorCase.InvalidUpdateFileRejected),
        BuildCase(UpdateIntroVideoUrlTutorCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateIntroVideoUrlTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateIntroVideoUrlTutorCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateIntroVideoUrlTutorCase.InvalidRequestShape),
        BuildCase(UpdateIntroVideoUrlTutorCase.InvalidTutorExistMissing),
        BuildCase(UpdateIntroVideoUrlTutorCase.InvalidUpdateFileRejected),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateIntroVideoUrlTutorCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateIntroVideoUrlTutorCase caseId)
    {
        return caseId switch
        {
            UpdateIntroVideoUrlTutorCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateIntroVideoUrlTutorCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateIntroVideoUrlTutorCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateIntroVideoUrlTutorCase.InvalidTutorExistMissing => CreateCase("invalid-tutorExist-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateIntroVideoUrlTutorCase.InvalidUpdateFileRejected => CreateFailureCase("invalid-update-file-rejected", CreateRequest(caseId)),
            UpdateIntroVideoUrlTutorCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommand request)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = UseCaseCaseKind.Invalid,
            HandlerExpectation = UseCaseHandlerExpectation.Failure,
            ValidatorExpectation = UseCaseValidatorExpectation.Pass,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = mocks =>
                    {
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var awsStorageMock = mocks.StrictMock<IAwsStorageService>();
                        var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                            .Returns(tutorRepositoryMock.Object);

                        tutorRepositoryMock
                            .Setup(repository => repository.FindByIdAsync(
                                request.Id,
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<Tutor, object>>[]>()))
                            .ReturnsAsync(new Tutor
                            {
                                Id = request.Id,
                                IntroVideoUrl = "intro/original-tutor.mp4",
                                Status = nameof(CommonStatus.Active)
                            });

                        awsStorageMock
                            .Setup(service => service.UpdateFileAsync(
                                request.File,
                                request.Id,
                                nameof(PrefixFile.IntroVideo),
                                It.IsAny<CancellationToken>()))
                            .ReturnsAsync((global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUploadResult?)null);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                        Assert.Equal("Validation.Failed", result.Error?.Code);
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

    private static global::EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommand CreateRequest(UpdateIntroVideoUrlTutorCase caseId)
    {
        return caseId switch
        {
            UpdateIntroVideoUrlTutorCase.ValidDefault => new global::EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "intro.mp4", ContentType = "video/mp4", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateIntroVideoUrlTutorCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "intro.mp4", ContentType = "video/mp4", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateIntroVideoUrlTutorCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "intro", ContentType = "video/mp4", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("00000000-0000-0000-0000-000000000000") },
            UpdateIntroVideoUrlTutorCase.InvalidTutorExistMissing => new global::EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "intro.mp4", ContentType = "video/mp4", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            UpdateIntroVideoUrlTutorCase.InvalidUpdateFileRejected => new global::EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "intro.mp4", ContentType = "video/mp4", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateIntroVideoUrlTutorCase.ExceptionPath => new global::EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor.UpdateIntroVideoUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "intro.mp4", ContentType = "video/mp4", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
