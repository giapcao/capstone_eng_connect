using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.GetListUser;

public class GetListUserHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListUserTestData.NormalHandlerCases), MemberType = typeof(GetListUserTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListUserTestData.Definition, caseSet);
    }
}