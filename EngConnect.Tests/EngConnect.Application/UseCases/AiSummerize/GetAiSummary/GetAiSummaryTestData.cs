using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AiSummerize.GetAiSummary;

internal enum GetAiSummaryCase
{
    ValidHighCoverage,
    ValidLowCoverageWarning,
    BoundaryZeroTotalOutcomes,
    InvalidEmptyLessonId,
    InvalidLessonNotFound,
    InvalidMissingLessonRecord,
    InvalidMissingSessionOutcomes,
    InvalidAiResponseNull,
    ExceptionRedisReadThrows,
}

internal static class GetAiSummaryTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetAiSummary",
        RequestTypeFullName = "EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/AiSummerize/GetAiSummary/GetAiSummaryCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/AiSummerize/GetAiSummary/GetAiSummaryCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/AiSummerize/GetAiSummary/GetAiSummaryCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetAiSummaryCase.ValidHighCoverage),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetAiSummaryCase.ValidLowCoverageWarning),
        BuildCase(GetAiSummaryCase.BoundaryZeroTotalOutcomes),
        BuildCase(GetAiSummaryCase.InvalidEmptyLessonId),
        BuildCase(GetAiSummaryCase.InvalidLessonNotFound),
        BuildCase(GetAiSummaryCase.InvalidMissingLessonRecord),
        BuildCase(GetAiSummaryCase.InvalidMissingSessionOutcomes),
        BuildCase(GetAiSummaryCase.InvalidAiResponseNull),
        BuildCase(GetAiSummaryCase.ExceptionRedisReadThrows),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetAiSummaryCase.ValidHighCoverage),
        BuildCase(GetAiSummaryCase.ValidLowCoverageWarning),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetAiSummaryCase.BoundaryZeroTotalOutcomes),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetAiSummaryCase.InvalidEmptyLessonId),
        BuildCase(GetAiSummaryCase.InvalidLessonNotFound),
        BuildCase(GetAiSummaryCase.InvalidMissingLessonRecord),
        BuildCase(GetAiSummaryCase.InvalidMissingSessionOutcomes),
        BuildCase(GetAiSummaryCase.InvalidAiResponseNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetAiSummaryCase.ExceptionRedisReadThrows),
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

    private static UseCaseCaseSet BuildCase(GetAiSummaryCase caseId)
    {
        return caseId switch
        {
            GetAiSummaryCase.ValidHighCoverage => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("valid-high-coverage"), CreateRequest(caseId)),
            GetAiSummaryCase.ValidLowCoverageWarning => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("valid-low-coverage-warning"), CreateRequest(caseId)),
            GetAiSummaryCase.BoundaryZeroTotalOutcomes => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("boundary-zero-total-outcomes"), CreateRequest(caseId)),
            GetAiSummaryCase.InvalidEmptyLessonId => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-empty-lesson-id"), CreateRequest(caseId)),
            GetAiSummaryCase.InvalidLessonNotFound => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-lesson-not-found"), CreateRequest(caseId)),
            GetAiSummaryCase.InvalidMissingLessonRecord => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-missing-lesson-record"), CreateRequest(caseId)),
            GetAiSummaryCase.InvalidMissingSessionOutcomes => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-missing-session-outcomes"), CreateRequest(caseId)),
            GetAiSummaryCase.InvalidAiResponseNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-ai-response-null"), CreateRequest(caseId)),
            GetAiSummaryCase.ExceptionRedisReadThrows => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("exception-redis-read-throws"), CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand CreateRequest(GetAiSummaryCase caseId)
    {
        return caseId switch
        {
            GetAiSummaryCase.ValidHighCoverage => new global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand { LessonId = Guid.Parse("44444444-4444-4444-4444-444444444444") },
            GetAiSummaryCase.ValidLowCoverageWarning => new global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand { LessonId = Guid.Parse("44444444-4444-4444-4444-444444444444") },
            GetAiSummaryCase.BoundaryZeroTotalOutcomes => new global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand { LessonId = Guid.Parse("44444444-4444-4444-4444-444444444444") },
            GetAiSummaryCase.InvalidEmptyLessonId => new global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand { LessonId = Guid.Parse("00000000-0000-0000-0000-000000000000") },
            GetAiSummaryCase.InvalidLessonNotFound => new global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand { LessonId = Guid.Parse("55555555-5555-5555-5555-555555555555") },
            GetAiSummaryCase.InvalidMissingLessonRecord => new global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand { LessonId = Guid.Parse("55555555-5555-5555-5555-555555555555") },
            GetAiSummaryCase.InvalidMissingSessionOutcomes => new global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand { LessonId = Guid.Parse("55555555-5555-5555-5555-555555555555") },
            GetAiSummaryCase.InvalidAiResponseNull => new global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand { LessonId = Guid.Parse("66666666-6666-6666-6666-666666666666") },
            GetAiSummaryCase.ExceptionRedisReadThrows => new global::EngConnect.Application.UseCases.AiSummerize.GetAiSummary.GetAiSummaryCommand { LessonId = Guid.Parse("77777777-7777-7777-7777-777777777777") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}