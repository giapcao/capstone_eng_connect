using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage;

internal enum UpdateSupportTicketMessageCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidSupportTicketMessageMissing,
    ExceptionPath,
}

internal static class UpdateSupportTicketMessageTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateSupportTicketMessage",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage.UpdateSupportTicketMessageCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage.UpdateSupportTicketMessageCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage.UpdateSupportTicketMessageCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/UpdateSupportTicketMessage/UpdateSupportTicketMessageCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/UpdateSupportTicketMessage/UpdateSupportTicketMessageCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/UpdateSupportTicketMessage/UpdateSupportTicketMessageCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateSupportTicketMessageCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateSupportTicketMessageCase.BoundaryDefault),
        BuildCase(UpdateSupportTicketMessageCase.InvalidRequestShape),
        BuildCase(UpdateSupportTicketMessageCase.InvalidSupportTicketMessageMissing),
        BuildCase(UpdateSupportTicketMessageCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateSupportTicketMessageCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateSupportTicketMessageCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateSupportTicketMessageCase.InvalidRequestShape),
        BuildCase(UpdateSupportTicketMessageCase.InvalidSupportTicketMessageMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateSupportTicketMessageCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateSupportTicketMessageCase caseId)
    {
        return caseId switch
        {
            UpdateSupportTicketMessageCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateSupportTicketMessageCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateSupportTicketMessageCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateSupportTicketMessageCase.InvalidSupportTicketMessageMissing => CreateCase("invalid-supportTicketMessage-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateSupportTicketMessageCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage.UpdateSupportTicketMessageCommand CreateRequest(UpdateSupportTicketMessageCase caseId)
    {
        return caseId switch
        {
            UpdateSupportTicketMessageCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage.UpdateSupportTicketMessageCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Message = "SampleValue" },
            UpdateSupportTicketMessageCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage.UpdateSupportTicketMessageCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Message = "SampleValue" },
            UpdateSupportTicketMessageCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage.UpdateSupportTicketMessageCommand { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Message = "" },
            UpdateSupportTicketMessageCase.InvalidSupportTicketMessageMissing => new global::EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage.UpdateSupportTicketMessageCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Message = "SampleValue" },
            UpdateSupportTicketMessageCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage.UpdateSupportTicketMessageCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Message = "SampleValue" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}