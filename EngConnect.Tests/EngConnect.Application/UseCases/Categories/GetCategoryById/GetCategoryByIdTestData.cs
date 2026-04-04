using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.GetCategoryById;

internal enum GetCategoryByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidNotFoundOrNull,
    ExceptionPath,
}

internal static class GetCategoryByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetCategoryById",
        RequestTypeFullName = "EngConnect.Application.UseCases.Categories.GetCategoryById.GetCategoryByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Categories.GetCategoryById.GetCategoryByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Categories/GetCategoryById/GetCategoryByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Categories/GetCategoryById/GetCategoryByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetCategoryByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetCategoryByIdCase.BoundaryDefault),
        BuildCase(GetCategoryByIdCase.InvalidNotFoundOrNull),
        BuildCase(GetCategoryByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetCategoryByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetCategoryByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetCategoryByIdCase.InvalidNotFoundOrNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetCategoryByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetCategoryByIdCase caseId)
    {
        return caseId switch
        {
            GetCategoryByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetCategoryByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetCategoryByIdCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            GetCategoryByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Categories.GetCategoryById.GetCategoryByIdQuery CreateRequest(GetCategoryByIdCase caseId)
    {
        return caseId switch
        {
            GetCategoryByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.Categories.GetCategoryById.GetCategoryByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetCategoryByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Categories.GetCategoryById.GetCategoryByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetCategoryByIdCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.Categories.GetCategoryById.GetCategoryByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetCategoryByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.Categories.GetCategoryById.GetCategoryByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}