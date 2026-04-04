using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterStudent;

public class RegisterStudentHandlerTests
{
    [Theory]
    [MemberData(nameof(RegisterStudentTestData.NormalHandlerCases), MemberType = typeof(RegisterStudentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RegisterStudentTestData.Definition, caseSet);
    }
}