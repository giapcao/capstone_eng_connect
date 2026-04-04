using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest;

internal enum GetListTutorVerificationRequestCase
{
    ValidDefault,
    ValidWithoutTutorId,
    ValidWithoutReviewedBy,
    ValidEmptyStatus,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListTutorVerificationRequestTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListTutorVerificationRequest",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest.GetListTutorVerificationRequestQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest.GetListTutorVerificationRequestQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/GetListTutorVerificationRequest/GetListTutorVerificationRequestQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorVerification/GetListTutorVerificationRequest/GetListTutorVerificationRequestQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListTutorVerificationRequestCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListTutorVerificationRequestCase.ValidWithoutTutorId),
        BuildCase(GetListTutorVerificationRequestCase.ValidWithoutReviewedBy),
        BuildCase(GetListTutorVerificationRequestCase.ValidEmptyStatus),
        BuildCase(GetListTutorVerificationRequestCase.BoundaryDefault),
        BuildCase(GetListTutorVerificationRequestCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListTutorVerificationRequestCase.ValidDefault),
        BuildCase(GetListTutorVerificationRequestCase.ValidWithoutTutorId),
        BuildCase(GetListTutorVerificationRequestCase.ValidWithoutReviewedBy),
        BuildCase(GetListTutorVerificationRequestCase.ValidEmptyStatus),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListTutorVerificationRequestCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListTutorVerificationRequestCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListTutorVerificationRequestCase caseId)
    {
        return caseId switch
        {
            GetListTutorVerificationRequestCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorVerificationRequestCase.ValidWithoutTutorId => CreateCase("valid-without-TutorId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorVerificationRequestCase.ValidWithoutReviewedBy => CreateCase("valid-without-ReviewedBy", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorVerificationRequestCase.ValidEmptyStatus => CreateCase("valid-empty-Status", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorVerificationRequestCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorVerificationRequestCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest.GetListTutorVerificationRequestQuery CreateRequest(GetListTutorVerificationRequestCase caseId)
    {
        return caseId switch
        {
            GetListTutorVerificationRequestCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest.GetListTutorVerificationRequestQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Active", ReviewedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorVerificationRequestCase.ValidWithoutTutorId => new global::EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest.GetListTutorVerificationRequestQuery { TutorId = null, Status = "Active", ReviewedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorVerificationRequestCase.ValidWithoutReviewedBy => new global::EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest.GetListTutorVerificationRequestQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Active", ReviewedBy = null, PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorVerificationRequestCase.ValidEmptyStatus => new global::EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest.GetListTutorVerificationRequestQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "", ReviewedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorVerificationRequestCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest.GetListTutorVerificationRequestQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Active", ReviewedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorVerificationRequestCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest.GetListTutorVerificationRequestQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Active", ReviewedBy = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}