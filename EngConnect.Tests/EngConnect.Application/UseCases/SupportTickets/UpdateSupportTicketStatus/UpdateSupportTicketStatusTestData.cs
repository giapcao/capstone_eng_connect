using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus;

internal enum UpdateSupportTicketStatusCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidSupportTicketMissing,
    ExceptionPath,
}

internal static class UpdateSupportTicketStatusTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateSupportTicketStatus",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus.UpdateSupportTicketStatusCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus.UpdateSupportTicketStatusCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus.UpdateSupportTicketStatusCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/UpdateSupportTicketStatus/UpdateSupportTicketStatusCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/UpdateSupportTicketStatus/UpdateSupportTicketStatusCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/UpdateSupportTicketStatus/UpdateSupportTicketStatusCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateSupportTicketStatusCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateSupportTicketStatusCase.BoundaryDefault),
        BuildCase(UpdateSupportTicketStatusCase.InvalidRequestShape),
        BuildCase(UpdateSupportTicketStatusCase.InvalidSupportTicketMissing),
        BuildCase(UpdateSupportTicketStatusCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateSupportTicketStatusCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateSupportTicketStatusCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateSupportTicketStatusCase.InvalidRequestShape),
        BuildCase(UpdateSupportTicketStatusCase.InvalidSupportTicketMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateSupportTicketStatusCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateSupportTicketStatusCase caseId)
    {
        return caseId switch
        {
            UpdateSupportTicketStatusCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateSupportTicketStatusCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateSupportTicketStatusCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateSupportTicketStatusCase.InvalidSupportTicketMissing => CreateCase("invalid-supportTicket-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateSupportTicketStatusCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus.UpdateSupportTicketStatusCommand CreateRequest(UpdateSupportTicketStatusCase caseId)
    {
        return caseId switch
        {
            UpdateSupportTicketStatusCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus.UpdateSupportTicketStatusCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Open" },
            UpdateSupportTicketStatusCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus.UpdateSupportTicketStatusCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Open" },
            UpdateSupportTicketStatusCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus.UpdateSupportTicketStatusCommand { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Status = "Invalid" },
            UpdateSupportTicketStatusCase.InvalidSupportTicketMissing => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus.UpdateSupportTicketStatusCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Status = "Open" },
            UpdateSupportTicketStatusCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus.UpdateSupportTicketStatusCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Open" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}