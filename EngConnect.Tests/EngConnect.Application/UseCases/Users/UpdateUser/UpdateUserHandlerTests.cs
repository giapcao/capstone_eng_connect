using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.UpdateUser;

public class UpdateUserHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateUserTestData.NormalHandlerCases), MemberType = typeof(UpdateUserTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateUserTestData.Definition, caseSet);
    }
}