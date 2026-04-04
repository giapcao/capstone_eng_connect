using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateTutor;

internal enum UpdateTutorCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidEntityMissing,
    ExceptionPath,
}

internal static class UpdateTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateTutor/UpdateTutorCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateTutor/UpdateTutorCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Tutors/UpdateTutor/UpdateTutorCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateTutorCase.BoundaryDefault),
        BuildCase(UpdateTutorCase.InvalidRequestShape),
        BuildCase(UpdateTutorCase.InvalidEntityMissing),
        BuildCase(UpdateTutorCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateTutorCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateTutorCase.InvalidRequestShape),
        BuildCase(UpdateTutorCase.InvalidEntityMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateTutorCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateTutorCase caseId)
    {
        return caseId switch
        {
            UpdateTutorCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateTutorCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateTutorCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateTutorCase.InvalidEntityMissing => CreateCase("invalid-entity-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateTutorCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorCommand CreateRequest(UpdateTutorCase caseId)
    {
        return caseId switch
        {
            UpdateTutorCase.ValidDefault => new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorRequest { Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, Status = "Active" }),
            UpdateTutorCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorRequest { Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, Status = "Active" }),
            UpdateTutorCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorCommand(Guid.Parse("00000000-0000-0000-0000-000000000000"), new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorRequest { Headline = "", Bio = "", MonthExperience = 0, Status = "Invalid" }),
            UpdateTutorCase.InvalidEntityMissing => new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorRequest { Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, Status = "Active" }),
            UpdateTutorCase.ExceptionPath => new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new global::EngConnect.Application.UseCases.Tutors.UpdateTutor.UpdateTutorRequest { Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, Status = "Active" }),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}