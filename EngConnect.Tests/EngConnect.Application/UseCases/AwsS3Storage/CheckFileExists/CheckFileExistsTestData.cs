using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists;

internal enum CheckFileExistsCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class CheckFileExistsTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CheckFileExists",
        RequestTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists.CheckFileExistsQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists.CheckFileExistsQueryHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists.CheckFileExistsQueryValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/CheckFileExists/CheckFileExistsQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/CheckFileExists/CheckFileExistsQueryHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/AwsS3Storage/CheckFileExists/CheckFileExistsQueryValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CheckFileExistsCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CheckFileExistsCase.BoundaryDefault),
        BuildCase(CheckFileExistsCase.InvalidRequestShape),
        BuildCase(CheckFileExistsCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CheckFileExistsCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CheckFileExistsCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CheckFileExistsCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CheckFileExistsCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CheckFileExistsCase caseId)
    {
        return caseId switch
        {
            CheckFileExistsCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CheckFileExistsCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CheckFileExistsCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CheckFileExistsCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists.CheckFileExistsQuery CreateRequest(CheckFileExistsCase caseId)
    {
        return caseId switch
        {
            CheckFileExistsCase.ValidDefault => new global::EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists.CheckFileExistsQuery { FileName = "Sample" },
            CheckFileExistsCase.BoundaryDefault => new global::EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists.CheckFileExistsQuery { FileName = "Sample" },
            CheckFileExistsCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists.CheckFileExistsQuery { FileName = "" },
            CheckFileExistsCase.ExceptionPath => new global::EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists.CheckFileExistsQuery { FileName = "Sample" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}