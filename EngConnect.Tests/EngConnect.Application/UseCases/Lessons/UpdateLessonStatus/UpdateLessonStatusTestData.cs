using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;

internal enum UpdateLessonStatusCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidLessonMissing,
    ExceptionPath,
}

internal static class UpdateLessonStatusTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateLessonStatus",
        RequestTypeFullName = "EngConnect.Application.UseCases.Lessons.UpdateLessonStatus.UpdateLessonStatusCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Lessons.UpdateLessonStatus.UpdateLessonStatusCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Lessons.UpdateLessonStatus.UpdateLessonStatusCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Lessons/UpdateLessonStatus/UpdateLessonStatusCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Lessons/UpdateLessonStatus/UpdateLessonStatusCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Lessons/UpdateLessonStatus/UpdateLessonStatusCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateLessonStatusCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateLessonStatusCase.BoundaryDefault),
        BuildCase(UpdateLessonStatusCase.InvalidRequestShape),
        BuildCase(UpdateLessonStatusCase.InvalidLessonMissing),
        BuildCase(UpdateLessonStatusCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateLessonStatusCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateLessonStatusCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateLessonStatusCase.InvalidRequestShape),
        BuildCase(UpdateLessonStatusCase.InvalidLessonMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateLessonStatusCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateLessonStatusCase caseId)
    {
        return caseId switch
        {
            UpdateLessonStatusCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonStatusCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonStatusCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateLessonStatusCase.InvalidLessonMissing => CreateCase("invalid-lesson-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateLessonStatusCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Lessons.UpdateLessonStatus.UpdateLessonStatusCommand CreateRequest(UpdateLessonStatusCase caseId)
    {
        return caseId switch
        {
            UpdateLessonStatusCase.ValidDefault => new global::EngConnect.Application.UseCases.Lessons.UpdateLessonStatus.UpdateLessonStatusCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Scheduled" },
            UpdateLessonStatusCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Lessons.UpdateLessonStatus.UpdateLessonStatusCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Scheduled" },
            UpdateLessonStatusCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Lessons.UpdateLessonStatus.UpdateLessonStatusCommand { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Status = "Invalid" },
            UpdateLessonStatusCase.InvalidLessonMissing => new global::EngConnect.Application.UseCases.Lessons.UpdateLessonStatus.UpdateLessonStatusCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Status = "Scheduled" },
            UpdateLessonStatusCase.ExceptionPath => new global::EngConnect.Application.UseCases.Lessons.UpdateLessonStatus.UpdateLessonStatusCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Status = "Scheduled" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}