using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages;

internal enum GetListSupportTicketMessagesCase
{
    ValidDefault,
    ValidEmptyTicketId,
    ValidEmptySenderId,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListSupportTicketMessagesTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListSupportTicketMessages",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages.GetListSupportTicketMessageQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages.GetListSupportTicketMessageQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/GetListSupportTicketMessages/GetListSupportTicketMessageQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/GetListSupportTicketMessages/GetListSupportTicketMessageQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListSupportTicketMessagesCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListSupportTicketMessagesCase.ValidEmptyTicketId),
        BuildCase(GetListSupportTicketMessagesCase.ValidEmptySenderId),
        BuildCase(GetListSupportTicketMessagesCase.BoundaryDefault),
        BuildCase(GetListSupportTicketMessagesCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListSupportTicketMessagesCase.ValidDefault),
        BuildCase(GetListSupportTicketMessagesCase.ValidEmptyTicketId),
        BuildCase(GetListSupportTicketMessagesCase.ValidEmptySenderId),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListSupportTicketMessagesCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListSupportTicketMessagesCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListSupportTicketMessagesCase caseId)
    {
        return caseId switch
        {
            GetListSupportTicketMessagesCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListSupportTicketMessagesCase.ValidEmptyTicketId => CreateCase("valid-empty-TicketId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListSupportTicketMessagesCase.ValidEmptySenderId => CreateCase("valid-empty-SenderId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListSupportTicketMessagesCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListSupportTicketMessagesCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages.GetListSupportTicketMessageQuery CreateRequest(GetListSupportTicketMessagesCase caseId)
    {
        return caseId switch
        {
            GetListSupportTicketMessagesCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages.GetListSupportTicketMessageQuery { TicketId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SenderId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListSupportTicketMessagesCase.ValidEmptyTicketId => new global::EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages.GetListSupportTicketMessageQuery { TicketId = null, SenderId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListSupportTicketMessagesCase.ValidEmptySenderId => new global::EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages.GetListSupportTicketMessageQuery { TicketId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SenderId = null, PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListSupportTicketMessagesCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages.GetListSupportTicketMessageQuery { TicketId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SenderId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListSupportTicketMessagesCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages.GetListSupportTicketMessageQuery { TicketId = Guid.Parse("11111111-1111-1111-1111-111111111111"), SenderId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}