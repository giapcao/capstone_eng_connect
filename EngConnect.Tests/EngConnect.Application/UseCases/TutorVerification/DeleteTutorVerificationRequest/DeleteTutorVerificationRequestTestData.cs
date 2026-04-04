using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest;

internal enum DeleteTutorVerificationRequestCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidEntityMissing,
    ExceptionPath,
}

internal static class DeleteTutorVerificationRequestTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteTutorVerificationRequest",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest.DeleteTutorVerificationRequestCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest.DeleteTutorVerificationRequestCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/DeleteTutorVerificationRequest/DeleteTutorVerificationRequestCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/DeleteTutorVerificationRequest/DeleteTutorVerificationRequestCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteTutorVerificationRequestCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteTutorVerificationRequestCase.BoundaryDefault),
        BuildCase(DeleteTutorVerificationRequestCase.InvalidEntityMissing),
        BuildCase(DeleteTutorVerificationRequestCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteTutorVerificationRequestCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteTutorVerificationRequestCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteTutorVerificationRequestCase.InvalidEntityMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteTutorVerificationRequestCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteTutorVerificationRequestCase caseId)
    {
        return caseId switch
        {
            DeleteTutorVerificationRequestCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteTutorVerificationRequestCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteTutorVerificationRequestCase.InvalidEntityMissing => CreateCase("invalid-entity-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteTutorVerificationRequestCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest.DeleteTutorVerificationRequestCommand CreateRequest(DeleteTutorVerificationRequestCase caseId)
    {
        return caseId switch
        {
            DeleteTutorVerificationRequestCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest.DeleteTutorVerificationRequestCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteTutorVerificationRequestCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest.DeleteTutorVerificationRequestCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteTutorVerificationRequestCase.InvalidEntityMissing => new global::EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest.DeleteTutorVerificationRequestCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            DeleteTutorVerificationRequestCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest.DeleteTutorVerificationRequestCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}