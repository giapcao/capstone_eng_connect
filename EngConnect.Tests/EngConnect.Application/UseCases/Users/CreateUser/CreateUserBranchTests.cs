using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.CreateUser;

public class CreateUserBranchTests
{
    [Theory]
    [MemberData(nameof(CreateUserTestData.HandlerBranchCases), MemberType = typeof(CreateUserTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateUserTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateUserTestData.ValidatorBranchCases), MemberType = typeof(CreateUserTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateUserTestData.Definition, caseSet);
    }
}