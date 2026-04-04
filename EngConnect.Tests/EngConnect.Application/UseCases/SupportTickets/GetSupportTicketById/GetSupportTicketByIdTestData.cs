using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById;

internal enum GetSupportTicketByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidSupportTicketMissing,
    ExceptionPath,
}

internal static class GetSupportTicketByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetSupportTicketById",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById.GetSupportTicketByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById.GetSupportTicketByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/GetSupportTicketById/GetSupportTicketByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTickets/GetSupportTicketById/GetSupportTicketByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetSupportTicketByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetSupportTicketByIdCase.BoundaryDefault),
        BuildCase(GetSupportTicketByIdCase.InvalidSupportTicketMissing),
        BuildCase(GetSupportTicketByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetSupportTicketByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetSupportTicketByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetSupportTicketByIdCase.InvalidSupportTicketMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetSupportTicketByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetSupportTicketByIdCase caseId)
    {
        return caseId switch
        {
            GetSupportTicketByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetSupportTicketByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetSupportTicketByIdCase.InvalidSupportTicketMissing => CreateCase("invalid-supportTicket-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetSupportTicketByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById.GetSupportTicketByIdQuery CreateRequest(GetSupportTicketByIdCase caseId)
    {
        return caseId switch
        {
            GetSupportTicketByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById.GetSupportTicketByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetSupportTicketByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById.GetSupportTicketByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetSupportTicketByIdCase.InvalidSupportTicketMissing => new global::EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById.GetSupportTicketByIdQuery { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            GetSupportTicketByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById.GetSupportTicketByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}