using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords;

public class GetListLessonRecordsBranchTests
{
    [Theory]
    [MemberData(nameof(GetListLessonRecordsTestData.HandlerBranchCases), MemberType = typeof(GetListLessonRecordsTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListLessonRecordsTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(GetListLessonRecordsTestData.ValidatorBranchCases), MemberType = typeof(GetListLessonRecordsTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetListLessonRecordsTestData.Definition, caseSet);
    }
}