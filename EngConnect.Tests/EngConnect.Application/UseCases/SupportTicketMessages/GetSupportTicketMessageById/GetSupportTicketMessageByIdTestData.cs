using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById;

internal enum GetSupportTicketMessageByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidSupportTicketMessageMissing,
    ExceptionPath,
}

internal static class GetSupportTicketMessageByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetSupportTicketMessageById",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById.GetSupportTicketMessageByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById.GetSupportTicketMessageByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/GetSupportTicketMessageById/GetSupportTicketMessageByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/GetSupportTicketMessageById/GetSupportTicketMessageByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetSupportTicketMessageByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetSupportTicketMessageByIdCase.BoundaryDefault),
        BuildCase(GetSupportTicketMessageByIdCase.InvalidSupportTicketMessageMissing),
        BuildCase(GetSupportTicketMessageByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetSupportTicketMessageByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetSupportTicketMessageByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetSupportTicketMessageByIdCase.InvalidSupportTicketMessageMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetSupportTicketMessageByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetSupportTicketMessageByIdCase caseId)
    {
        return caseId switch
        {
            GetSupportTicketMessageByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetSupportTicketMessageByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetSupportTicketMessageByIdCase.InvalidSupportTicketMessageMissing => CreateCase("invalid-supportTicketMessage-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetSupportTicketMessageByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById.GetSupportTicketMessageByIdQuery CreateRequest(GetSupportTicketMessageByIdCase caseId)
    {
        return caseId switch
        {
            GetSupportTicketMessageByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById.GetSupportTicketMessageByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetSupportTicketMessageByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById.GetSupportTicketMessageByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetSupportTicketMessageByIdCase.InvalidSupportTicketMessageMissing => new global::EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById.GetSupportTicketMessageByIdQuery { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            GetSupportTicketMessageByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById.GetSupportTicketMessageByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}