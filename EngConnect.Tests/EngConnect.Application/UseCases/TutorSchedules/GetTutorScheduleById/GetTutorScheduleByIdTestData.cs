using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById;

internal enum GetTutorScheduleByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidEntityMissing,
    ExceptionPath,
}

internal static class GetTutorScheduleByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetTutorScheduleById",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById.GetTutorScheduleByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById.GetTutorScheduleByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/GetTutorScheduleById/GetTutorScheduleByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/GetTutorScheduleById/GetTutorScheduleByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetTutorScheduleByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetTutorScheduleByIdCase.BoundaryDefault),
        BuildCase(GetTutorScheduleByIdCase.InvalidEntityMissing),
        BuildCase(GetTutorScheduleByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetTutorScheduleByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetTutorScheduleByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetTutorScheduleByIdCase.InvalidEntityMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetTutorScheduleByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetTutorScheduleByIdCase caseId)
    {
        return caseId switch
        {
            GetTutorScheduleByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetTutorScheduleByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetTutorScheduleByIdCase.InvalidEntityMissing => CreateCase("invalid-entity-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetTutorScheduleByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById.GetTutorScheduleByIdQuery CreateRequest(GetTutorScheduleByIdCase caseId)
    {
        return caseId switch
        {
            GetTutorScheduleByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById.GetTutorScheduleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetTutorScheduleByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById.GetTutorScheduleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetTutorScheduleByIdCase.InvalidEntityMissing => new global::EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById.GetTutorScheduleByIdQuery(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            GetTutorScheduleByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById.GetTutorScheduleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}