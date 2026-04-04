using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.CreateLesson;

internal enum CreateLessonCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidNotFoundOrNull,
    InvalidEnrollmentMissing,
    InvalidStudentMissing,
    InvalidSessionMissing,
    ExceptionPath,
}

internal static class CreateLessonTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateLesson",
        RequestTypeFullName = "EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Lessons/CreateLesson/CreateLessonCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Lessons/CreateLesson/CreateLessonCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Lessons/CreateLesson/CreateLessonCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateLessonCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateLessonCase.BoundaryDefault),
        BuildCase(CreateLessonCase.InvalidRequestShape),
        BuildCase(CreateLessonCase.InvalidNotFoundOrNull),
        BuildCase(CreateLessonCase.InvalidEnrollmentMissing),
        BuildCase(CreateLessonCase.InvalidStudentMissing),
        BuildCase(CreateLessonCase.InvalidSessionMissing),
        BuildCase(CreateLessonCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreateLessonCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateLessonCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateLessonCase.InvalidRequestShape),
        BuildCase(CreateLessonCase.InvalidNotFoundOrNull),
        BuildCase(CreateLessonCase.InvalidEnrollmentMissing),
        BuildCase(CreateLessonCase.InvalidStudentMissing),
        BuildCase(CreateLessonCase.InvalidSessionMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateLessonCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreateLessonCase caseId)
    {
        return caseId switch
        {
            CreateLessonCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateLessonCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            CreateLessonCase.InvalidEnrollmentMissing => CreateCase("invalid-enrollment-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonCase.InvalidStudentMissing => CreateCase("invalid-student-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonCase.InvalidSessionMissing => CreateCase("invalid-session-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand CreateRequest(CreateLessonCase caseId)
    {
        return caseId switch
        {
            CreateLessonCase.ValidDefault => new global::EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand { StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            CreateLessonCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand { StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = null, SessionId = null, StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            CreateLessonCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand { StudentId = Guid.Parse("00000000-0000-0000-0000-000000000000"), EnrollmentId = Guid.Parse("00000000-0000-0000-0000-000000000000"), ModuleId = Guid.Parse("00000000-0000-0000-0000-000000000000"), SessionId = Guid.Parse("00000000-0000-0000-0000-000000000000"), StartTime = new DateTime(2026, 4, 3, 13, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2026, 4, 3, 12, 55, 4, DateTimeKind.Utc), MeetingUrl = "" },
            CreateLessonCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand { StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            CreateLessonCase.InvalidEnrollmentMissing => new global::EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand { StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            CreateLessonCase.InvalidStudentMissing => new global::EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand { StudentId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            CreateLessonCase.InvalidSessionMissing => new global::EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand { StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            CreateLessonCase.ExceptionPath => new global::EngConnect.Application.UseCases.Lessons.CreateLesson.CreateLessonCommand { StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
