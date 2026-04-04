using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.UpdateLesson;

internal enum UpdateLessonCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidLessonMissing,
    InvalidEnrollmentMissing,
    InvalidStudentMissing,
    InvalidSessionMissing,
    ExceptionPath,
}

internal static class UpdateLessonTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateLesson",
        RequestTypeFullName = "EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Lessons/UpdateLesson/UpdateLessonCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Lessons/UpdateLesson/UpdateLessonCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Lessons/UpdateLesson/UpdateLessonCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateLessonCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateLessonCase.BoundaryDefault),
        BuildCase(UpdateLessonCase.InvalidRequestShape),
        BuildCase(UpdateLessonCase.InvalidLessonMissing),
        BuildCase(UpdateLessonCase.InvalidEnrollmentMissing),
        BuildCase(UpdateLessonCase.InvalidStudentMissing),
        BuildCase(UpdateLessonCase.InvalidSessionMissing),
        BuildCase(UpdateLessonCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateLessonCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateLessonCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateLessonCase.InvalidRequestShape),
        BuildCase(UpdateLessonCase.InvalidLessonMissing),
        BuildCase(UpdateLessonCase.InvalidEnrollmentMissing),
        BuildCase(UpdateLessonCase.InvalidStudentMissing),
        BuildCase(UpdateLessonCase.InvalidSessionMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateLessonCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateLessonCase caseId)
    {
        return caseId switch
        {
            UpdateLessonCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateLessonCase.InvalidLessonMissing => CreateCase("invalid-lesson-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonCase.InvalidEnrollmentMissing => CreateCase("invalid-enrollment-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonCase.InvalidStudentMissing => CreateCase("invalid-student-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonCase.InvalidSessionMissing => CreateCase("invalid-session-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand CreateRequest(UpdateLessonCase caseId)
    {
        return caseId switch
        {
            UpdateLessonCase.ValidDefault => new global::EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            UpdateLessonCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = null, SessionId = null, StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            UpdateLessonCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), StudentId = Guid.Parse("00000000-0000-0000-0000-000000000000"), EnrollmentId = Guid.Parse("00000000-0000-0000-0000-000000000000"), ModuleId = Guid.Parse("00000000-0000-0000-0000-000000000000"), SessionId = Guid.Parse("00000000-0000-0000-0000-000000000000"), StartTime = new DateTime(2026, 4, 3, 13, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2026, 4, 3, 12, 55, 4, DateTimeKind.Utc), MeetingUrl = "" },
            UpdateLessonCase.InvalidLessonMissing => new global::EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            UpdateLessonCase.InvalidEnrollmentMissing => new global::EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            UpdateLessonCase.InvalidStudentMissing => new global::EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            UpdateLessonCase.InvalidSessionMissing => new global::EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            UpdateLessonCase.ExceptionPath => new global::EngConnect.Application.UseCases.Lessons.UpdateLesson.UpdateLessonCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), EnrollmentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SessionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTime = new DateTime(2035, 4, 3, 15, 55, 4, DateTimeKind.Utc), EndTime = new DateTime(2035, 4, 3, 16, 55, 4, DateTimeKind.Utc), MeetingUrl = "https://example.com/test" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
