using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.GetUserById;

internal enum GetUserByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidNotFoundOrNull,
    ExceptionPath,
}

internal static class GetUserByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetUserById",
        RequestTypeFullName = "EngConnect.Application.UseCases.Users.GetUserById.GetUserByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Users.GetUserById.GetUserByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Users/GetUserById/GetUserByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Users/GetUserById/GetUserByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetUserByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetUserByIdCase.BoundaryDefault),
        BuildCase(GetUserByIdCase.InvalidNotFoundOrNull),
        BuildCase(GetUserByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetUserByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetUserByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetUserByIdCase.InvalidNotFoundOrNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetUserByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetUserByIdCase caseId)
    {
        return caseId switch
        {
            GetUserByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetUserByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetUserByIdCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            GetUserByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Users.GetUserById.GetUserByIdQuery CreateRequest(GetUserByIdCase caseId)
    {
        return caseId switch
        {
            GetUserByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.Users.GetUserById.GetUserByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetUserByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Users.GetUserById.GetUserByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetUserByIdCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.Users.GetUserById.GetUserByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetUserByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.Users.GetUserById.GetUserByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}