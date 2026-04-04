using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule;

internal enum UpdateTutorScheduleCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidEntityMissing,
    ExceptionPath,
}

internal static class UpdateTutorScheduleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateTutorSchedule",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/UpdateTutorSchedule/UpdateTutorScheduleCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/UpdateTutorSchedule/UpdateTutorScheduleCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/UpdateTutorSchedule/UpdateTutorScheduleCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateTutorScheduleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateTutorScheduleCase.BoundaryDefault),
        BuildCase(UpdateTutorScheduleCase.InvalidRequestShape),
        BuildCase(UpdateTutorScheduleCase.InvalidEntityMissing),
        BuildCase(UpdateTutorScheduleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateTutorScheduleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateTutorScheduleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateTutorScheduleCase.InvalidRequestShape),
        BuildCase(UpdateTutorScheduleCase.InvalidEntityMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateTutorScheduleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateTutorScheduleCase caseId)
    {
        return caseId switch
        {
            UpdateTutorScheduleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateTutorScheduleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateTutorScheduleCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateTutorScheduleCase.InvalidEntityMissing => CreateCase("invalid-entity-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateTutorScheduleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleCommand CreateRequest(UpdateTutorScheduleCase caseId)
    {
        return caseId switch
        {
            UpdateTutorScheduleCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleRequest { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Weekday = "SampleValue", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(10, 0, 0) }),
            UpdateTutorScheduleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleRequest { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Weekday = "SampleValue", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(10, 0, 0) }),
            UpdateTutorScheduleCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleRequest { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Weekday = "", StartTime = new TimeOnly(10, 0, 0), EndTime = new TimeOnly(8, 0, 0) }),
            UpdateTutorScheduleCase.InvalidEntityMissing => new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleRequest { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Weekday = "SampleValue", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(10, 0, 0) }),
            UpdateTutorScheduleCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule.UpdateTutorScheduleRequest { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Weekday = "SampleValue", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(10, 0, 0) }),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}