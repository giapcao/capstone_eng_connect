using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterTutor;

public class RegisterTutorBranchTests
{
    [Theory]
    [MemberData(nameof(RegisterTutorTestData.HandlerBranchCases), MemberType = typeof(RegisterTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RegisterTutorTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(RegisterTutorTestData.ValidatorBranchCases), MemberType = typeof(RegisterTutorTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RegisterTutorTestData.Definition, caseSet);
    }
}