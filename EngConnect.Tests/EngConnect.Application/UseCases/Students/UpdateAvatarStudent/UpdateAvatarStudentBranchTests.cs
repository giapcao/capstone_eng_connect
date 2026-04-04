using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateAvatarStudent;

public class UpdateAvatarStudentBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateAvatarStudentTestData.HandlerBranchCases), MemberType = typeof(UpdateAvatarStudentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateAvatarStudentTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateAvatarStudentTestData.ValidatorBranchCases), MemberType = typeof(UpdateAvatarStudentTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateAvatarStudentTestData.Definition, caseSet);
    }
}