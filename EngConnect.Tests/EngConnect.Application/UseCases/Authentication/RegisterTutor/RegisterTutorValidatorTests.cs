using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterTutor;

public class RegisterTutorValidatorTests
{
    [Theory]
    [MemberData(nameof(RegisterTutorTestData.NormalValidatorCases), MemberType = typeof(RegisterTutorTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RegisterTutorTestData.Definition, caseSet);
    }
}