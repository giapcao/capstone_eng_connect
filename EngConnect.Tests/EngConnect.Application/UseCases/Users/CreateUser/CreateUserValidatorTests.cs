using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.CreateUser;

public class CreateUserValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateUserTestData.NormalValidatorCases), MemberType = typeof(CreateUserTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateUserTestData.Definition, caseSet);
    }
}