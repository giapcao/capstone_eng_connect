using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;

public class CreateLessonRecordBranchTests
{
    [Theory]
    [MemberData(nameof(CreateLessonRecordTestData.HandlerBranchCases), MemberType = typeof(CreateLessonRecordTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateLessonRecordTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateLessonRecordTestData.ValidatorBranchCases), MemberType = typeof(CreateLessonRecordTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateLessonRecordTestData.Definition, caseSet);
    }
}