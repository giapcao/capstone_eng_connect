using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.CreateUser;

public class CreateUserHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateUserTestData.NormalHandlerCases), MemberType = typeof(CreateUserTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateUserTestData.Definition, caseSet);
    }
}