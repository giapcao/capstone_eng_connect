using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.UpdateLessonRecord;

public class UpdateLessonRecordBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonRecordTestData.HandlerBranchCases), MemberType = typeof(UpdateLessonRecordTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonRecordTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateLessonRecordTestData.ValidatorBranchCases), MemberType = typeof(UpdateLessonRecordTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonRecordTestData.Definition, caseSet);
    }
}