using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;

internal enum CreateLessonScriptCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidLessonExistsMissing,
    ExceptionPath,
}

internal static class CreateLessonScriptTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateLessonScript",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonScripts.CreateLessonScript.CreateLessonScriptCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonScripts.CreateLessonScript.CreateLessonScriptCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.LessonScripts.CreateLessonScript.CreateLessonScriptCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/CreateLessonScript/CreateLessonScriptCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/CreateLessonScript/CreateLessonScriptCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/CreateLessonScript/CreateLessonScriptCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateLessonScriptCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateLessonScriptCase.BoundaryDefault),
        BuildCase(CreateLessonScriptCase.InvalidRequestShape),
        BuildCase(CreateLessonScriptCase.InvalidLessonExistsMissing),
        BuildCase(CreateLessonScriptCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreateLessonScriptCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateLessonScriptCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateLessonScriptCase.InvalidRequestShape),
        BuildCase(CreateLessonScriptCase.InvalidLessonExistsMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateLessonScriptCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreateLessonScriptCase caseId)
    {
        return caseId switch
        {
            CreateLessonScriptCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonScriptCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonScriptCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateLessonScriptCase.InvalidLessonExistsMissing => CreateCase("invalid-lessonExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateLessonScriptCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonScripts.CreateLessonScript.CreateLessonScriptCommand CreateRequest(CreateLessonScriptCase caseId)
    {
        return caseId switch
        {
            CreateLessonScriptCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonScripts.CreateLessonScript.CreateLessonScriptCommand { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", FullText = "Sample description", SummarizeText = "Sample description", LessonOutcome = "SampleValue", CoveragePercent = 1m },
            CreateLessonScriptCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonScripts.CreateLessonScript.CreateLessonScriptCommand { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", FullText = "Sample description", SummarizeText = "Sample description", LessonOutcome = "SampleValue", CoveragePercent = 1m },
            CreateLessonScriptCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.LessonScripts.CreateLessonScript.CreateLessonScriptCommand { LessonId = Guid.Parse("00000000-0000-0000-0000-000000000000"), RecordId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Language = "Invalid", FullText = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", SummarizeText = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", LessonOutcome = "", CoveragePercent = 1m },
            CreateLessonScriptCase.InvalidLessonExistsMissing => new global::EngConnect.Application.UseCases.LessonScripts.CreateLessonScript.CreateLessonScriptCommand { LessonId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), RecordId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Language = "En", FullText = "Sample description", SummarizeText = "Sample description", LessonOutcome = "SampleValue", CoveragePercent = 1m },
            CreateLessonScriptCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonScripts.CreateLessonScript.CreateLessonScriptCommand { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", FullText = "Sample description", SummarizeText = "Sample description", LessonOutcome = "SampleValue", CoveragePercent = 1m },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}