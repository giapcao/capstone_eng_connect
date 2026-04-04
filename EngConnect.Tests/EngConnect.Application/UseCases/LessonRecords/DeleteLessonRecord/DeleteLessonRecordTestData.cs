using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord;

internal enum DeleteLessonRecordCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidLessonRecordMissing,
    ExceptionPath,
}

internal static class DeleteLessonRecordTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteLessonRecord",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord.DeleteLessonRecordCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord.DeleteLessonRecordCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/DeleteLessonRecord/DeleteLessonRecordCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/DeleteLessonRecord/DeleteLessonRecordCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteLessonRecordCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteLessonRecordCase.BoundaryDefault),
        BuildCase(DeleteLessonRecordCase.InvalidLessonRecordMissing),
        BuildCase(DeleteLessonRecordCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteLessonRecordCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteLessonRecordCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteLessonRecordCase.InvalidLessonRecordMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteLessonRecordCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteLessonRecordCase caseId)
    {
        return caseId switch
        {
            DeleteLessonRecordCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteLessonRecordCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteLessonRecordCase.InvalidLessonRecordMissing => CreateCase("invalid-lessonRecord-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteLessonRecordCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord.DeleteLessonRecordCommand CreateRequest(DeleteLessonRecordCase caseId)
    {
        return caseId switch
        {
            DeleteLessonRecordCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord.DeleteLessonRecordCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteLessonRecordCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord.DeleteLessonRecordCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            DeleteLessonRecordCase.InvalidLessonRecordMissing => new global::EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord.DeleteLessonRecordCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            DeleteLessonRecordCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord.DeleteLessonRecordCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}