using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket;

internal enum UpdateSupportTicketCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidSupportTicketMissing,
    ExceptionPath,
}

internal static class UpdateSupportTicketTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateSupportTicket",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket.UpdateSupportTicketCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket.UpdateSupportTicketCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket.UpdateSupportTicketCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/UpdateSupportTicket/UpdateSupportTicketCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/UpdateSupportTicket/UpdateSupportTicketCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/UpdateSupportTicket/UpdateSupportTicketCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateSupportTicketCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateSupportTicketCase.BoundaryDefault),
        BuildCase(UpdateSupportTicketCase.InvalidRequestShape),
        BuildCase(UpdateSupportTicketCase.InvalidSupportTicketMissing),
        BuildCase(UpdateSupportTicketCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateSupportTicketCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateSupportTicketCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateSupportTicketCase.InvalidRequestShape),
        BuildCase(UpdateSupportTicketCase.InvalidSupportTicketMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateSupportTicketCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateSupportTicketCase caseId)
    {
        return caseId switch
        {
            UpdateSupportTicketCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateSupportTicketCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateSupportTicketCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateSupportTicketCase.InvalidSupportTicketMissing => CreateCase("invalid-supportTicket-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateSupportTicketCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket.UpdateSupportTicketCommand CreateRequest(UpdateSupportTicketCase caseId)
    {
        return caseId switch
        {
            UpdateSupportTicketCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket.UpdateSupportTicketCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Subject = "English Basics", Description = "Sample description", Type = "Error" },
            UpdateSupportTicketCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket.UpdateSupportTicketCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Subject = "English Basics", Description = "Sample description", Type = "Error" },
            UpdateSupportTicketCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket.UpdateSupportTicketCommand { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Subject = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Description = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Type = "Invalid" },
            UpdateSupportTicketCase.InvalidSupportTicketMissing => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket.UpdateSupportTicketCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Subject = "English Basics", Description = "Sample description", Type = "Error" },
            UpdateSupportTicketCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket.UpdateSupportTicketCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Subject = "English Basics", Description = "Sample description", Type = "Error" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}