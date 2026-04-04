using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket;

internal enum DeleteSupportTicketCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidSupportTicketMissing,
    ExceptionPath,
}

internal static class DeleteSupportTicketTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteSupportTicket",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket.DeleteSupportTicketCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket.DeleteSupportTicketCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/DeleteSupportTicket/DeleteSupportTicketCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/DeleteSupportTicket/DeleteSupportTicketCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteSupportTicketCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteSupportTicketCase.BoundaryDefault),
        BuildCase(DeleteSupportTicketCase.InvalidSupportTicketMissing),
        BuildCase(DeleteSupportTicketCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteSupportTicketCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteSupportTicketCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteSupportTicketCase.InvalidSupportTicketMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteSupportTicketCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteSupportTicketCase caseId)
    {
        return caseId switch
        {
            DeleteSupportTicketCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteSupportTicketCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteSupportTicketCase.InvalidSupportTicketMissing => CreateCase("invalid-supportTicket-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteSupportTicketCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket.DeleteSupportTicketCommand CreateRequest(DeleteSupportTicketCase caseId)
    {
        return caseId switch
        {
            DeleteSupportTicketCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket.DeleteSupportTicketCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteSupportTicketCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket.DeleteSupportTicketCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteSupportTicketCase.InvalidSupportTicketMissing => new global::EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket.DeleteSupportTicketCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            DeleteSupportTicketCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket.DeleteSupportTicketCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}