using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.GetLessonById;

internal enum GetLessonByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidLessonMissing,
    ExceptionPath,
}

internal static class GetLessonByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetLessonById",
        RequestTypeFullName = "EngConnect.Application.UseCases.Lessons.GetLessonById.GetLessonByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Lessons.GetLessonById.GetLessonByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Lessons/GetLessonById/GetLessonByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Lessons/GetLessonById/GetLessonByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetLessonByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetLessonByIdCase.BoundaryDefault),
        BuildCase(GetLessonByIdCase.InvalidLessonMissing),
        BuildCase(GetLessonByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetLessonByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetLessonByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetLessonByIdCase.InvalidLessonMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetLessonByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetLessonByIdCase caseId)
    {
        return caseId switch
        {
            GetLessonByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonByIdCase.InvalidLessonMissing => CreateCase("invalid-lesson-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Lessons.GetLessonById.GetLessonByIdQuery CreateRequest(GetLessonByIdCase caseId)
    {
        return caseId switch
        {
            GetLessonByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.Lessons.GetLessonById.GetLessonByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetLessonByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Lessons.GetLessonById.GetLessonByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetLessonByIdCase.InvalidLessonMissing => new global::EngConnect.Application.UseCases.Lessons.GetLessonById.GetLessonByIdQuery { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            GetLessonByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.Lessons.GetLessonById.GetLessonByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}