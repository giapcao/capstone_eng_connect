using System.Reflection;
using EngConnect.Application.UseCases.AiSummerize.GetAiSummary;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AiSummerize.GetAiSummary;

public class GetAiSummaryPrivateMethodTests
{
    [Fact]
    public void CreateLessonScript_private_method_returns_expected_script()
    {
        var lessonId = Guid.Parse("11111111-2222-3333-4444-555555555555");
        var recordId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        const string transcript = "hello lesson";
        var analysisResponse = new AnalysisResponse
        {
            AiSummarizeText = "summary",
            CoveragePercent = 75.5m,
            Detail = new DetailResult
            {
                Pass = ["grammar"],
                Fail = ["pronunciation"]
            }
        };

        var result = InvokePrivateStatic<LessonScript>(
            nameof(GetAiSummaryCommandHandler),
            "CreateLessonScript",
            lessonId,
            recordId,
            transcript,
            analysisResponse);

        Assert.Equal(lessonId, result.LessonId);
        Assert.Equal(recordId, result.RecordId);
        Assert.Equal("Vi", result.Language);
        Assert.Equal(transcript, result.FullText);
        Assert.Equal("summary", result.SummarizeText);
        Assert.Equal(75.5m, result.CoveragePercent);
        Assert.Contains("grammar", result.LessonOutcome);
        Assert.Contains("pronunciation", result.LessonOutcome);
    }

    [Fact]
    public void AddWarningToOutBox_private_method_pushes_outbox_event()
    {
        var outboxRepository = new Mock<IGenericRepository<OutboxEvent, Guid>>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);

        unitOfWork
            .Setup(work => work.GetRepository<OutboxEvent, Guid>())
            .Returns(outboxRepository.Object);

        OutboxEvent? capturedEvent = null;
        outboxRepository
            .Setup(repository => repository.Add(It.IsAny<OutboxEvent>()))
            .Callback<OutboxEvent>(entity => capturedEvent = entity);

        var handler = new GetAiSummaryCommandHandler(
            Mock.Of<IAiService>(),
            Mock.Of<IRedisService>(),
            unitOfWork.Object,
            NullLogger<GetAiSummaryCommandHandler>.Instance);

        var lesson = CreateLesson();
        var analysisResponse = new AnalysisResponse
        {
            CoveragePercent = 20m,
            Detail = new DetailResult
            {
                Pass = [],
                Fail = ["missing outcome"]
            }
        };

        InvokePrivateInstance(
            handler,
            "AddWarningToOutBox",
            lesson,
            analysisResponse);

        outboxRepository.Verify(repository => repository.Add(It.IsAny<OutboxEvent>()), Times.Once);
        Assert.NotNull(capturedEvent);
        Assert.Equal(nameof(Lesson), capturedEvent!.AggregateType);
        Assert.Equal(lesson.Id, capturedEvent.AggregateId);
    }

    private static T InvokePrivateStatic<T>(string declaringTypeName, string methodName, params object[] arguments)
    {
        var handlerType = typeof(GetAiSummaryCommandHandler);
        var method = handlerType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static)
                     ?? throw new InvalidOperationException($"Method {methodName} was not found on {declaringTypeName}.");

        return (T)(method.Invoke(null, arguments)
                   ?? throw new InvalidOperationException($"Method {methodName} returned null."));
    }

    private static void InvokePrivateInstance(object target, string methodName, params object[] arguments)
    {
        var method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
                     ?? throw new InvalidOperationException($"Method {methodName} was not found on {target.GetType().FullName}.");

        method.Invoke(target, arguments);
    }

    private static Lesson CreateLesson()
    {
        var studentUser = new User
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            FirstName = "Student",
            LastName = "Sample",
            UserName = "student.sample",
            Email = "student@example.com",
            PasswordHash = "hash",
            Status = "Active"
        };

        var tutorUser = new User
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            FirstName = "Tutor",
            LastName = "Sample",
            UserName = "tutor.sample",
            Email = "tutor@example.com",
            PasswordHash = "hash",
            Status = "Active"
        };

        var student = new Student
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            UserId = studentUser.Id,
            Status = "Active",
            User = studentUser
        };

        var tutor = new Tutor
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000004"),
            UserId = tutorUser.Id,
            Status = "Active",
            VerifiedStatus = "Verified",
            User = tutorUser
        };

        return new Lesson
        {
            Id = Guid.Parse("50000000-0000-0000-0000-000000000005"),
            StudentId = student.Id,
            TutorId = tutor.Id,
            StartTime = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc),
            EndTime = new DateTime(2026, 1, 1, 9, 0, 0, DateTimeKind.Utc),
            Student = student,
            Tutor = tutor
        };
    }
}
