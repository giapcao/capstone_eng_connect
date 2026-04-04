using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.GetMeetingInfo;

internal enum GetMeetingInfoCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidLessonMissing,
    ExceptionPath,
}

internal static class GetMeetingInfoTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetMeetingInfo",
        RequestTypeFullName = "EngConnect.Application.UseCases.Meetings.GetMeetingInfo.GetMeetingInfoQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Meetings.GetMeetingInfo.GetMeetingInfoQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Meetings/GetMeetingInfo/GetMeetingInfoQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Meetings/GetMeetingInfo/GetMeetingInfoQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetMeetingInfoCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetMeetingInfoCase.BoundaryDefault),
        BuildCase(GetMeetingInfoCase.InvalidLessonMissing),
        BuildCase(GetMeetingInfoCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetMeetingInfoCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetMeetingInfoCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetMeetingInfoCase.InvalidLessonMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetMeetingInfoCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetMeetingInfoCase caseId)
    {
        return caseId switch
        {
            GetMeetingInfoCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetMeetingInfoCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetMeetingInfoCase.InvalidLessonMissing => CreateCase("invalid-lesson-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetMeetingInfoCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Meetings.GetMeetingInfo.GetMeetingInfoQuery CreateRequest(GetMeetingInfoCase caseId)
    {
        return caseId switch
        {
            GetMeetingInfoCase.ValidDefault => new global::EngConnect.Application.UseCases.Meetings.GetMeetingInfo.GetMeetingInfoQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetMeetingInfoCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Meetings.GetMeetingInfo.GetMeetingInfoQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetMeetingInfoCase.InvalidLessonMissing => new global::EngConnect.Application.UseCases.Meetings.GetMeetingInfo.GetMeetingInfoQuery(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetMeetingInfoCase.ExceptionPath => new global::EngConnect.Application.UseCases.Meetings.GetMeetingInfo.GetMeetingInfoQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}