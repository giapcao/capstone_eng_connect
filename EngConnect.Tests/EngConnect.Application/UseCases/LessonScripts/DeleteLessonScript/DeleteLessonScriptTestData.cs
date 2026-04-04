using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript;

internal enum DeleteLessonScriptCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidLessonScriptExistsMissing,
    ExceptionPath,
}

internal static class DeleteLessonScriptTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteLessonScript",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript.DeleteLessonScriptCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript.DeleteLessonScriptCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/DeleteLessonScript/DeleteLessonScriptCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/DeleteLessonScript/DeleteLessonScriptCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteLessonScriptCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteLessonScriptCase.BoundaryDefault),
        BuildCase(DeleteLessonScriptCase.InvalidLessonScriptExistsMissing),
        BuildCase(DeleteLessonScriptCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteLessonScriptCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteLessonScriptCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteLessonScriptCase.InvalidLessonScriptExistsMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteLessonScriptCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteLessonScriptCase caseId)
    {
        return caseId switch
        {
            DeleteLessonScriptCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteLessonScriptCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteLessonScriptCase.InvalidLessonScriptExistsMissing => CreateCase("invalid-lessonScriptExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteLessonScriptCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript.DeleteLessonScriptCommand CreateRequest(DeleteLessonScriptCase caseId)
    {
        return caseId switch
        {
            DeleteLessonScriptCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript.DeleteLessonScriptCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteLessonScriptCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript.DeleteLessonScriptCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteLessonScriptCase.InvalidLessonScriptExistsMissing => new global::EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript.DeleteLessonScriptCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            DeleteLessonScriptCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript.DeleteLessonScriptCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}