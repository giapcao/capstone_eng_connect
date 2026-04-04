using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById;

internal enum GetLessonRescheduleRequestByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidEntityMissing,
    ExceptionPath,
}

internal static class GetLessonRescheduleRequestByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetLessonRescheduleRequestById",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById.GetLessonRescheduleRequestByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById.GetLessonRescheduleRequestByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/GetLessonRescheduleRequestById/GetLessonRescheduleRequestByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/GetLessonRescheduleRequestById/GetLessonRescheduleRequestByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetLessonRescheduleRequestByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetLessonRescheduleRequestByIdCase.BoundaryDefault),
        BuildCase(GetLessonRescheduleRequestByIdCase.InvalidEntityMissing),
        BuildCase(GetLessonRescheduleRequestByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetLessonRescheduleRequestByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetLessonRescheduleRequestByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetLessonRescheduleRequestByIdCase.InvalidEntityMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetLessonRescheduleRequestByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetLessonRescheduleRequestByIdCase caseId)
    {
        return caseId switch
        {
            GetLessonRescheduleRequestByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonRescheduleRequestByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonRescheduleRequestByIdCase.InvalidEntityMissing => CreateCase("invalid-entity-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonRescheduleRequestByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById.GetLessonRescheduleRequestByIdQuery CreateRequest(GetLessonRescheduleRequestByIdCase caseId)
    {
        return caseId switch
        {
            GetLessonRescheduleRequestByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById.GetLessonRescheduleRequestByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetLessonRescheduleRequestByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById.GetLessonRescheduleRequestByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetLessonRescheduleRequestByIdCase.InvalidEntityMissing => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById.GetLessonRescheduleRequestByIdQuery(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            GetLessonRescheduleRequestByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById.GetLessonRescheduleRequestByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}