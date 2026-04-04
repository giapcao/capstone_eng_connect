using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;

internal enum CreateLessonRecordCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidLessonExistsMissing,
    ExceptionPath,
}

internal static class CreateLessonRecordTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateLessonRecord",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord.CreateLessonRecordCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord.CreateLessonRecordCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord.CreateLessonRecordCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/CreateLessonRecord/CreateLessonRecordCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/CreateLessonRecord/CreateLessonRecordCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/CreateLessonRecord/CreateLessonRecordCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateLessonRecordCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateLessonRecordCase.BoundaryDefault),
        BuildCase(CreateLessonRecordCase.InvalidRequestShape),
        BuildCase(CreateLessonRecordCase.InvalidLessonExistsMissing),
        BuildCase(CreateLessonRecordCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreateLessonRecordCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateLessonRecordCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateLessonRecordCase.InvalidRequestShape),
        BuildCase(CreateLessonRecordCase.InvalidLessonExistsMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateLessonRecordCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreateLessonRecordCase caseId)
    {
        return caseId switch
        {
            CreateLessonRecordCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonRecordCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonRecordCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateLessonRecordCase.InvalidLessonExistsMissing => CreateCase("invalid-lessonExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonRecordCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord.CreateLessonRecordCommand CreateRequest(CreateLessonRecordCase caseId)
    {
        return caseId switch
        {
            CreateLessonRecordCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord.CreateLessonRecordCommand { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordUrl = "https://example.com/test", DurationSeconds = 1, RecordingStartedAt = new DateTime(2026, 4, 3, 12, 55, 4, DateTimeKind.Utc), RecordingEndedAt = new DateTime(2026, 4, 3, 13, 55, 4, DateTimeKind.Utc) },
            CreateLessonRecordCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord.CreateLessonRecordCommand { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordUrl = "https://example.com/test", DurationSeconds = 1, RecordingStartedAt = new DateTime(2026, 4, 3, 12, 55, 4, DateTimeKind.Utc), RecordingEndedAt = new DateTime(2026, 4, 3, 13, 55, 4, DateTimeKind.Utc) },
            CreateLessonRecordCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord.CreateLessonRecordCommand { LessonId = Guid.Parse("00000000-0000-0000-0000-000000000000"), RecordUrl = "", DurationSeconds = 0, RecordingStartedAt = new DateTime(2026, 4, 3, 16, 55, 4, DateTimeKind.Utc), RecordingEndedAt = new DateTime(2026, 4, 3, 11, 55, 4, DateTimeKind.Utc) },
            CreateLessonRecordCase.InvalidLessonExistsMissing => new global::EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord.CreateLessonRecordCommand { LessonId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), RecordUrl = "https://example.com/test", DurationSeconds = 1, RecordingStartedAt = new DateTime(2026, 4, 3, 12, 55, 4, DateTimeKind.Utc), RecordingEndedAt = new DateTime(2026, 4, 3, 13, 55, 4, DateTimeKind.Utc) },
            CreateLessonRecordCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord.CreateLessonRecordCommand { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordUrl = "https://example.com/test", DurationSeconds = 1, RecordingStartedAt = new DateTime(2026, 4, 3, 12, 55, 4, DateTimeKind.Utc), RecordingEndedAt = new DateTime(2026, 4, 3, 13, 55, 4, DateTimeKind.Utc) },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}