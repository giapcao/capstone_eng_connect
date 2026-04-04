using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.DeleteTutor;

internal enum DeleteTutorCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidEntityMissing,
    ExceptionPath,
}

internal static class DeleteTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.DeleteTutor.DeleteTutorCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.DeleteTutor.DeleteTutorCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/DeleteTutor/DeleteTutorCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/DeleteTutor/DeleteTutorCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteTutorCase.BoundaryDefault),
        BuildCase(DeleteTutorCase.InvalidEntityMissing),
        BuildCase(DeleteTutorCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteTutorCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteTutorCase.InvalidEntityMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteTutorCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteTutorCase caseId)
    {
        return caseId switch
        {
            DeleteTutorCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteTutorCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteTutorCase.InvalidEntityMissing => CreateCase("invalid-entity-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteTutorCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Tutors.DeleteTutor.DeleteTutorCommand CreateRequest(DeleteTutorCase caseId)
    {
        return caseId switch
        {
            DeleteTutorCase.ValidDefault => new global::EngConnect.Application.UseCases.Tutors.DeleteTutor.DeleteTutorCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteTutorCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Tutors.DeleteTutor.DeleteTutorCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteTutorCase.InvalidEntityMissing => new global::EngConnect.Application.UseCases.Tutors.DeleteTutor.DeleteTutorCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            DeleteTutorCase.ExceptionPath => new global::EngConnect.Application.UseCases.Tutors.DeleteTutor.DeleteTutorCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}