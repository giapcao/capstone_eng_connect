using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.DeleteCategory;

internal enum DeleteCategoryCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidCategoryMissing,
    ExceptionPath,
}

internal static class DeleteCategoryTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteCategory",
        RequestTypeFullName = "EngConnect.Application.UseCases.Categories.DeleteCategory.DeleteCategoryCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Categories.DeleteCategory.DeleteCategoryCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Categories/DeleteCategory/DeleteCategoryCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Categories/DeleteCategory/DeleteCategoryCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteCategoryCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteCategoryCase.BoundaryDefault),
        BuildCase(DeleteCategoryCase.InvalidCategoryMissing),
        BuildCase(DeleteCategoryCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteCategoryCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteCategoryCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteCategoryCase.InvalidCategoryMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteCategoryCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteCategoryCase caseId)
    {
        return caseId switch
        {
            DeleteCategoryCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteCategoryCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteCategoryCase.InvalidCategoryMissing => CreateCase("invalid-category-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteCategoryCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Categories.DeleteCategory.DeleteCategoryCommand CreateRequest(DeleteCategoryCase caseId)
    {
        return caseId switch
        {
            DeleteCategoryCase.ValidDefault => new global::EngConnect.Application.UseCases.Categories.DeleteCategory.DeleteCategoryCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteCategoryCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Categories.DeleteCategory.DeleteCategoryCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteCategoryCase.InvalidCategoryMissing => new global::EngConnect.Application.UseCases.Categories.DeleteCategory.DeleteCategoryCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            DeleteCategoryCase.ExceptionPath => new global::EngConnect.Application.UseCases.Categories.DeleteCategory.DeleteCategoryCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}