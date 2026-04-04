using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket;

internal enum CreateSupportTicketCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidNotFoundOrNull,
    ExceptionPath,
}

internal static class CreateSupportTicketTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateSupportTicket",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket.CreateSupportTicketCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket.CreateSupportTicketCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket.CreateSupportTicketCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/CreateSupportTicket/CreateSupportTicketCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/CreateSupportTicket/CreateSupportTicketCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/CreateSupportTicket/CreateSupportTicketCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateSupportTicketCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateSupportTicketCase.BoundaryDefault),
        BuildCase(CreateSupportTicketCase.InvalidRequestShape),
        BuildCase(CreateSupportTicketCase.InvalidNotFoundOrNull),
        BuildCase(CreateSupportTicketCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreateSupportTicketCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateSupportTicketCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateSupportTicketCase.InvalidRequestShape),
        BuildCase(CreateSupportTicketCase.InvalidNotFoundOrNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateSupportTicketCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreateSupportTicketCase caseId)
    {
        return caseId switch
        {
            CreateSupportTicketCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateSupportTicketCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateSupportTicketCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateSupportTicketCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            CreateSupportTicketCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket.CreateSupportTicketCommand CreateRequest(CreateSupportTicketCase caseId)
    {
        return caseId switch
        {
            CreateSupportTicketCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket.CreateSupportTicketCommand { CreatedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), Subject = "English Basics", Description = "Sample description", Type = "Error" },
            CreateSupportTicketCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket.CreateSupportTicketCommand { CreatedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), Subject = "English Basics", Description = "Sample description", Type = "Error" },
            CreateSupportTicketCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket.CreateSupportTicketCommand { CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"), Subject = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Description = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Type = "Invalid" },
            CreateSupportTicketCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket.CreateSupportTicketCommand { CreatedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), Subject = "English Basics", Description = "Sample description", Type = "Error" },
            CreateSupportTicketCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket.CreateSupportTicketCommand { CreatedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), Subject = "English Basics", Description = "Sample description", Type = "Error" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}