using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.DeleteStudent;

internal enum DeleteStudentCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidNotFoundOrNull,
    ExceptionPath,
}

internal static class DeleteStudentTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteStudent",
        RequestTypeFullName = "EngConnect.Application.UseCases.Students.DeleteStudent.DeleteStudentCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Students.DeleteStudent.DeleteStudentCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Students/DeleteStudent/DeleteStudentCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Students/DeleteStudent/DeleteStudentCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteStudentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteStudentCase.BoundaryDefault),
        BuildCase(DeleteStudentCase.InvalidNotFoundOrNull),
        BuildCase(DeleteStudentCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteStudentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteStudentCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteStudentCase.InvalidNotFoundOrNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteStudentCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteStudentCase caseId)
    {
        return caseId switch
        {
            DeleteStudentCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteStudentCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteStudentCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            DeleteStudentCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Students.DeleteStudent.DeleteStudentCommand CreateRequest(DeleteStudentCase caseId)
    {
        return caseId switch
        {
            DeleteStudentCase.ValidDefault => new global::EngConnect.Application.UseCases.Students.DeleteStudent.DeleteStudentCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteStudentCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Students.DeleteStudent.DeleteStudentCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteStudentCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.Students.DeleteStudent.DeleteStudentCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteStudentCase.ExceptionPath => new global::EngConnect.Application.UseCases.Students.DeleteStudent.DeleteStudentCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}