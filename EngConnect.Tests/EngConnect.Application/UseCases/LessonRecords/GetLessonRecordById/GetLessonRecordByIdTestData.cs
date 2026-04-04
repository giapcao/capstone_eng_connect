using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById;

internal enum GetLessonRecordByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidLessonRecordMissing,
    ExceptionPath,
}

internal static class GetLessonRecordByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetLessonRecordById",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById.GetLessonRecordByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById.GetLessonRecordByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/GetLessonRecordById/GetLessonRecordByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/GetLessonRecordById/GetLessonRecordByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetLessonRecordByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetLessonRecordByIdCase.BoundaryDefault),
        BuildCase(GetLessonRecordByIdCase.InvalidLessonRecordMissing),
        BuildCase(GetLessonRecordByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetLessonRecordByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetLessonRecordByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetLessonRecordByIdCase.InvalidLessonRecordMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetLessonRecordByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetLessonRecordByIdCase caseId)
    {
        return caseId switch
        {
            GetLessonRecordByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonRecordByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonRecordByIdCase.InvalidLessonRecordMissing => CreateCase("invalid-lessonRecord-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetLessonRecordByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById.GetLessonRecordByIdQuery CreateRequest(GetLessonRecordByIdCase caseId)
    {
        return caseId switch
        {
            GetLessonRecordByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById.GetLessonRecordByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetLessonRecordByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById.GetLessonRecordByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetLessonRecordByIdCase.InvalidLessonRecordMissing => new global::EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById.GetLessonRecordByIdQuery { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            GetLessonRecordByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById.GetLessonRecordByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}