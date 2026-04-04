using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById;

internal enum GetLessonScriptByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidLessonScriptMissing,
    ExceptionPath,
}

internal static class GetLessonScriptByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetLessonScriptById",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById.GetLessonScriptByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById.GetLessonScriptByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/GetLessonScriptById/GetLessonScriptByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/GetLessonScriptById/GetLessonScriptByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetLessonScriptByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetLessonScriptByIdCase.BoundaryDefault),
        BuildCase(GetLessonScriptByIdCase.InvalidLessonScriptMissing),
        BuildCase(GetLessonScriptByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetLessonScriptByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetLessonScriptByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetLessonScriptByIdCase.InvalidLessonScriptMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetLessonScriptByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetLessonScriptByIdCase caseId)
    {
        return caseId switch
        {
            GetLessonScriptByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonScriptByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonScriptByIdCase.InvalidLessonScriptMissing => CreateCase("invalid-lessonScript-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonScriptByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById.GetLessonScriptByIdQuery CreateRequest(GetLessonScriptByIdCase caseId)
    {
        return caseId switch
        {
            GetLessonScriptByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById.GetLessonScriptByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetLessonScriptByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById.GetLessonScriptByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetLessonScriptByIdCase.InvalidLessonScriptMissing => new global::EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById.GetLessonScriptByIdQuery { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            GetLessonScriptByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById.GetLessonScriptByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}