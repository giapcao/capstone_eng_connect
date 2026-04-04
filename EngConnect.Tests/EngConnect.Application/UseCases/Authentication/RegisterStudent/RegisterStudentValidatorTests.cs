using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterStudent;

public class RegisterStudentValidatorTests
{
    [Theory]
    [MemberData(nameof(RegisterStudentTestData.NormalValidatorCases), MemberType = typeof(RegisterStudentTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RegisterStudentTestData.Definition, caseSet);
    }
}