using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor;

public class GetIntroVideoUrlTutorBranchTests
{
    [Theory]
    [MemberData(nameof(GetIntroVideoUrlTutorTestData.HandlerBranchCases), MemberType = typeof(GetIntroVideoUrlTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetIntroVideoUrlTutorTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetIntroVideoUrlTutorTestData.Definition.ValidatorTypeFullName));
    }
}