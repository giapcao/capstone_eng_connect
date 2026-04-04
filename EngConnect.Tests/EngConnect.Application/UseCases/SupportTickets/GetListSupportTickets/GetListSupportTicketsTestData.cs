using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets;

internal enum GetListSupportTicketsCase
{
    ValidDefault,
    ValidWithoutCreatedBy,
    ValidEmptyStatus,
    ValidEmptyType,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListSupportTicketsTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListSupportTickets",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets.GetListSupportTicketQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets.GetListSupportTicketQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/GetListSupportTickets/GetListSupportTicketQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/GetListSupportTickets/GetListSupportTicketQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListSupportTicketsCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListSupportTicketsCase.ValidWithoutCreatedBy),
        BuildCase(GetListSupportTicketsCase.ValidEmptyStatus),
        BuildCase(GetListSupportTicketsCase.ValidEmptyType),
        BuildCase(GetListSupportTicketsCase.BoundaryDefault),
        BuildCase(GetListSupportTicketsCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListSupportTicketsCase.ValidDefault),
        BuildCase(GetListSupportTicketsCase.ValidWithoutCreatedBy),
        BuildCase(GetListSupportTicketsCase.ValidEmptyStatus),
        BuildCase(GetListSupportTicketsCase.ValidEmptyType),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListSupportTicketsCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListSupportTicketsCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListSupportTicketsCase caseId)
    {
        return caseId switch
        {
            GetListSupportTicketsCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListSupportTicketsCase.ValidWithoutCreatedBy => CreateCase("valid-without-CreatedBy", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListSupportTicketsCase.ValidEmptyStatus => CreateCase("valid-empty-Status", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListSupportTicketsCase.ValidEmptyType => CreateCase("valid-empty-Type", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListSupportTicketsCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListSupportTicketsCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets.GetListSupportTicketQuery CreateRequest(GetListSupportTicketsCase caseId)
    {
        return caseId switch
        {
            GetListSupportTicketsCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets.GetListSupportTicketQuery { Status = "Open", Type = "Error", CreatedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListSupportTicketsCase.ValidWithoutCreatedBy => new global::EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets.GetListSupportTicketQuery { Status = "Open", Type = "Error", CreatedBy = null, PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListSupportTicketsCase.ValidEmptyStatus => new global::EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets.GetListSupportTicketQuery { Status = "", Type = "Error", CreatedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListSupportTicketsCase.ValidEmptyType => new global::EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets.GetListSupportTicketQuery { Status = "Open", Type = "", CreatedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListSupportTicketsCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets.GetListSupportTicketQuery { Status = "Open", Type = "Error", CreatedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListSupportTicketsCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets.GetListSupportTicketQuery { Status = "Open", Type = "Error", CreatedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}