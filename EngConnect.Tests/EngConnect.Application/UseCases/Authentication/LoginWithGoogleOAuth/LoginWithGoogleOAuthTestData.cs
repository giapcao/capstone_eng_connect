using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth;

internal enum LoginWithGoogleOAuthCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestGuardPrincipal,
    InvalidNotFoundOrNull,
    ExceptionPath,
}

internal static class LoginWithGoogleOAuthTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "LoginWithGoogleOAuth",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth.LoginWithGoogleOAuthCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth.LoginWithGoogleOAuthCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/LoginWithGoogleOAuth/LoginWithGoogleOAuthCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/LoginWithGoogleOAuth/LoginWithGoogleOAuthCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(LoginWithGoogleOAuthCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(LoginWithGoogleOAuthCase.BoundaryDefault),
        BuildCase(LoginWithGoogleOAuthCase.InvalidRequestGuardPrincipal),
        BuildCase(LoginWithGoogleOAuthCase.InvalidNotFoundOrNull),
        BuildCase(LoginWithGoogleOAuthCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(LoginWithGoogleOAuthCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(LoginWithGoogleOAuthCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(LoginWithGoogleOAuthCase.InvalidRequestGuardPrincipal),
        BuildCase(LoginWithGoogleOAuthCase.InvalidNotFoundOrNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(LoginWithGoogleOAuthCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(LoginWithGoogleOAuthCase caseId)
    {
        return caseId switch
        {
            LoginWithGoogleOAuthCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            LoginWithGoogleOAuthCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            LoginWithGoogleOAuthCase.InvalidRequestGuardPrincipal => CreateCase("invalid-request-guard-Principal", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            LoginWithGoogleOAuthCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            LoginWithGoogleOAuthCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth.LoginWithGoogleOAuthCommand CreateRequest(LoginWithGoogleOAuthCase caseId)
    {
        return caseId switch
        {
            LoginWithGoogleOAuthCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth.LoginWithGoogleOAuthCommand { Principal = TestObjectFactory.CreatePrincipal() },
            LoginWithGoogleOAuthCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth.LoginWithGoogleOAuthCommand { Principal = TestObjectFactory.CreatePrincipal() },
            LoginWithGoogleOAuthCase.InvalidRequestGuardPrincipal => new global::EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth.LoginWithGoogleOAuthCommand { Principal = null },
            LoginWithGoogleOAuthCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth.LoginWithGoogleOAuthCommand { Principal = TestObjectFactory.CreatePrincipal() },
            LoginWithGoogleOAuthCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth.LoginWithGoogleOAuthCommand { Principal = TestObjectFactory.CreatePrincipal() },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}