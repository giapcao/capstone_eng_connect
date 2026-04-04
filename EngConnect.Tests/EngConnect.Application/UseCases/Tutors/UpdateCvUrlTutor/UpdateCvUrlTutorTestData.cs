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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor;

internal enum UpdateCvUrlTutorCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidTutorExistMissing,
    InvalidUpdateFileRejected,
    ExceptionPath,
}

internal static class UpdateCvUrlTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateCvUrlTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateCvUrlTutor/UpdateCvUrlTutorCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateCvUrlTutor/UpdateCvUrlTutorCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateCvUrlTutor/UpdateCvUrlTutorValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateCvUrlTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateCvUrlTutorCase.BoundaryDefault),
        BuildCase(UpdateCvUrlTutorCase.InvalidRequestShape),
        BuildCase(UpdateCvUrlTutorCase.InvalidTutorExistMissing),
        BuildCase(UpdateCvUrlTutorCase.InvalidUpdateFileRejected),
        BuildCase(UpdateCvUrlTutorCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateCvUrlTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateCvUrlTutorCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateCvUrlTutorCase.InvalidRequestShape),
        BuildCase(UpdateCvUrlTutorCase.InvalidTutorExistMissing),
        BuildCase(UpdateCvUrlTutorCase.InvalidUpdateFileRejected),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateCvUrlTutorCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateCvUrlTutorCase caseId)
    {
        return caseId switch
        {
            UpdateCvUrlTutorCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateCvUrlTutorCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateCvUrlTutorCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateCvUrlTutorCase.InvalidTutorExistMissing => CreateCase("invalid-tutorExist-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateCvUrlTutorCase.InvalidUpdateFileRejected => CreateFailureCase("invalid-update-file-rejected", CreateRequest(caseId)),
            UpdateCvUrlTutorCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommand request)
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
                                CvUrl = "cv/original-tutor.pdf",
                                Status = nameof(CommonStatus.Active)
                            });

                        awsStorageMock
                            .Setup(service => service.UpdateFileAsync(
                                request.File,
                                request.Id,
                                nameof(PrefixFile.CV),
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

    private static global::EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommand CreateRequest(UpdateCvUrlTutorCase caseId)
    {
        return caseId switch
        {
            UpdateCvUrlTutorCase.ValidDefault => new global::EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume.pdf", ContentType = "application/pdf", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateCvUrlTutorCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume.pdf", ContentType = "application/pdf", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateCvUrlTutorCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume", ContentType = "application/pdf", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("00000000-0000-0000-0000-000000000000") },
            UpdateCvUrlTutorCase.InvalidTutorExistMissing => new global::EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume.pdf", ContentType = "application/pdf", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            UpdateCvUrlTutorCase.InvalidUpdateFileRejected => new global::EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume.pdf", ContentType = "application/pdf", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateCvUrlTutorCase.ExceptionPath => new global::EngConnect.Application.UseCases.Tutors.UpdateCvUrlTutor.UpdateCvUrlTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume.pdf", ContentType = "application/pdf", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
