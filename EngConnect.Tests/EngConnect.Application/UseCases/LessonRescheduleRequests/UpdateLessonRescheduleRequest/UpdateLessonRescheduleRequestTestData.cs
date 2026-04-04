using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest;

internal enum UpdateLessonRescheduleRequestCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidRequestMissing,
    ExceptionPath,
}

internal static class UpdateLessonRescheduleRequestTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateLessonRescheduleRequest",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequestCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequestCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequestCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/UpdateLessonRescheduleRequest/UpdateLessonRescheduleRequestCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/UpdateLessonRescheduleRequest/UpdateLessonRescheduleRequestCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/LessonRescheduleRequests/UpdateLessonRescheduleRequest/UpdateLessonRescheduleRequestCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateLessonRescheduleRequestCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateLessonRescheduleRequestCase.BoundaryDefault),
        BuildCase(UpdateLessonRescheduleRequestCase.InvalidRequestShape),
        BuildCase(UpdateLessonRescheduleRequestCase.InvalidRequestMissing),
        BuildCase(UpdateLessonRescheduleRequestCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateLessonRescheduleRequestCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateLessonRescheduleRequestCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateLessonRescheduleRequestCase.InvalidRequestShape),
        BuildCase(UpdateLessonRescheduleRequestCase.InvalidRequestMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateLessonRescheduleRequestCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateLessonRescheduleRequestCase caseId)
    {
        return caseId switch
        {
            UpdateLessonRescheduleRequestCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonRescheduleRequestCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonRescheduleRequestCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateLessonRescheduleRequestCase.InvalidRequestMissing => CreateCase("invalid-request-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonRescheduleRequestCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequestCommand CreateRequest(UpdateLessonRescheduleRequestCase caseId)
    {
        return caseId switch
        {
            UpdateLessonRescheduleRequestCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequestCommand(new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequest { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Pending", TutorNote = "SampleValue" }),
            UpdateLessonRescheduleRequestCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequestCommand(new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequest { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Pending", TutorNote = "SampleValue" }),
            UpdateLessonRescheduleRequestCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequestCommand(new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequest { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Status = "Invalid", TutorNote = "" }),
            UpdateLessonRescheduleRequestCase.InvalidRequestMissing => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequestCommand(new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequest { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Status = "Pending", TutorNote = "SampleValue" }),
            UpdateLessonRescheduleRequestCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequestCommand(new global::EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest.UpdateLessonRescheduleRequest { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Pending", TutorNote = "SampleValue" }),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}