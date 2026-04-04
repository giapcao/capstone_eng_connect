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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor;

internal enum UpdateAvatarTutorCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidTutorExistMissing,
    InvalidUpdateFileRejected,
    ExceptionPath,
}

internal static class UpdateAvatarTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateAvatarTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateAvatarTutor/UpdateAvatarTutorCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateAvatarTutor/UpdateAvatarTutorCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateAvatarTutor/UpdateAvatarTutorValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateAvatarTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateAvatarTutorCase.BoundaryDefault),
        BuildCase(UpdateAvatarTutorCase.InvalidRequestShape),
        BuildCase(UpdateAvatarTutorCase.InvalidTutorExistMissing),
        BuildCase(UpdateAvatarTutorCase.InvalidUpdateFileRejected),
        BuildCase(UpdateAvatarTutorCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateAvatarTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateAvatarTutorCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateAvatarTutorCase.InvalidRequestShape),
        BuildCase(UpdateAvatarTutorCase.InvalidTutorExistMissing),
        BuildCase(UpdateAvatarTutorCase.InvalidUpdateFileRejected),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateAvatarTutorCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateAvatarTutorCase caseId)
    {
        return caseId switch
        {
            UpdateAvatarTutorCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateAvatarTutorCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateAvatarTutorCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateAvatarTutorCase.InvalidTutorExistMissing => CreateCase("invalid-tutorExist-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateAvatarTutorCase.InvalidUpdateFileRejected => CreateFailureCase("invalid-update-file-rejected", CreateRequest(caseId)),
            UpdateAvatarTutorCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommand request)
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
                                Avatar = "avatars/original-tutor.png",
                                Status = nameof(CommonStatus.Active)
                            });

                        awsStorageMock
                            .Setup(service => service.UpdateFileAsync(
                                request.File,
                                request.Id,
                                nameof(PrefixFile.TutorAvatar),
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

    private static global::EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommand CreateRequest(UpdateAvatarTutorCase caseId)
    {
        return caseId switch
        {
            UpdateAvatarTutorCase.ValidDefault => new global::EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateAvatarTutorCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateAvatarTutorCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar", ContentType = "image/png", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("00000000-0000-0000-0000-000000000000") },
            UpdateAvatarTutorCase.InvalidTutorExistMissing => new global::EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            UpdateAvatarTutorCase.InvalidUpdateFileRejected => new global::EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateAvatarTutorCase.ExceptionPath => new global::EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor.UpdateAvatarTutorCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
