using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule;

internal enum DeleteTutorScheduleCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidEntityMissing,
    ExceptionPath,
}

internal static class DeleteTutorScheduleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteTutorSchedule",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule.DeleteTutorScheduleCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule.DeleteTutorScheduleCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule.DeleteTutorScheduleCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/DeleteTutorSchedule/DeleteTutorScheduleCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/DeleteTutorSchedule/DeleteTutorScheduleCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/TutorSchedules/DeleteTutorSchedule/DeleteTutorScheduleCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteTutorScheduleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteTutorScheduleCase.BoundaryDefault),
        BuildCase(DeleteTutorScheduleCase.InvalidRequestShape),
        BuildCase(DeleteTutorScheduleCase.InvalidEntityMissing),
        BuildCase(DeleteTutorScheduleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteTutorScheduleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteTutorScheduleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteTutorScheduleCase.InvalidRequestShape),
        BuildCase(DeleteTutorScheduleCase.InvalidEntityMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteTutorScheduleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteTutorScheduleCase caseId)
    {
        return caseId switch
        {
            DeleteTutorScheduleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            DeleteTutorScheduleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            DeleteTutorScheduleCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            DeleteTutorScheduleCase.InvalidEntityMissing => CreateCase("invalid-entity-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            DeleteTutorScheduleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule.DeleteTutorScheduleCommand CreateRequest(DeleteTutorScheduleCase caseId)
    {
        return caseId switch
        {
            DeleteTutorScheduleCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule.DeleteTutorScheduleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteTutorScheduleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule.DeleteTutorScheduleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteTutorScheduleCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule.DeleteTutorScheduleCommand(Guid.Parse("00000000-0000-0000-0000-000000000000")),
            DeleteTutorScheduleCase.InvalidEntityMissing => new global::EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule.DeleteTutorScheduleCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            DeleteTutorScheduleCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule.DeleteTutorScheduleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}