using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.UpdateUser;

public class UpdateUserBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateUserTestData.HandlerBranchCases), MemberType = typeof(UpdateUserTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateUserTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateUserTestData.ValidatorBranchCases), MemberType = typeof(UpdateUserTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateUserTestData.Definition, caseSet);
    }
}