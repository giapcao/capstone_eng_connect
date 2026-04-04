using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.UpdateCategory;

internal enum UpdateCategoryCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidCategoryMissing,
    InvalidNameExistsExisting,
    ExceptionPath,
}

internal static class UpdateCategoryTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateCategory",
        RequestTypeFullName = "EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Categories/UpdateCategory/UpdateCategoryCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Categories/UpdateCategory/UpdateCategoryCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Categories/UpdateCategory/UpdateCategoryCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateCategoryCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateCategoryCase.BoundaryDefault),
        BuildCase(UpdateCategoryCase.InvalidRequestShape),
        BuildCase(UpdateCategoryCase.InvalidCategoryMissing),
        BuildCase(UpdateCategoryCase.InvalidNameExistsExisting),
        BuildCase(UpdateCategoryCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateCategoryCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateCategoryCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateCategoryCase.InvalidRequestShape),
        BuildCase(UpdateCategoryCase.InvalidCategoryMissing),
        BuildCase(UpdateCategoryCase.InvalidNameExistsExisting),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateCategoryCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateCategoryCase caseId)
    {
        return caseId switch
        {
            UpdateCategoryCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateCategoryCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateCategoryCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateCategoryCase.InvalidCategoryMissing => CreateCase("invalid-category-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateCategoryCase.InvalidNameExistsExisting => CreateDuplicateNameCase(),
            UpdateCategoryCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommand CreateRequest(UpdateCategoryCase caseId)
    {
        return caseId switch
        {
            UpdateCategoryCase.ValidDefault => new global::EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Sample", Description = "Sample description", Type = "General" },
            UpdateCategoryCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Sample", Description = "Sample description", Type = "General" },
            UpdateCategoryCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommand { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Name = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Description = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Type = "Invalid" },
            UpdateCategoryCase.InvalidCategoryMissing => new global::EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "Sample", Description = "Sample description", Type = "General" },
            UpdateCategoryCase.InvalidNameExistsExisting => new global::EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "seed-value", Description = "Sample description", Type = "seed-value" },
            UpdateCategoryCase.ExceptionPath => new global::EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Sample", Description = "Sample description", Type = "General" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateDuplicateNameCase()
    {
        var request = new global::EngConnect.Application.UseCases.Categories.UpdateCategory.UpdateCategoryCommand
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Speaking",
            Description = "Updated description",
            Type = "Skill"
        };

        var currentCategory = new Category
        {
            Id = request.Id,
            Name = "Grammar",
            Description = "Current description",
            Type = "Skill"
        };

        return new UseCaseCaseSet
        {
            Name = "invalid-nameExists-existing",
            Kind = UseCaseCaseKind.Invalid,
            HandlerExpectation = UseCaseHandlerExpectation.Failure,
            ValidatorExpectation = UseCaseValidatorExpectation.Pass,
            TestCase = new UseCaseTestCase
            {
                Name = "invalid-nameExists-existing",
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = mocks =>
                    {
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var repositoryMock = new Mock<IGenericRepository<Category, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Category, Guid>())
                            .Returns(repositoryMock.Object);

                        repositoryMock
                            .Setup(repository => repository.FindSingleAsync(
                                It.IsAny<Expression<Func<Category, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Category, object>>[]>()))
                            .ReturnsAsync(currentCategory);

                        repositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<Category, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Category, object>>[]>()))
                            .ReturnsAsync(true);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                        Assert.Equal("CategoryNameExists", result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }
}
