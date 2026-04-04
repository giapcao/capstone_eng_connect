using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.GetUserById;

public class GetUserByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetUserByIdTestData.NormalHandlerCases), MemberType = typeof(GetUserByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetUserByIdTestData.Definition, caseSet);
    }
}