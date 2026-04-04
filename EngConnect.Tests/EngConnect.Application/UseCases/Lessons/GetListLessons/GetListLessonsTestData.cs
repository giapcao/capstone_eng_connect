using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.GetListLessons;

internal enum GetListLessonsCase
{
    ValidDefault,
    ValidWithoutStartTimeFrom,
    ValidWithoutStartTimeTo,
    ValidWithoutStudentId,
    ValidEmptyStatus,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class GetListLessonsTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListLessons",
        RequestTypeFullName = "EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQueryHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Lessons/GetListLessons/GetListLessonQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Lessons/GetListLessons/GetListLessonQueryHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Lessons/GetListLessons/GetListLessonValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListLessonsCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListLessonsCase.ValidWithoutStartTimeFrom),
        BuildCase(GetListLessonsCase.ValidWithoutStartTimeTo),
        BuildCase(GetListLessonsCase.ValidWithoutStudentId),
        BuildCase(GetListLessonsCase.ValidEmptyStatus),
        BuildCase(GetListLessonsCase.BoundaryDefault),
        BuildCase(GetListLessonsCase.InvalidRequestShape),
        BuildCase(GetListLessonsCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListLessonsCase.ValidDefault),
        BuildCase(GetListLessonsCase.ValidWithoutStartTimeFrom),
        BuildCase(GetListLessonsCase.ValidWithoutStartTimeTo),
        BuildCase(GetListLessonsCase.ValidWithoutStudentId),
        BuildCase(GetListLessonsCase.ValidEmptyStatus),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListLessonsCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetListLessonsCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListLessonsCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListLessonsCase caseId)
    {
        return caseId switch
        {
            GetListLessonsCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonsCase.ValidWithoutStartTimeFrom => CreateCase("valid-without-StartTimeFrom", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonsCase.ValidWithoutStartTimeTo => CreateCase("valid-without-StartTimeTo", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonsCase.ValidWithoutStudentId => CreateCase("valid-without-StudentId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonsCase.ValidEmptyStatus => CreateCase("valid-empty-Status", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonsCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonsCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            GetListLessonsCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery CreateRequest(GetListLessonsCase caseId)
    {
        return caseId switch
        {
            GetListLessonsCase.ValidDefault => new global::EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery { Status = "Scheduled", TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTimeFrom = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), StartTimeTo = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonsCase.ValidWithoutStartTimeFrom => new global::EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery { Status = "Scheduled", TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTimeFrom = null, StartTimeTo = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonsCase.ValidWithoutStartTimeTo => new global::EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery { Status = "Scheduled", TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTimeFrom = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), StartTimeTo = null, PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonsCase.ValidWithoutStudentId => new global::EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery { Status = "Scheduled", TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = null, StartTimeFrom = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), StartTimeTo = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonsCase.ValidEmptyStatus => new global::EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery { Status = "", TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTimeFrom = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), StartTimeTo = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonsCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery { Status = "Scheduled", TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTimeFrom = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), StartTimeTo = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonsCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery { Status = "Invalid", TutorId = Guid.Parse("00000000-0000-0000-0000-000000000000"), StudentId = Guid.Parse("00000000-0000-0000-0000-000000000000"), StartTimeFrom = new DateTime(2026, 4, 3, 13, 55, 4, DateTimeKind.Utc), StartTimeTo = new DateTime(2026, 4, 3, 13, 55, 4, DateTimeKind.Utc), PageNumber = 1, PageSize = 1, SearchTerm = "", SortParams = "" },
            GetListLessonsCase.ExceptionPath => new global::EngConnect.Application.UseCases.Lessons.GetListLessons.GetListLessonQuery { Status = "Scheduled", TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), StartTimeFrom = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), StartTimeTo = new DateTime(2026, 4, 3, 15, 55, 4, DateTimeKind.Utc), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}