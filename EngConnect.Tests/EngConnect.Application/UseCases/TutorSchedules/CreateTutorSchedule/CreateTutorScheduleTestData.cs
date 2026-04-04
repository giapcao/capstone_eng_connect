using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule;

internal enum CreateTutorScheduleCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidTutorMissing,
    ExceptionPath,
}

internal static class CreateTutorScheduleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateTutorSchedule",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/CreateTutorSchedule/CreateTutorScheduleCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/CreateTutorSchedule/CreateTutorScheduleCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/CreateTutorSchedule/CreateTutorScheduleCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateTutorScheduleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateTutorScheduleCase.BoundaryDefault),
        BuildCase(CreateTutorScheduleCase.InvalidRequestShape),
        BuildCase(CreateTutorScheduleCase.InvalidTutorMissing),
        BuildCase(CreateTutorScheduleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreateTutorScheduleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateTutorScheduleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateTutorScheduleCase.InvalidRequestShape),
        BuildCase(CreateTutorScheduleCase.InvalidTutorMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateTutorScheduleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreateTutorScheduleCase caseId)
    {
        return caseId switch
        {
            CreateTutorScheduleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateTutorScheduleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateTutorScheduleCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateTutorScheduleCase.InvalidTutorMissing => CreateCase("invalid-tutor-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateTutorScheduleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleCommand CreateRequest(CreateTutorScheduleCase caseId)
    {
        return caseId switch
        {
            CreateTutorScheduleCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleRequest { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Weekday = "SampleValue", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(10, 0, 0) }),
            CreateTutorScheduleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleRequest { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Weekday = "SampleValue", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(10, 0, 0) }),
            CreateTutorScheduleCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleRequest { TutorId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Weekday = "", StartTime = new TimeOnly(10, 0, 0), EndTime = new TimeOnly(8, 0, 0) }),
            CreateTutorScheduleCase.InvalidTutorMissing => new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleRequest { TutorId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Weekday = "SampleValue", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(10, 0, 0) }),
            CreateTutorScheduleCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleCommand(new global::EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule.CreateTutorScheduleRequest { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Weekday = "SampleValue", StartTime = new TimeOnly(8, 0, 0), EndTime = new TimeOnly(10, 0, 0) }),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}