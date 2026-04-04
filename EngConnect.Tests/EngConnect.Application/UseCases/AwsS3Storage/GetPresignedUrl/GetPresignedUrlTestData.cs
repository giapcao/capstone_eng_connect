using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl;

internal enum GetPresignedUrlCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class GetPresignedUrlTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetPresignedUrl",
        RequestTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl.GetPresignedUrlQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl.GetPresignedUrlQueryHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl.GetPresignedUrlQueryValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/GetPresignedUrl/GetPresignedUrlQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/GetPresignedUrl/GetPresignedUrlQueryHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/GetPresignedUrl/GetPresignedUrlQueryValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetPresignedUrlCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetPresignedUrlCase.BoundaryDefault),
        BuildCase(GetPresignedUrlCase.InvalidRequestShape),
        BuildCase(GetPresignedUrlCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetPresignedUrlCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetPresignedUrlCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetPresignedUrlCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetPresignedUrlCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetPresignedUrlCase caseId)
    {
        return caseId switch
        {
            GetPresignedUrlCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetPresignedUrlCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetPresignedUrlCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            GetPresignedUrlCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl.GetPresignedUrlQuery CreateRequest(GetPresignedUrlCase caseId)
    {
        return caseId switch
        {
            GetPresignedUrlCase.ValidDefault => new global::EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl.GetPresignedUrlQuery { FileName = "Sample", DurationMinutes = 1 },
            GetPresignedUrlCase.BoundaryDefault => new global::EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl.GetPresignedUrlQuery { FileName = "Sample", DurationMinutes = 1 },
            GetPresignedUrlCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl.GetPresignedUrlQuery { FileName = "", DurationMinutes = 0 },
            GetPresignedUrlCase.ExceptionPath => new global::EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl.GetPresignedUrlQuery { FileName = "Sample", DurationMinutes = 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}