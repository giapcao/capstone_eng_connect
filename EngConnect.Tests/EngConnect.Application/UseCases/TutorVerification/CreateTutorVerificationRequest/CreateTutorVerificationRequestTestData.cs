using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest;

internal enum CreateTutorVerificationRequestCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidTutorMissing,
    InvalidHasPendingExisting,
    InvalidProfileIncomplete,
    ExceptionPath,
}

internal static class CreateTutorVerificationRequestTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateTutorVerificationRequest",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/CreateTutorVerificationRequest/CreateTutorVerificationRequestCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/CreateTutorVerificationRequest/CreateTutorVerificationRequestCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/CreateTutorVerificationRequest/CreateTutorVerificationRequestCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateTutorVerificationRequestCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateTutorVerificationRequestCase.BoundaryDefault),
        BuildCase(CreateTutorVerificationRequestCase.InvalidRequestShape),
        BuildCase(CreateTutorVerificationRequestCase.InvalidTutorMissing),
        BuildCase(CreateTutorVerificationRequestCase.InvalidHasPendingExisting),
        BuildCase(CreateTutorVerificationRequestCase.InvalidProfileIncomplete),
        BuildCase(CreateTutorVerificationRequestCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateTutorVerificationRequestCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateTutorVerificationRequestCase.InvalidRequestShape),
        BuildCase(CreateTutorVerificationRequestCase.InvalidTutorMissing),
        BuildCase(CreateTutorVerificationRequestCase.InvalidHasPendingExisting),
        BuildCase(CreateTutorVerificationRequestCase.InvalidProfileIncomplete)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateTutorVerificationRequestCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(CreateTutorVerificationRequestCase caseId)
    {
        return caseId switch
        {
            CreateTutorVerificationRequestCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                monthExperience: 36),
            CreateTutorVerificationRequestCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                monthExperience: 1),
            CreateTutorVerificationRequestCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            CreateTutorVerificationRequestCase.InvalidTutorMissing => CreateTutorMissingCase(),
            CreateTutorVerificationRequestCase.InvalidHasPendingExisting => CreatePendingRequestCase(),
            CreateTutorVerificationRequestCase.InvalidProfileIncomplete => CreateIncompleteProfileCase(),
            CreateTutorVerificationRequestCase.ExceptionPath => CreateCase(
                "exception-path",
                UseCaseCaseKind.Exception,
                UseCaseHandlerExpectation.Exception,
                UseCaseValidatorExpectation.Skip,
                CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand CreateRequest(CreateTutorVerificationRequestCase caseId)
    {
        return caseId switch
        {
            CreateTutorVerificationRequestCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequest { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111") }),
            CreateTutorVerificationRequestCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequest { TutorId = Guid.Parse("22222222-2222-2222-2222-222222222222") }),
            CreateTutorVerificationRequestCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequest { TutorId = Guid.Empty }),
            CreateTutorVerificationRequestCase.InvalidTutorMissing => new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequest { TutorId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") }),
            CreateTutorVerificationRequestCase.InvalidHasPendingExisting => new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequest { TutorId = Guid.Parse("33333333-3333-3333-3333-333333333333") }),
            CreateTutorVerificationRequestCase.InvalidProfileIncomplete => new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequest { TutorId = Guid.Parse("44444444-4444-4444-4444-444444444444") }),
            CreateTutorVerificationRequestCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequest { TutorId = Guid.Parse("55555555-5555-5555-5555-555555555555") }),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand request,
        int monthExperience)
    {
        TutorVerificationRequest? createdRequest = null;
        var tutor = new Tutor
        {
            Id = request.Request.TutorId,
            Bio = "Experienced tutor",
            CvUrl = "https://example.com/cv.pdf",
            IntroVideoUrl = "https://example.com/intro.mp4",
            Headline = "IELTS Mentor",
            MonthExperience = monthExperience
        };

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
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                        var requestRepositoryMock = new Mock<IGenericRepository<TutorVerificationRequest, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                            .Returns(tutorRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<TutorVerificationRequest, Guid>())
                            .Returns(requestRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ReturnsAsync(1);

                        tutorRepositoryMock
                            .Setup(repository => repository.FindFirstAsync(
                                It.IsAny<Expression<Func<Tutor, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Tutor, object>>[]>()))
                            .ReturnsAsync(tutor);

                        requestRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<TutorVerificationRequest, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<TutorVerificationRequest, object>>[]>()))
                            .ReturnsAsync(false);
                        requestRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<TutorVerificationRequest>()))
                            .Callback<TutorVerificationRequest>(entity => createdRequest = entity);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<Guid>>(resultObject);
                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(createdRequest);
                        Assert.Equal(request.Request.TutorId, createdRequest!.TutorId);
                        Assert.Equal("Pending", createdRequest.Status);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateTutorMissingCase()
    {
        var request = CreateRequest(CreateTutorVerificationRequestCase.InvalidTutorMissing);

        return CreateFailureCase(
            "invalid-tutor-missing",
            request,
            mocks =>
            {
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                var requestRepositoryMock = new Mock<IGenericRepository<TutorVerificationRequest, Guid>>(MockBehavior.Strict);

                unitOfWorkMock
                    .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                    .Returns(tutorRepositoryMock.Object);
                unitOfWorkMock
                    .Setup(unitOfWork => unitOfWork.GetRepository<TutorVerificationRequest, Guid>())
                    .Returns(requestRepositoryMock.Object);

                tutorRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Tutor, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Tutor, object>>[]>()))
                    .ReturnsAsync((Tutor?)null);
            },
            expectedErrorCode: "Tutor.NotFound");
    }

    private static UseCaseCaseSet CreatePendingRequestCase()
    {
        var request = CreateRequest(CreateTutorVerificationRequestCase.InvalidHasPendingExisting);
        var tutor = new Tutor
        {
            Id = request.Request.TutorId,
            Bio = "Experienced tutor",
            CvUrl = "https://example.com/cv.pdf",
            IntroVideoUrl = "https://example.com/intro.mp4",
            Headline = "IELTS Mentor",
            MonthExperience = 36
        };

        return CreateFailureCase(
            "invalid-hasPending-existing",
            request,
            mocks =>
            {
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                var requestRepositoryMock = new Mock<IGenericRepository<TutorVerificationRequest, Guid>>(MockBehavior.Strict);

                unitOfWorkMock
                    .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                    .Returns(tutorRepositoryMock.Object);
                unitOfWorkMock
                    .Setup(unitOfWork => unitOfWork.GetRepository<TutorVerificationRequest, Guid>())
                    .Returns(requestRepositoryMock.Object);

                tutorRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Tutor, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Tutor, object>>[]>()))
                    .ReturnsAsync(tutor);

                requestRepositoryMock
                    .Setup(repository => repository.AnyAsync(
                        It.IsAny<Expression<Func<TutorVerificationRequest, bool>>>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<TutorVerificationRequest, object>>[]>()))
                    .ReturnsAsync(true);
            },
            expectedErrorCode: "Tutor.VerificationRequestAlreadyPending");
    }

    private static UseCaseCaseSet CreateIncompleteProfileCase()
    {
        var request = CreateRequest(CreateTutorVerificationRequestCase.InvalidProfileIncomplete);
        var tutor = new Tutor
        {
            Id = request.Request.TutorId,
            Bio = "Experienced tutor",
            CvUrl = null,
            IntroVideoUrl = "https://example.com/intro.mp4",
            Headline = "IELTS Mentor",
            MonthExperience = 36
        };

        return CreateFailureCase(
            "invalid-profile-incomplete",
            request,
            mocks =>
            {
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                var requestRepositoryMock = new Mock<IGenericRepository<TutorVerificationRequest, Guid>>(MockBehavior.Strict);

                unitOfWorkMock
                    .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                    .Returns(tutorRepositoryMock.Object);
                unitOfWorkMock
                    .Setup(unitOfWork => unitOfWork.GetRepository<TutorVerificationRequest, Guid>())
                    .Returns(requestRepositoryMock.Object);

                tutorRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Tutor, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Tutor, object>>[]>()))
                    .ReturnsAsync(tutor);

                requestRepositoryMock
                    .Setup(repository => repository.AnyAsync(
                        It.IsAny<Expression<Func<TutorVerificationRequest, bool>>>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<TutorVerificationRequest, object>>[]>()))
                    .ReturnsAsync(false);
            },
            expectedErrorCode: "Tutor.ProfileIncomplete");
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest.CreateTutorVerificationRequestCommand request,
        Action<UseCaseMockContext> arrangeMocks,
        string expectedErrorCode)
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
                    ArrangeMocks = arrangeMocks,
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                        Assert.Equal(expectedErrorCode, result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }
}
