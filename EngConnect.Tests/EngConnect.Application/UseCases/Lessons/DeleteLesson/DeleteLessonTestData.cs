using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.DeleteLesson;

internal enum DeleteLessonCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidLessonMissing,
    ExceptionPath,
}

internal static class DeleteLessonTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteLesson",
        RequestTypeFullName = "EngConnect.Application.UseCases.Lessons.DeleteLesson.DeleteLessonCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Lessons.DeleteLesson.DeleteLessonCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Lessons/DeleteLesson/DeleteLessonCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Lessons/DeleteLesson/DeleteLessonCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteLessonCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteLessonCase.BoundaryDefault),
        BuildCase(DeleteLessonCase.InvalidLessonMissing),
        BuildCase(DeleteLessonCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteLessonCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteLessonCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteLessonCase.InvalidLessonMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteLessonCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteLessonCase caseId)
    {
        return caseId switch
        {
            DeleteLessonCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteLessonCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteLessonCase.InvalidLessonMissing => CreateCase("invalid-lesson-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteLessonCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Lessons.DeleteLesson.DeleteLessonCommand CreateRequest(DeleteLessonCase caseId)
    {
        return caseId switch
        {
            DeleteLessonCase.ValidDefault => new global::EngConnect.Application.UseCases.Lessons.DeleteLesson.DeleteLessonCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteLessonCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Lessons.DeleteLesson.DeleteLessonCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteLessonCase.InvalidLessonMissing => new global::EngConnect.Application.UseCases.Lessons.DeleteLesson.DeleteLessonCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            DeleteLessonCase.ExceptionPath => new global::EngConnect.Application.UseCases.Lessons.DeleteLesson.DeleteLessonCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}