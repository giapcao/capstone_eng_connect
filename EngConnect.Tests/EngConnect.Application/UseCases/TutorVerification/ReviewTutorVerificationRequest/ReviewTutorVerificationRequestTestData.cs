using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest;

internal enum ReviewTutorVerificationRequestCase
{
    ValidApproveWithTutorUser,
    BoundaryRejectWithTutorUserMissing,
    ValidApproveWithTutorMissing,
    InvalidRequestShape,
    InvalidRequestMissing,
    InvalidAlreadyReviewed,
    ExceptionPath,
}

internal static class ReviewTutorVerificationRequestTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "ReviewTutorVerificationRequest",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/ReviewTutorVerificationRequest/ReviewTutorVerificationRequestCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/ReviewTutorVerificationRequest/ReviewTutorVerificationRequestCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/ReviewTutorVerificationRequest/ReviewTutorVerificationRequestCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(ReviewTutorVerificationRequestCase.ValidApproveWithTutorUser)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(ReviewTutorVerificationRequestCase.BoundaryRejectWithTutorUserMissing),
        BuildCase(ReviewTutorVerificationRequestCase.ValidApproveWithTutorMissing),
        BuildCase(ReviewTutorVerificationRequestCase.InvalidRequestShape),
        BuildCase(ReviewTutorVerificationRequestCase.InvalidRequestMissing),
        BuildCase(ReviewTutorVerificationRequestCase.InvalidAlreadyReviewed),
        BuildCase(ReviewTutorVerificationRequestCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(ReviewTutorVerificationRequestCase.BoundaryRejectWithTutorUserMissing),
        BuildCase(ReviewTutorVerificationRequestCase.ValidApproveWithTutorMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(ReviewTutorVerificationRequestCase.InvalidRequestShape),
        BuildCase(ReviewTutorVerificationRequestCase.InvalidRequestMissing),
        BuildCase(ReviewTutorVerificationRequestCase.InvalidAlreadyReviewed)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(ReviewTutorVerificationRequestCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(ReviewTutorVerificationRequestCase caseId)
    {
        return caseId switch
        {
            ReviewTutorVerificationRequestCase.ValidApproveWithTutorUser => CreateSuccessCase(
                "valid-approve-with-tutor-user",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                CreatePendingRequest(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")),
                new Tutor
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    VerifiedStatus = nameof(TutorVerifiedStatus.Unverified)
                },
                new User
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Email = "approved.tutor@example.com",
                    FirstName = "Approved",
                    LastName = "Tutor"
                },
                expectOutbox: true,
                expectedRequestStatus: nameof(TutorVerificationRequestStatus.Approved),
                expectedTutorStatus: nameof(TutorVerifiedStatus.Verified),
                expectedRejectionReason: null),
            ReviewTutorVerificationRequestCase.BoundaryRejectWithTutorUserMissing => CreateSuccessCase(
                "boundary-reject-with-tutor-user-missing",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                CreatePendingRequest(Guid.Parse("22222222-2222-2222-2222-222222222222"), Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")),
                new Tutor
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    UserId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    VerifiedStatus = nameof(TutorVerifiedStatus.Unverified)
                },
                tutorUser: null,
                expectOutbox: false,
                expectedRequestStatus: nameof(TutorVerificationRequestStatus.Rejected),
                expectedTutorStatus: nameof(TutorVerifiedStatus.Rejected),
                expectedRejectionReason: "Boundary rejection reason"),
            ReviewTutorVerificationRequestCase.ValidApproveWithTutorMissing => CreateSuccessCase(
                "valid-approve-with-tutor-missing",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                CreatePendingRequest(Guid.Parse("33333333-3333-3333-3333-333333333333"), Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee")),
                tutor: null,
                tutorUser: null,
                expectOutbox: false,
                expectedRequestStatus: nameof(TutorVerificationRequestStatus.Approved),
                expectedTutorStatus: null,
                expectedRejectionReason: null),
            ReviewTutorVerificationRequestCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            ReviewTutorVerificationRequestCase.InvalidRequestMissing => CreateFailureCase(
                "invalid-request-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var requestRepositoryMock = new Mock<IGenericRepository<TutorVerificationRequest, Guid>>(MockBehavior.Strict);
                    var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    var outboxRepositoryMock = new Mock<IGenericRepository<OutboxEvent, Guid>>(MockBehavior.Strict);

                    SetupRepositories(unitOfWorkMock, requestRepositoryMock, tutorRepositoryMock, userRepositoryMock, outboxRepositoryMock);

                    requestRepositoryMock
                        .Setup(repository => repository.FindFirstAsync(
                            It.IsAny<Expression<Func<TutorVerificationRequest, bool>>>(),
                            It.IsAny<bool>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<TutorVerificationRequest, object>>[]>()))
                        .ReturnsAsync((TutorVerificationRequest?)null);
                },
                expectedStatusCode: HttpStatusCode.NotFound,
                expectedErrorCode: "Tutor.VerificationRequest.NotFound"),
            ReviewTutorVerificationRequestCase.InvalidAlreadyReviewed => CreateFailureCase(
                "invalid-already-reviewed",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var requestRepositoryMock = new Mock<IGenericRepository<TutorVerificationRequest, Guid>>(MockBehavior.Strict);
                    var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    var outboxRepositoryMock = new Mock<IGenericRepository<OutboxEvent, Guid>>(MockBehavior.Strict);

                    SetupRepositories(unitOfWorkMock, requestRepositoryMock, tutorRepositoryMock, userRepositoryMock, outboxRepositoryMock);

                    requestRepositoryMock
                        .Setup(repository => repository.FindFirstAsync(
                            It.IsAny<Expression<Func<TutorVerificationRequest, bool>>>(),
                            It.IsAny<bool>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<TutorVerificationRequest, object>>[]>()))
                        .ReturnsAsync(new TutorVerificationRequest
                        {
                            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                            TutorId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                            Status = nameof(TutorVerificationRequestStatus.Approved)
                        });
                },
                expectedStatusCode: HttpStatusCode.BadRequest,
                expectedErrorCode: "Tutor.VerificationRequest.AlreadyReviewed"),
            ReviewTutorVerificationRequestCase.ExceptionPath => CreateFailureCase(
                "exception-savechanges-throws",
                UseCaseCaseKind.Exception,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var requestRepositoryMock = new Mock<IGenericRepository<TutorVerificationRequest, Guid>>(MockBehavior.Strict);
                    var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    var outboxRepositoryMock = new Mock<IGenericRepository<OutboxEvent, Guid>>(MockBehavior.Strict);
                    var request = CreatePendingRequest(Guid.Parse("55555555-5555-5555-5555-555555555555"), Guid.Parse("11111111-2222-3333-4444-555555555555"));

                    SetupRepositories(unitOfWorkMock, requestRepositoryMock, tutorRepositoryMock, userRepositoryMock, outboxRepositoryMock);

                    requestRepositoryMock
                        .Setup(repository => repository.FindFirstAsync(
                            It.IsAny<Expression<Func<TutorVerificationRequest, bool>>>(),
                            It.IsAny<bool>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<TutorVerificationRequest, object>>[]>()))
                        .ReturnsAsync(request);

                    tutorRepositoryMock
                        .Setup(repository => repository.FindFirstAsync(
                            It.IsAny<Expression<Func<Tutor, bool>>>(),
                            It.IsAny<bool>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<Tutor, object>>[]>()))
                        .ReturnsAsync((Tutor?)null);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                        .ThrowsAsync(new InvalidOperationException("save failed"));
                },
                expectedStatusCode: HttpStatusCode.InternalServerError,
                expectedErrorCode: "Server.Error"),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand request,
        TutorVerificationRequest requestEntity,
        Tutor? tutor,
        User? tutorUser,
        bool expectOutbox,
        string expectedRequestStatus,
        string? expectedTutorStatus,
        string? expectedRejectionReason)
    {
        OutboxEvent? addedOutboxEvent = null;

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
                        var requestRepositoryMock = new Mock<IGenericRepository<TutorVerificationRequest, Guid>>(MockBehavior.Strict);
                        var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                        var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                        var outboxRepositoryMock = new Mock<IGenericRepository<OutboxEvent, Guid>>(MockBehavior.Strict);

                        SetupRepositories(unitOfWorkMock, requestRepositoryMock, tutorRepositoryMock, userRepositoryMock, outboxRepositoryMock);

                        requestRepositoryMock
                            .Setup(repository => repository.FindFirstAsync(
                                It.IsAny<Expression<Func<TutorVerificationRequest, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<TutorVerificationRequest, object>>[]>()))
                            .ReturnsAsync(requestEntity);

                        tutorRepositoryMock
                            .Setup(repository => repository.FindFirstAsync(
                                It.IsAny<Expression<Func<Tutor, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Tutor, object>>[]>()))
                            .ReturnsAsync(tutor);

                        if (tutor is not null)
                        {
                            userRepositoryMock
                                .Setup(repository => repository.FindByIdAsync(
                                    tutor.UserId,
                                    false,
                                    It.IsAny<CancellationToken>(),
                                    It.IsAny<Expression<Func<User, object>>[]>()))
                                .ReturnsAsync(tutorUser);
                        }

                        outboxRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<OutboxEvent>()))
                            .Callback<OutboxEvent>(entity => addedOutboxEvent = entity);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ReturnsAsync(1);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<Guid>>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.Equal(requestEntity.Id, result.Data);
                        Assert.Equal(expectedRequestStatus, requestEntity.Status);
                        Assert.Equal(request.Request.AdminUserId, requestEntity.ReviewedBy);
                        Assert.Equal(expectedRejectionReason, requestEntity.RejectionReason);
                        Assert.NotNull(requestEntity.ReviewedAt);

                        if (tutor is not null)
                        {
                            Assert.Equal(expectedTutorStatus, tutor.VerifiedStatus);
                        }

                        if (expectOutbox)
                        {
                            Assert.NotNull(addedOutboxEvent);
                        }
                        else
                        {
                            Assert.Null(addedOutboxEvent);
                        }

                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand request,
        Action<UseCaseMockContext> arrangeMocks,
        HttpStatusCode expectedStatusCode,
        string expectedErrorCode)
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
                        Assert.Equal(expectedErrorCode, result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static void SetupRepositories(
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IGenericRepository<TutorVerificationRequest, Guid>> requestRepositoryMock,
        Mock<IGenericRepository<Tutor, Guid>> tutorRepositoryMock,
        Mock<IGenericRepository<User, Guid>> userRepositoryMock,
        Mock<IGenericRepository<OutboxEvent, Guid>> outboxRepositoryMock)
    {
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<TutorVerificationRequest, Guid>())
            .Returns(requestRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
            .Returns(tutorRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
            .Returns(userRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<OutboxEvent, Guid>())
            .Returns(outboxRepositoryMock.Object);
    }

    private static TutorVerificationRequest CreatePendingRequest(Guid requestId, Guid tutorId)
    {
        return new TutorVerificationRequest
        {
            Id = requestId,
            TutorId = tutorId,
            Status = nameof(TutorVerificationRequestStatus.Pending)
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

    private static global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand CreateRequest(
        ReviewTutorVerificationRequestCase caseId)
    {
        return caseId switch
        {
            ReviewTutorVerificationRequestCase.ValidApproveWithTutorUser => new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequest
                {
                    RequestId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    AdminUserId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Approved = true,
                    RejectionReason = "Should be ignored"
                }),
            ReviewTutorVerificationRequestCase.BoundaryRejectWithTutorUserMissing => new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequest
                {
                    RequestId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    AdminUserId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Approved = false,
                    RejectionReason = "Boundary rejection reason"
                }),
            ReviewTutorVerificationRequestCase.ValidApproveWithTutorMissing => new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequest
                {
                    RequestId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    AdminUserId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Approved = true,
                    RejectionReason = "No tutor branch"
                }),
            ReviewTutorVerificationRequestCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequest
                {
                    RequestId = Guid.Empty,
                    AdminUserId = null,
                    Approved = false,
                    RejectionReason = string.Empty
                }),
            ReviewTutorVerificationRequestCase.InvalidRequestMissing => new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequest
                {
                    RequestId = Guid.Parse("aaaaaaaa-1111-2222-3333-444444444444"),
                    AdminUserId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Approved = false,
                    RejectionReason = "Missing request"
                }),
            ReviewTutorVerificationRequestCase.InvalidAlreadyReviewed => new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequest
                {
                    RequestId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    AdminUserId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Approved = false,
                    RejectionReason = "Already reviewed"
                }),
            ReviewTutorVerificationRequestCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequestCommand(
                new global::EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest.ReviewTutorVerificationRequest
                {
                    RequestId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    AdminUserId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Approved = true,
                    RejectionReason = "Save changes throws"
                }),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
