using System.Linq.Expressions;
using System.Net;
using System.Text.Json;
using EngConnect.Application.UseCases.AiSummerize.GetAiSummary;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.Domain.Persistence.Models;
using Moq;
using Xunit;

namespace EngConnect.Tests.Common;

internal static class UseCaseCustomCaseRegistry
{
    private const string GetAiSummaryRequestTypeFullName =
        "EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand";

    public static UseCaseCaseCatalog Apply(UseCaseDefinition definition, UseCaseCaseCatalog defaultCatalog)
    {
        return string.Equals(definition.RequestTypeFullName, GetAiSummaryRequestTypeFullName, StringComparison.Ordinal)
            ? CreateGetAiSummaryCatalog()
            : defaultCatalog;
    }

    private static UseCaseCaseCatalog CreateGetAiSummaryCatalog()
    {
        return new UseCaseCaseCatalog
        {
            ValidCases =
            [
                CreateGetAiSummarySuccessCase(
                    "valid-high-coverage",
                    UseCaseCaseKind.Valid,
                    CreateAnalysisResponse("High coverage summary", passCount: 7, failCount: 1),
                    expectedCoveragePercent: 87.5m,
                    expectWarningOutbox: false),
                CreateGetAiSummarySuccessCase(
                    "valid-low-coverage-warning",
                    UseCaseCaseKind.Valid,
                    CreateAnalysisResponse("Low coverage summary", passCount: 1, failCount: 2),
                    expectedCoveragePercent: 33.33m,
                    expectWarningOutbox: true)
            ],
            BoundaryCases =
            [
                CreateGetAiSummarySuccessCase(
                    "boundary-zero-total-outcomes",
                    UseCaseCaseKind.Boundary,
                    CreateAnalysisResponse("Empty detail summary", passCount: 0, failCount: 0),
                    expectedCoveragePercent: 0m,
                    expectWarningOutbox: true)
            ],
            InvalidCases =
            [
                CreateGetAiSummaryValidatorFailureCase(),
                CreateGetAiSummaryFailureCase(
                    "invalid-lesson-not-found",
                    HttpStatusCode.NotFound,
                    lessonFactory: _ => null),
                CreateGetAiSummaryFailureCase(
                    "invalid-missing-lesson-record",
                    HttpStatusCode.NotFound,
                    lessonFactory: lessonId => CreateLesson(lessonId, includeLessonRecord: false, sessionOutcomes: "Seed outcomes")),
                CreateGetAiSummaryFailureCase(
                    "invalid-missing-session-outcomes",
                    HttpStatusCode.NotFound,
                    lessonFactory: lessonId => CreateLesson(lessonId, includeLessonRecord: true, sessionOutcomes: null)),
                CreateGetAiSummaryAiNullCase()
            ],
            ExceptionCases =
            [
                CreateGetAiSummaryExceptionCase()
            ]
        };
    }

    private static UseCaseCaseSet CreateGetAiSummaryValidatorFailureCase()
    {
        return CreateCaseSet(
            name: "invalid-empty-lesson-id",
            kind: UseCaseCaseKind.Invalid,
            scenario: new UseCaseScenario
            {
                Request = new GetAiSummaryCommand
                {
                    LessonId = Guid.Empty
                },
                AssertValidatorResultAsync = validationResult =>
                {
                    Assert.False(validationResult.IsValid);
                    Assert.Contains(validationResult.Errors, error => error.PropertyName == nameof(GetAiSummaryCommand.LessonId));
                    return Task.CompletedTask;
                }
            },
            handlerExpectation: UseCaseHandlerExpectation.Skip,
            validatorExpectation: UseCaseValidatorExpectation.Fail);
    }

    private static UseCaseCaseSet CreateGetAiSummarySuccessCase(
        string name,
        UseCaseCaseKind kind,
        AnalysisResponse analysisResponse,
        decimal expectedCoveragePercent,
        bool expectWarningOutbox)
    {
        var lessonId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var lesson = CreateLesson(lessonId, includeLessonRecord: true, sessionOutcomes: "Learner can explain grammar rules");
        var transcriptParts = new[] { "segment one", "segment two" };
        LessonScript? addedLessonScript = null;
        OutboxEvent? addedOutboxEvent = null;
        Mock<IAiService>? aiServiceMock = null;
        Mock<IRedisService>? redisServiceMock = null;
        Mock<IUnitOfWork>? unitOfWorkMock = null;
        Mock<IGenericRepository<Lesson, Guid>>? lessonRepositoryMock = null;
        Mock<IGenericRepository<LessonScript, Guid>>? lessonScriptRepositoryMock = null;
        Mock<IGenericRepository<OutboxEvent, Guid>>? outboxRepositoryMock = null;

        return CreateCaseSet(
            name: name,
            kind: kind,
            scenario: new UseCaseScenario
            {
                Request = new GetAiSummaryCommand
                {
                    LessonId = lessonId
                },
                ArrangeMocks = mocks =>
                {
                    aiServiceMock = mocks.StrictMock<IAiService>();
                    redisServiceMock = mocks.StrictMock<IRedisService>();
                    unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    lessonRepositoryMock = new Mock<IGenericRepository<Lesson, Guid>>(MockBehavior.Strict);
                    lessonScriptRepositoryMock = new Mock<IGenericRepository<LessonScript, Guid>>(MockBehavior.Strict);
                    outboxRepositoryMock = new Mock<IGenericRepository<OutboxEvent, Guid>>(MockBehavior.Strict);

                    ConfigureUnitOfWork(unitOfWorkMock, lessonRepositoryMock, lessonScriptRepositoryMock, outboxRepositoryMock);
                    ConfigureLessonLookup(lessonRepositoryMock, lessonId, lesson);

                    redisServiceMock
                        .Setup(service => service.SortedSetRangeAsync(lessonId.ToString()))
                        .ReturnsAsync(transcriptParts);
                    redisServiceMock
                        .Setup(service => service.DeleteCacheAsync(lessonId.ToString()))
                        .ReturnsAsync(true);

                    aiServiceMock
                        .Setup(service => service.AnalyzeContentAsync(It.Is<AnalysisRequest>(request =>
                            request.Transcript == string.Join(" ", transcriptParts)
                            && request.Outcome == lesson.Session!.Outcomes)))
                        .ReturnsAsync(CreateAnalysisResponse(
                            analysisResponse.AiSummarizeText,
                            analysisResponse.Detail.Pass.Count,
                            analysisResponse.Detail.Fail.Count));

                    lessonScriptRepositoryMock
                        .Setup(repository => repository.Add(It.IsAny<LessonScript>()))
                        .Callback<LessonScript>(entity => addedLessonScript = entity);

                    outboxRepositoryMock
                        .Setup(repository => repository.Add(It.IsAny<OutboxEvent>()))
                        .Callback<OutboxEvent>(entity => addedOutboxEvent = entity);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                        .ReturnsAsync(1);
                },
                AssertHandlerResultAsync = resultObject =>
                {
                    var result = Assert.IsType<Result<AnalysisResponse>>(resultObject);
                    var response = Assert.IsType<AnalysisResponse>(result.Data);

                    Assert.True(result.IsSuccess);
                    Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                    Assert.Equal(expectedCoveragePercent, response.CoveragePercent);
                    Assert.NotNull(addedLessonScript);
                    Assert.Equal(lesson.Id, addedLessonScript!.LessonId);
                    Assert.Equal(lesson.LessonRecord!.Id, addedLessonScript.RecordId);
                    Assert.Equal("segment one segment two", addedLessonScript.FullText);
                    Assert.Equal(analysisResponse.AiSummarizeText, addedLessonScript.SummarizeText);
                    Assert.Equal(expectedCoveragePercent, addedLessonScript.CoveragePercent);
                    Assert.Equal(JsonSerializer.Serialize(response.Detail), addedLessonScript.LessonOutcome);

                    if (expectWarningOutbox)
                    {
                        Assert.NotNull(addedOutboxEvent);
                    }
                    else
                    {
                        Assert.Null(addedOutboxEvent);
                    }

                    lessonRepositoryMock!.Verify(repository => repository.FindByIdAsync(
                        lessonId,
                        false,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()), Times.Once);
                    lessonScriptRepositoryMock!.Verify(repository => repository.Add(It.IsAny<LessonScript>()), Times.Once);
                    outboxRepositoryMock!.Verify(repository => repository.Add(It.IsAny<OutboxEvent>()), expectWarningOutbox ? Times.Once() : Times.Never());
                    aiServiceMock!.Verify(service => service.AnalyzeContentAsync(It.IsAny<AnalysisRequest>()), Times.Once);
                    redisServiceMock!.Verify(service => service.SortedSetRangeAsync(lessonId.ToString()), Times.Once);
                    redisServiceMock!.Verify(service => service.DeleteCacheAsync(lessonId.ToString()), Times.Once);
                    unitOfWorkMock!.Verify(unitOfWork => unitOfWork.SaveChangesAsync(true, true), Times.Once);
                    return Task.CompletedTask;
                }
            },
            handlerExpectation: UseCaseHandlerExpectation.Completes,
            validatorExpectation: UseCaseValidatorExpectation.Pass);
    }

    private static UseCaseCaseSet CreateGetAiSummaryFailureCase(
        string name,
        HttpStatusCode expectedStatusCode,
        Func<Guid, Lesson?> lessonFactory)
    {
        var lessonId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var lesson = lessonFactory(lessonId);
        Mock<IAiService>? aiServiceMock = null;
        Mock<IRedisService>? redisServiceMock = null;
        Mock<IUnitOfWork>? unitOfWorkMock = null;
        Mock<IGenericRepository<Lesson, Guid>>? lessonRepositoryMock = null;

        return CreateCaseSet(
            name: name,
            kind: UseCaseCaseKind.Invalid,
            scenario: new UseCaseScenario
            {
                Request = new GetAiSummaryCommand
                {
                    LessonId = lessonId
                },
                ArrangeMocks = mocks =>
                {
                    aiServiceMock = mocks.StrictMock<IAiService>();
                    redisServiceMock = mocks.StrictMock<IRedisService>();
                    unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    lessonRepositoryMock = new Mock<IGenericRepository<Lesson, Guid>>(MockBehavior.Strict);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<Lesson, Guid>())
                        .Returns(lessonRepositoryMock.Object);

                    ConfigureLessonLookup(lessonRepositoryMock, lessonId, lesson);

                    redisServiceMock
                        .Setup(service => service.SortedSetRangeAsync(lessonId.ToString()))
                        .ReturnsAsync(new[] { "segment one", "segment two" });
                },
                AssertHandlerResultAsync = resultObject =>
                {
                    var result = Assert.IsType<Result<AnalysisResponse>>(resultObject);

                    Assert.True(result.IsFailure);
                    Assert.Equal(expectedStatusCode, result.HttpStatusCode);
                    Assert.Null(result.Data);

                    lessonRepositoryMock!.Verify(repository => repository.FindByIdAsync(
                        lessonId,
                        false,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()), Times.Once);
                    redisServiceMock!.Verify(service => service.SortedSetRangeAsync(lessonId.ToString()), Times.Once);
                    aiServiceMock!.Verify(service => service.AnalyzeContentAsync(It.IsAny<AnalysisRequest>()), Times.Never);
                    unitOfWorkMock!.Verify(unitOfWork => unitOfWork.SaveChangesAsync(true, true), Times.Never);
                    return Task.CompletedTask;
                }
            },
            handlerExpectation: UseCaseHandlerExpectation.Failure,
            validatorExpectation: UseCaseValidatorExpectation.Pass);
    }

    private static UseCaseCaseSet CreateGetAiSummaryAiNullCase()
    {
        var lessonId = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var lesson = CreateLesson(lessonId, includeLessonRecord: true, sessionOutcomes: "Learner can summarize a conversation");
        Mock<IAiService>? aiServiceMock = null;
        Mock<IRedisService>? redisServiceMock = null;
        Mock<IUnitOfWork>? unitOfWorkMock = null;
        Mock<IGenericRepository<Lesson, Guid>>? lessonRepositoryMock = null;

        return CreateCaseSet(
            name: "invalid-ai-response-null",
            kind: UseCaseCaseKind.Invalid,
            scenario: new UseCaseScenario
            {
                Request = new GetAiSummaryCommand
                {
                    LessonId = lessonId
                },
                ArrangeMocks = mocks =>
                {
                    aiServiceMock = mocks.StrictMock<IAiService>();
                    redisServiceMock = mocks.StrictMock<IRedisService>();
                    unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    lessonRepositoryMock = new Mock<IGenericRepository<Lesson, Guid>>(MockBehavior.Strict);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<Lesson, Guid>())
                        .Returns(lessonRepositoryMock.Object);

                    ConfigureLessonLookup(lessonRepositoryMock, lessonId, lesson);

                    redisServiceMock
                        .Setup(service => service.SortedSetRangeAsync(lessonId.ToString()))
                        .ReturnsAsync(new[] { "segment one", "segment two" });

                    aiServiceMock
                        .Setup(service => service.AnalyzeContentAsync(It.IsAny<AnalysisRequest>()))
                        .ReturnsAsync((AnalysisResponse?)null);
                },
                AssertHandlerResultAsync = resultObject =>
                {
                    var result = Assert.IsType<Result<AnalysisResponse>>(resultObject);

                    Assert.True(result.IsFailure);
                    Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                    Assert.Null(result.Data);

                    lessonRepositoryMock!.Verify(repository => repository.FindByIdAsync(
                        lessonId,
                        false,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<Lesson, object>>[]>()), Times.Once);
                    redisServiceMock!.Verify(service => service.SortedSetRangeAsync(lessonId.ToString()), Times.Once);
                    aiServiceMock!.Verify(service => service.AnalyzeContentAsync(It.IsAny<AnalysisRequest>()), Times.Once);
                    unitOfWorkMock!.Verify(unitOfWork => unitOfWork.SaveChangesAsync(true, true), Times.Never);
                    redisServiceMock.Verify(service => service.DeleteCacheAsync(It.IsAny<string>()), Times.Never);
                    return Task.CompletedTask;
                }
            },
            handlerExpectation: UseCaseHandlerExpectation.Failure,
            validatorExpectation: UseCaseValidatorExpectation.Pass);
    }

    private static UseCaseCaseSet CreateGetAiSummaryExceptionCase()
    {
        var lessonId = Guid.Parse("77777777-7777-7777-7777-777777777777");
        Mock<IRedisService>? redisServiceMock = null;

        return CreateCaseSet(
            name: "exception-redis-read-throws",
            kind: UseCaseCaseKind.Exception,
            scenario: new UseCaseScenario
            {
                Request = new GetAiSummaryCommand
                {
                    LessonId = lessonId
                },
                ArrangeMocks = mocks =>
                {
                    mocks.StrictMock<IAiService>();
                    mocks.StrictMock<IUnitOfWork>();
                    redisServiceMock = mocks.StrictMock<IRedisService>();

                    redisServiceMock
                        .Setup(service => service.SortedSetRangeAsync(lessonId.ToString()))
                        .ThrowsAsync(new InvalidOperationException("redis failure"));
                },
                AssertHandlerResultAsync = resultObject =>
                {
                    var result = Assert.IsType<Result<AnalysisResponse>>(resultObject);

                    Assert.True(result.IsFailure);
                    Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatusCode);
                    Assert.Null(result.Data);
                    redisServiceMock!.Verify(service => service.SortedSetRangeAsync(lessonId.ToString()), Times.Once);
                    return Task.CompletedTask;
                }
            },
            handlerExpectation: UseCaseHandlerExpectation.Failure,
            validatorExpectation: UseCaseValidatorExpectation.Pass);
    }

    private static void ConfigureUnitOfWork(
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IGenericRepository<Lesson, Guid>> lessonRepositoryMock,
        Mock<IGenericRepository<LessonScript, Guid>> lessonScriptRepositoryMock,
        Mock<IGenericRepository<OutboxEvent, Guid>> outboxRepositoryMock)
    {
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Lesson, Guid>())
            .Returns(lessonRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<LessonScript, Guid>())
            .Returns(lessonScriptRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<OutboxEvent, Guid>())
            .Returns(outboxRepositoryMock.Object);
    }

    private static void ConfigureLessonLookup(
        Mock<IGenericRepository<Lesson, Guid>> lessonRepositoryMock,
        Guid lessonId,
        Lesson? lesson)
    {
        lessonRepositoryMock
            .Setup(repository => repository.FindByIdAsync(
                lessonId,
                false,
                It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Lesson, object>>[]>()))
            .ReturnsAsync(lesson);
    }

    private static Lesson CreateLesson(Guid lessonId, bool includeLessonRecord, string? sessionOutcomes)
    {
        var studentUser = new User
        {
            Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            FirstName = "Student",
            LastName = "Tester",
            UserName = "student-user",
            Email = "student@example.com",
            PasswordHash = UseCaseSeedDefaults.KnownPasswordHash,
            Status = UseCaseSeedDefaults.KnownStatus
        };

        var tutorUser = new User
        {
            Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            FirstName = "Tutor",
            LastName = "Reviewer",
            UserName = "tutor-user",
            Email = "tutor@example.com",
            PasswordHash = UseCaseSeedDefaults.KnownPasswordHash,
            Status = UseCaseSeedDefaults.KnownStatus
        };

        var student = new Student
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            UserId = studentUser.Id,
            Status = UseCaseSeedDefaults.KnownStatus,
            User = studentUser
        };

        var tutor = new Tutor
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            UserId = tutorUser.Id,
            Status = UseCaseSeedDefaults.KnownStatus,
            VerifiedStatus = "Verified",
            User = tutorUser
        };

        var session = new CourseSession
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            TutorId = tutor.Id,
            Tutor = tutor,
            Outcomes = sessionOutcomes
        };

        var lesson = new Lesson
        {
            Id = lessonId,
            TutorId = tutor.Id,
            StudentId = student.Id,
            SessionId = session.Id,
            StartTime = UseCaseSeedDefaults.KnownUtcDateTime,
            EndTime = UseCaseSeedDefaults.KnownUtcDateTime.AddHours(1),
            Status = "Scheduled",
            Student = student,
            Tutor = tutor,
            Session = session
        };

        if (includeLessonRecord)
        {
            lesson.LessonRecord = new LessonRecord
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                LessonId = lesson.Id,
                RecordUrl = UseCaseSeedDefaults.KnownUrl,
                RecordingStartedAt = UseCaseSeedDefaults.KnownUtcDateTime,
                RecordingEndedAt = UseCaseSeedDefaults.KnownUtcDateTime.AddMinutes(30),
                Lesson = lesson
            };
        }

        student.User.Student = student;
        tutor.User.Tutor = tutor;
        student.Lessons.Add(lesson);
        tutor.Lessons.Add(lesson);
        session.Lessons.Add(lesson);
        return lesson;
    }

    private static AnalysisResponse CreateAnalysisResponse(string summaryText, int passCount, int failCount)
    {
        return new AnalysisResponse
        {
            AiSummarizeText = summaryText,
            Detail = new DetailResult
            {
                Pass = Enumerable.Range(0, passCount).Select(index => $"pass-{index}").ToList(),
                Fail = Enumerable.Range(0, failCount).Select(index => $"fail-{index}").ToList()
            }
        };
    }

    private static UseCaseCaseSet CreateCaseSet(
        string name,
        UseCaseCaseKind kind,
        UseCaseScenario scenario,
        UseCaseHandlerExpectation handlerExpectation,
        UseCaseValidatorExpectation validatorExpectation)
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
                Scenario = scenario
            }
        };
    }
}
