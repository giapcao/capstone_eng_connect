using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ForgotPassword;

internal enum ForgotPasswordCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidUserMissing,
    ExceptionPath,
}

internal static class ForgotPasswordTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "ForgotPassword",
        RequestTypeFullName = "EngConnect.Application.UseCases.Users.ForgotPassword.ForgotPasswordCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Users.ForgotPassword.ForgotPasswordCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Users.ForgotPassword.ForgotPasswordCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Users/ForgotPassword/ForgotPasswordCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Users/ForgotPassword/ForgotPasswordCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Users/ForgotPassword/ForgotPasswordCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(ForgotPasswordCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(ForgotPasswordCase.BoundaryDefault),
        BuildCase(ForgotPasswordCase.InvalidRequestShape),
        BuildCase(ForgotPasswordCase.InvalidUserMissing),
        BuildCase(ForgotPasswordCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(ForgotPasswordCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(ForgotPasswordCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(ForgotPasswordCase.InvalidRequestShape),
        BuildCase(ForgotPasswordCase.InvalidUserMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(ForgotPasswordCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(ForgotPasswordCase caseId)
    {
        return caseId switch
        {
            ForgotPasswordCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            ForgotPasswordCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            ForgotPasswordCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            ForgotPasswordCase.InvalidUserMissing => CreateCase("invalid-user-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            ForgotPasswordCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Users.ForgotPassword.ForgotPasswordCommand CreateRequest(ForgotPasswordCase caseId)
    {
        return caseId switch
        {
            ForgotPasswordCase.ValidDefault => new global::EngConnect.Application.UseCases.Users.ForgotPassword.ForgotPasswordCommand { Email = "tester@example.com" },
            ForgotPasswordCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Users.ForgotPassword.ForgotPasswordCommand { Email = "tester@example.com" },
            ForgotPasswordCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Users.ForgotPassword.ForgotPasswordCommand { Email = "" },
            ForgotPasswordCase.InvalidUserMissing => new global::EngConnect.Application.UseCases.Users.ForgotPassword.ForgotPasswordCommand { Email = "missing@example.com" },
            ForgotPasswordCase.ExceptionPath => new global::EngConnect.Application.UseCases.Users.ForgotPassword.ForgotPasswordCommand { Email = "tester@example.com" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}