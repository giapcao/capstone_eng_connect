using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule;

internal enum GetListTutorScheduleCase
{
    ValidDefault,
    ValidWithoutTutorId,
    ValidEmptyWeekday,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListTutorScheduleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListTutorSchedule",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule.GetListTutorScheduleQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule.GetListTutorScheduleQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/GetListTutorSchedule/GetListTutorScheduleQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/GetListTutorSchedule/GetListTutorScheduleQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListTutorScheduleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListTutorScheduleCase.ValidWithoutTutorId),
        BuildCase(GetListTutorScheduleCase.ValidEmptyWeekday),
        BuildCase(GetListTutorScheduleCase.BoundaryDefault),
        BuildCase(GetListTutorScheduleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListTutorScheduleCase.ValidDefault),
        BuildCase(GetListTutorScheduleCase.ValidWithoutTutorId),
        BuildCase(GetListTutorScheduleCase.ValidEmptyWeekday),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListTutorScheduleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListTutorScheduleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListTutorScheduleCase caseId)
    {
        return caseId switch
        {
            GetListTutorScheduleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorScheduleCase.ValidWithoutTutorId => CreateCase("valid-without-TutorId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorScheduleCase.ValidEmptyWeekday => CreateCase("valid-empty-Weekday", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorScheduleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorScheduleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule.GetListTutorScheduleQuery CreateRequest(GetListTutorScheduleCase caseId)
    {
        return caseId switch
        {
            GetListTutorScheduleCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule.GetListTutorScheduleQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), "SampleValue") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorScheduleCase.ValidWithoutTutorId => new global::EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule.GetListTutorScheduleQuery(null, "SampleValue") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorScheduleCase.ValidEmptyWeekday => new global::EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule.GetListTutorScheduleQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), "") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorScheduleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule.GetListTutorScheduleQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), "SampleValue") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorScheduleCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule.GetListTutorScheduleQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), "SampleValue") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}