using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript;

internal enum UpdateLessonScriptCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidLessonRecordExistsMissing,
    InvalidLessonScriptExistsMissing,
    ExceptionPath,
}

internal static class UpdateLessonScriptTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateLessonScript",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/UpdateLessonScript/UpdateLessonScriptCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/UpdateLessonScript/UpdateLessonScriptCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/UpdateLessonScript/UpdateLessonScriptCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateLessonScriptCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateLessonScriptCase.BoundaryDefault),
        BuildCase(UpdateLessonScriptCase.InvalidRequestShape),
        BuildCase(UpdateLessonScriptCase.InvalidLessonRecordExistsMissing),
        BuildCase(UpdateLessonScriptCase.InvalidLessonScriptExistsMissing),
        BuildCase(UpdateLessonScriptCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateLessonScriptCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateLessonScriptCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateLessonScriptCase.InvalidRequestShape),
        BuildCase(UpdateLessonScriptCase.InvalidLessonRecordExistsMissing),
        BuildCase(UpdateLessonScriptCase.InvalidLessonScriptExistsMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateLessonScriptCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateLessonScriptCase caseId)
    {
        return caseId switch
        {
            UpdateLessonScriptCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonScriptCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonScriptCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateLessonScriptCase.InvalidLessonRecordExistsMissing => CreateCase("invalid-lessonRecordExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonScriptCase.InvalidLessonScriptExistsMissing => CreateCase("invalid-lessonScriptExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonScriptCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommand CreateRequest(UpdateLessonScriptCase caseId)
    {
        return caseId switch
        {
            UpdateLessonScriptCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", FullText = "Sample description", SummarizeText = "Sample description", LessonOutcome = "SampleValue", CoveragePercent = 1m },
            UpdateLessonScriptCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", FullText = "Sample description", SummarizeText = "Sample description", LessonOutcome = "SampleValue", CoveragePercent = 1m },
            UpdateLessonScriptCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommand { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), LessonId = Guid.Parse("00000000-0000-0000-0000-000000000000"), RecordId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Language = "Invalid", FullText = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", SummarizeText = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", LessonOutcome = "", CoveragePercent = 1m },
            UpdateLessonScriptCase.InvalidLessonRecordExistsMissing => new global::EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Language = "En", FullText = "Sample description", SummarizeText = "Sample description", LessonOutcome = "SampleValue", CoveragePercent = 1m },
            UpdateLessonScriptCase.InvalidLessonScriptExistsMissing => new global::EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), LessonId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), RecordId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Language = "En", FullText = "Sample description", SummarizeText = "Sample description", LessonOutcome = "SampleValue", CoveragePercent = 1m },
            UpdateLessonScriptCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript.UpdateLessonScriptCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", FullText = "Sample description", SummarizeText = "Sample description", LessonOutcome = "SampleValue", CoveragePercent = 1m },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}