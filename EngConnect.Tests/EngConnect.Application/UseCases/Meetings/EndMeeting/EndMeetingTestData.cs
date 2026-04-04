using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.EndMeeting;

internal enum EndMeetingCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidLessonMissing,
    InvalidTutorMissing,
    ExceptionPath,
}

internal static class EndMeetingTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "EndMeeting",
        RequestTypeFullName = "EngConnect.Application.UseCases.Meetings.EndMeeting.EndMeetingCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Meetings.EndMeeting.EndMeetingCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Meetings/EndMeeting/EndMeetingCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Meetings/EndMeeting/EndMeetingCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(EndMeetingCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(EndMeetingCase.BoundaryDefault),
        BuildCase(EndMeetingCase.InvalidLessonMissing),
        BuildCase(EndMeetingCase.InvalidTutorMissing),
        BuildCase(EndMeetingCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(EndMeetingCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(EndMeetingCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(EndMeetingCase.InvalidLessonMissing),
        BuildCase(EndMeetingCase.InvalidTutorMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(EndMeetingCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(EndMeetingCase caseId)
    {
        return caseId switch
        {
            EndMeetingCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            EndMeetingCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            EndMeetingCase.InvalidLessonMissing => CreateCase("invalid-lesson-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            EndMeetingCase.InvalidTutorMissing => CreateCase("invalid-tutor-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            EndMeetingCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Meetings.EndMeeting.EndMeetingCommand CreateRequest(EndMeetingCase caseId)
    {
        return caseId switch
        {
            EndMeetingCase.ValidDefault => new global::EngConnect.Application.UseCases.Meetings.EndMeeting.EndMeetingCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111"), 1),
            EndMeetingCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Meetings.EndMeeting.EndMeetingCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111"), 1),
            EndMeetingCase.InvalidLessonMissing => new global::EngConnect.Application.UseCases.Meetings.EndMeeting.EndMeetingCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Guid.Parse("11111111-1111-1111-1111-111111111111"), 1),
            EndMeetingCase.InvalidTutorMissing => new global::EngConnect.Application.UseCases.Meetings.EndMeeting.EndMeetingCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), 1),
            EndMeetingCase.ExceptionPath => new global::EngConnect.Application.UseCases.Meetings.EndMeeting.EndMeetingCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("11111111-1111-1111-1111-111111111111"), 1),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}