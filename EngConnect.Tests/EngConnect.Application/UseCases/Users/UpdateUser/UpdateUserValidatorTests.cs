using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.UpdateUser;

public class UpdateUserValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateUserTestData.NormalValidatorCases), MemberType = typeof(UpdateUserTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateUserTestData.Definition, caseSet);
    }
}