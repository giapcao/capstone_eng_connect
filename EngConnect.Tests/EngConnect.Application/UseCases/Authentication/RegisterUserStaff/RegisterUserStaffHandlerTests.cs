using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterUserStaff;

public class RegisterUserStaffHandlerTests
{
    [Theory]
    [MemberData(nameof(RegisterUserStaffTestData.NormalHandlerCases), MemberType = typeof(RegisterUserStaffTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RegisterUserStaffTestData.Definition, caseSet);
    }
}