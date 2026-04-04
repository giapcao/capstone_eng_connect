using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateStudent;

public class UpdateStudentBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateStudentTestData.HandlerBranchCases), MemberType = typeof(UpdateStudentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateStudentTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateStudentTestData.ValidatorBranchCases), MemberType = typeof(UpdateStudentTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateStudentTestData.Definition, caseSet);
    }
}