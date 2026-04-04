using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest;

internal enum GetListLessonRescheduleRequestCase
{
    ValidDefault,
    ValidWithoutLessonId,
    ValidWithoutStudentId,
    ValidEmptyStatus,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListLessonRescheduleRequestTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListLessonRescheduleRequest",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest.GetListLessonRescheduleRequestQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest.GetListLessonRescheduleRequestQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/GetListLessonRescheduleRequest/GetListLessonRescheduleRequestQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/GetListLessonRescheduleRequest/GetListLessonRescheduleRequestQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListLessonRescheduleRequestCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListLessonRescheduleRequestCase.ValidWithoutLessonId),
        BuildCase(GetListLessonRescheduleRequestCase.ValidWithoutStudentId),
        BuildCase(GetListLessonRescheduleRequestCase.ValidEmptyStatus),
        BuildCase(GetListLessonRescheduleRequestCase.BoundaryDefault),
        BuildCase(GetListLessonRescheduleRequestCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListLessonRescheduleRequestCase.ValidDefault),
        BuildCase(GetListLessonRescheduleRequestCase.ValidWithoutLessonId),
        BuildCase(GetListLessonRescheduleRequestCase.ValidWithoutStudentId),
        BuildCase(GetListLessonRescheduleRequestCase.ValidEmptyStatus),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListLessonRescheduleRequestCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListLessonRescheduleRequestCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListLessonRescheduleRequestCase caseId)
    {
        return caseId switch
        {
            GetListLessonRescheduleRequestCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListLessonRescheduleRequestCase.ValidWithoutLessonId => CreateCase("valid-without-LessonId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListLessonRescheduleRequestCase.ValidWithoutStudentId => CreateCase("valid-without-StudentId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListLessonRescheduleRequestCase.ValidEmptyStatus => CreateCase("valid-empty-Status", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListLessonRescheduleRequestCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListLessonRescheduleRequestCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest.GetListLessonRescheduleRequestQuery CreateRequest(GetListLessonRescheduleRequestCase caseId)
    {
        return caseId switch
        {
            GetListLessonRescheduleRequestCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest.GetListLessonRescheduleRequestQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111"), "Pending") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonRescheduleRequestCase.ValidWithoutLessonId => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest.GetListLessonRescheduleRequestQuery(null, Guid.Parse("11111111-1111-1111-1111-111111111111"), "Pending") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonRescheduleRequestCase.ValidWithoutStudentId => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest.GetListLessonRescheduleRequestQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), null, "Pending") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonRescheduleRequestCase.ValidEmptyStatus => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest.GetListLessonRescheduleRequestQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111"), "") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonRescheduleRequestCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest.GetListLessonRescheduleRequestQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111"), "Pending") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonRescheduleRequestCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest.GetListLessonRescheduleRequestQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111"), "Pending") { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}