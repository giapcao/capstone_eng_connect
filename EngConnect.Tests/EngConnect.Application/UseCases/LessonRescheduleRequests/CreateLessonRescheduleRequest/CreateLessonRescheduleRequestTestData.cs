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

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest;

internal enum CreateLessonRescheduleRequestCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidLessonMissing,
    InvalidStudentMissing,
    InvalidPreviousLessonBuffer,
    InvalidNextLessonBuffer,
    InvalidHasPendingExisting,
    ExceptionPath,
}

internal static class CreateLessonRescheduleRequestTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateLessonRescheduleRequest",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest.CreateLessonRescheduleRequestCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest.CreateLessonRescheduleRequestCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest.CreateLessonRescheduleRequestCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/CreateLessonRescheduleRequest/CreateLessonRescheduleRequestCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/CreateLessonRescheduleRequest/CreateLessonRescheduleRequestCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/CreateLessonRescheduleRequest/CreateLessonRescheduleRequestCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateLessonRescheduleRequestCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateLessonRescheduleRequestCase.BoundaryDefault),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidRequestShape),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidLessonMissing),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidStudentMissing),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidPreviousLessonBuffer),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidNextLessonBuffer),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidHasPendingExisting),
        BuildCase(CreateLessonRescheduleRequestCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateLessonRescheduleRequestCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateLessonRescheduleRequestCase.InvalidRequestShape),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidLessonMissing),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidStudentMissing),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidPreviousLessonBuffer),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidNextLessonBuffer),
        BuildCase(CreateLessonRescheduleRequestCase.InvalidHasPendingExisting)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateLessonRescheduleRequestCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(CreateLessonRescheduleRequestCase caseId)
    {
        return caseId switch
        {
            CreateLessonRescheduleRequestCase.ValidDefault => CreateSuccessCase("valid-default", UseCaseCaseKind.Valid, CreateRequest(caseId)),
            CreateLessonRescheduleRequestCase.BoundaryDefault => CreateSuccessCase("boundary-default", UseCaseCaseKind.Boundary, CreateRequest(caseId)),
            CreateLessonRescheduleRequestCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            CreateLessonRescheduleRequestCase.InvalidLessonMissing => CreateLessonMissingCase(),
            CreateLessonRescheduleRequestCase.InvalidStudentMissing => CreateStudentMissingCase(),
            CreateLessonRescheduleRequestCase.InvalidPreviousLessonBuffer => CreatePreviousLessonBufferCase(),
            CreateLessonRescheduleRequestCase.InvalidNextLessonBuffer => CreateNextLessonBufferCase(),
            CreateLessonRescheduleRequestCase.InvalidHasPendingExisting => CreatePendingRequestCase(),
            CreateLessonRescheduleRequestCase.ExceptionPath => CreateCase(
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

    private static global::EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest.CreateLessonRescheduleRequestCommand CreateRequest(CreateLessonRescheduleRequestCase caseId)
    {
        return caseId switch
        {
            CreateLessonRescheduleRequestCase.ValidDefault => CreateCommand(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                new DateTime(2035, 4, 3, 15, 0, 0, DateTimeKind.Utc),
                new DateTime(2035, 4, 3, 16, 0, 0, DateTimeKind.Utc)),
            CreateLessonRescheduleRequestCase.BoundaryDefault => CreateCommand(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                new DateTime(2035, 4, 3, 17, 0, 0, DateTimeKind.Utc),
                new DateTime(2035, 4, 3, 18, 0, 0, DateTimeKind.Utc)),
            CreateLessonRescheduleRequestCase.InvalidRequestShape => CreateCommand(
                Guid.Empty,
                Guid.Empty,
                new DateTime(2035, 4, 3, 16, 0, 0, DateTimeKind.Utc),
                new DateTime(2035, 4, 3, 15, 0, 0, DateTimeKind.Utc),
                string.Empty),
            CreateLessonRescheduleRequestCase.InvalidLessonMissing => CreateCommand(
                Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                new DateTime(2035, 4, 3, 15, 0, 0, DateTimeKind.Utc),
                new DateTime(2035, 4, 3, 16, 0, 0, DateTimeKind.Utc)),
            CreateLessonRescheduleRequestCase.InvalidStudentMissing => CreateCommand(
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                new DateTime(2035, 4, 3, 15, 0, 0, DateTimeKind.Utc),
                new DateTime(2035, 4, 3, 16, 0, 0, DateTimeKind.Utc)),
            CreateLessonRescheduleRequestCase.InvalidPreviousLessonBuffer => CreateCommand(
                Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                new DateTime(2035, 4, 3, 15, 0, 0, DateTimeKind.Utc),
                new DateTime(2035, 4, 3, 16, 0, 0, DateTimeKind.Utc)),
            CreateLessonRescheduleRequestCase.InvalidNextLessonBuffer => CreateCommand(
                Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                new DateTime(2035, 4, 3, 15, 0, 0, DateTimeKind.Utc),
                new DateTime(2035, 4, 3, 16, 0, 0, DateTimeKind.Utc)),
            CreateLessonRescheduleRequestCase.InvalidHasPendingExisting => CreateCommand(
                Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Guid.Parse("ffffffff-1111-1111-1111-111111111111"),
                new DateTime(2035, 4, 3, 15, 0, 0, DateTimeKind.Utc),
                new DateTime(2035, 4, 3, 16, 0, 0, DateTimeKind.Utc)),
            CreateLessonRescheduleRequestCase.ExceptionPath => CreateCommand(
                Guid.Parse("77777777-7777-7777-7777-777777777777"),
                Guid.Parse("99999999-9999-9999-9999-999999999999"),
                new DateTime(2035, 4, 3, 15, 0, 0, DateTimeKind.Utc),
                new DateTime(2035, 4, 3, 16, 0, 0, DateTimeKind.Utc)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static global::EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest.CreateLessonRescheduleRequestCommand CreateCommand(
        Guid lessonId,
        Guid studentId,
        DateTime proposedStart,
        DateTime proposedEnd,
        string tutorNote = "SampleValue")
    {
        return new global::EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest.CreateLessonRescheduleRequestCommand(
            new global::EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest.CreateLessonRescheduleRequest
            {
                LessonId = lessonId,
                StudentId = studentId,
                ProposedStartTime = proposedStart,
                ProposedEndTime = proposedEnd,
                TutorNote = tutorNote
            });
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest.CreateLessonRescheduleRequestCommand request)
    {
        LessonRescheduleRequest? createdRequest = null;

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
                        var lessonRepositoryMock = new Mock<IGenericRepository<Lesson, Guid>>(MockBehavior.Strict);
                        var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                        var requestRepositoryMock = new Mock<IGenericRepository<LessonRescheduleRequest, Guid>>(MockBehavior.Strict);

                        SetupLessonRescheduleDependencies(
                            unitOfWorkMock,
                            lessonRepositoryMock,
                            studentRepositoryMock,
                            requestRepositoryMock);

                        lessonRepositoryMock
                            .Setup(repository => repository.FindFirstAsync(
                                It.IsAny<Expression<Func<Lesson, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Lesson, object>>[]>()))
                            .ReturnsAsync(CreateLesson(request.Request.LessonId, request.Request.ProposedStartTime.AddHours(-3), request.Request.ProposedStartTime.AddHours(-2)));

                        studentRepositoryMock
                            .Setup(repository => repository.FindFirstAsync(
                                It.IsAny<Expression<Func<Student, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Student, object>>[]>()))
                            .ReturnsAsync(new Student { Id = request.Request.StudentId });

                        lessonRepositoryMock
                            .Setup(repository => repository.FindAll(
                                It.IsAny<Expression<Func<Lesson, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Lesson, object>>[]>()))
                            .Returns(new TestAsyncEnumerable<Lesson>([]));

                        requestRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<LessonRescheduleRequest, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<LessonRescheduleRequest, object>>[]>()))
                            .ReturnsAsync(false);
                        requestRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<LessonRescheduleRequest>()))
                            .Callback<LessonRescheduleRequest>(entity => createdRequest = entity);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ReturnsAsync(1);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<Guid>>(resultObject);
                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(createdRequest);
                        Assert.Equal("Pending", createdRequest!.Status);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateLessonMissingCase()
    {
        var request = CreateRequest(CreateLessonRescheduleRequestCase.InvalidLessonMissing);

        return CreateFailureCase(
            "invalid-lesson-missing",
            request,
            mocks =>
            {
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var lessonRepositoryMock = new Mock<IGenericRepository<Lesson, Guid>>(MockBehavior.Strict);
                var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                var requestRepositoryMock = new Mock<IGenericRepository<LessonRescheduleRequest, Guid>>(MockBehavior.Strict);

                SetupLessonRescheduleDependencies(unitOfWorkMock, lessonRepositoryMock, studentRepositoryMock, requestRepositoryMock);

                lessonRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Lesson, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()))
                    .ReturnsAsync((Lesson?)null);
            },
            expectedStatusCode: HttpStatusCode.NotFound,
            expectedErrorCode: "Schedule.Lesson.NotFound");
    }

    private static UseCaseCaseSet CreateStudentMissingCase()
    {
        var request = CreateRequest(CreateLessonRescheduleRequestCase.InvalidStudentMissing);

        return CreateFailureCase(
            "invalid-student-missing",
            request,
            mocks =>
            {
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var lessonRepositoryMock = new Mock<IGenericRepository<Lesson, Guid>>(MockBehavior.Strict);
                var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                var requestRepositoryMock = new Mock<IGenericRepository<LessonRescheduleRequest, Guid>>(MockBehavior.Strict);

                SetupLessonRescheduleDependencies(unitOfWorkMock, lessonRepositoryMock, studentRepositoryMock, requestRepositoryMock);

                lessonRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Lesson, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()))
                    .ReturnsAsync(CreateLesson(request.Request.LessonId, request.Request.ProposedStartTime.AddHours(-3), request.Request.ProposedStartTime.AddHours(-2)));

                studentRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Student, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Student, object>>[]>()))
                    .ReturnsAsync((Student?)null);
            },
            expectedStatusCode: HttpStatusCode.NotFound,
            expectedErrorCode: "Schedule.Student.NotFound");
    }

    private static UseCaseCaseSet CreatePreviousLessonBufferCase()
    {
        var request = CreateRequest(CreateLessonRescheduleRequestCase.InvalidPreviousLessonBuffer);
        var currentLesson = CreateLesson(request.Request.LessonId, request.Request.ProposedStartTime.AddHours(-4), request.Request.ProposedStartTime.AddHours(-3));
        var previousLesson = CreateLesson(Guid.NewGuid(), request.Request.ProposedStartTime.AddHours(-90.0 / 60.0), request.Request.ProposedStartTime.AddMinutes(-30));

        return CreateFailureCase(
            "invalid-previous-lesson-buffer",
            request,
            mocks =>
            {
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var lessonRepositoryMock = new Mock<IGenericRepository<Lesson, Guid>>(MockBehavior.Strict);
                var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                var requestRepositoryMock = new Mock<IGenericRepository<LessonRescheduleRequest, Guid>>(MockBehavior.Strict);

                SetupLessonRescheduleDependencies(unitOfWorkMock, lessonRepositoryMock, studentRepositoryMock, requestRepositoryMock);

                lessonRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Lesson, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()))
                    .ReturnsAsync(currentLesson);

                studentRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Student, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Student, object>>[]>()))
                    .ReturnsAsync(new Student { Id = request.Request.StudentId });

                lessonRepositoryMock
                    .SetupSequence(repository => repository.FindAll(
                        It.IsAny<Expression<Func<Lesson, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()))
                    .Returns(new TestAsyncEnumerable<Lesson>([previousLesson]))
                    .Returns(new TestAsyncEnumerable<Lesson>([]));
            },
            expectedErrorCode: "Schedule.Reschedule.OneHourBufferRequired");
    }

    private static UseCaseCaseSet CreateNextLessonBufferCase()
    {
        var request = CreateRequest(CreateLessonRescheduleRequestCase.InvalidNextLessonBuffer);
        var currentLesson = CreateLesson(request.Request.LessonId, request.Request.ProposedStartTime.AddHours(-4), request.Request.ProposedStartTime.AddHours(-3));
        var nextLesson = CreateLesson(Guid.NewGuid(), request.Request.ProposedEndTime.AddMinutes(30), request.Request.ProposedEndTime.AddHours(1.5));

        return CreateFailureCase(
            "invalid-next-lesson-buffer",
            request,
            mocks =>
            {
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var lessonRepositoryMock = new Mock<IGenericRepository<Lesson, Guid>>(MockBehavior.Strict);
                var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                var requestRepositoryMock = new Mock<IGenericRepository<LessonRescheduleRequest, Guid>>(MockBehavior.Strict);

                SetupLessonRescheduleDependencies(unitOfWorkMock, lessonRepositoryMock, studentRepositoryMock, requestRepositoryMock);

                lessonRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Lesson, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()))
                    .ReturnsAsync(currentLesson);

                studentRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Student, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Student, object>>[]>()))
                    .ReturnsAsync(new Student { Id = request.Request.StudentId });

                lessonRepositoryMock
                    .SetupSequence(repository => repository.FindAll(
                        It.IsAny<Expression<Func<Lesson, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()))
                    .Returns(new TestAsyncEnumerable<Lesson>([]))
                    .Returns(new TestAsyncEnumerable<Lesson>([nextLesson]));
            },
            expectedErrorCode: "Schedule.Reschedule.OneHourBufferRequired");
    }

    private static UseCaseCaseSet CreatePendingRequestCase()
    {
        var request = CreateRequest(CreateLessonRescheduleRequestCase.InvalidHasPendingExisting);

        return CreateFailureCase(
            "invalid-hasPending-existing",
            request,
            mocks =>
            {
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var lessonRepositoryMock = new Mock<IGenericRepository<Lesson, Guid>>(MockBehavior.Strict);
                var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                var requestRepositoryMock = new Mock<IGenericRepository<LessonRescheduleRequest, Guid>>(MockBehavior.Strict);

                SetupLessonRescheduleDependencies(unitOfWorkMock, lessonRepositoryMock, studentRepositoryMock, requestRepositoryMock);

                lessonRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Lesson, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()))
                    .ReturnsAsync(CreateLesson(request.Request.LessonId, request.Request.ProposedStartTime.AddHours(-4), request.Request.ProposedStartTime.AddHours(-3)));

                studentRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<Student, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Student, object>>[]>()))
                    .ReturnsAsync(new Student { Id = request.Request.StudentId });

                lessonRepositoryMock
                    .Setup(repository => repository.FindAll(
                        It.IsAny<Expression<Func<Lesson, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()))
                    .Returns(new TestAsyncEnumerable<Lesson>([]));

                requestRepositoryMock
                    .Setup(repository => repository.AnyAsync(
                        It.IsAny<Expression<Func<LessonRescheduleRequest, bool>>>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<LessonRescheduleRequest, object>>[]>()))
                    .ReturnsAsync(true);
            },
            expectedErrorCode: "Schedule.Reschedule.PendingAlreadyExists");
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest.CreateLessonRescheduleRequestCommand request,
        Action<UseCaseMockContext> arrangeMocks,
        string expectedErrorCode,
        HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest)
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
                        Assert.Equal(expectedStatusCode, result.HttpStatusCode);
                        Assert.Equal(expectedErrorCode, result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static void SetupLessonRescheduleDependencies(
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IGenericRepository<Lesson, Guid>> lessonRepositoryMock,
        Mock<IGenericRepository<Student, Guid>> studentRepositoryMock,
        Mock<IGenericRepository<LessonRescheduleRequest, Guid>> requestRepositoryMock)
    {
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Lesson, Guid>())
            .Returns(lessonRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
            .Returns(studentRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<LessonRescheduleRequest, Guid>())
            .Returns(requestRepositoryMock.Object);
    }

    private static Lesson CreateLesson(Guid lessonId, DateTime startTime, DateTime endTime)
    {
        return new Lesson
        {
            Id = lessonId,
            TutorId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            StartTime = startTime,
            EndTime = endTime
        };
    }
}
