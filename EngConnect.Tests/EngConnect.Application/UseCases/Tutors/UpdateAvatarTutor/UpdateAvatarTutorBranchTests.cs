using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor;

public class UpdateAvatarTutorBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateAvatarTutorTestData.HandlerBranchCases), MemberType = typeof(UpdateAvatarTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateAvatarTutorTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateAvatarTutorTestData.ValidatorBranchCases), MemberType = typeof(UpdateAvatarTutorTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateAvatarTutorTestData.Definition, caseSet);
    }
}